using System;
using System.Buffers;
using System.IO.Pipelines;
using System.Threading;
using System.Threading.Tasks;

namespace BedrockRtmp.Messages
{
    public class RtmpMessageHandler
    {
        private readonly PipeReader _input;
        private readonly PipeWriter _output;
        private ReadOnlySequence<byte>? _buffer;
        private SequencePosition _consumed, _examined;
        private bool _isCanceled, _isCompleted;

        public RtmpMessageHandler(IDuplexPipe transport)
        {
            _buffer = null;
            _input = transport.Input;
            _output = transport.Output;
        }

        public async Task<TMessage> ReadAsync<TMessage>(CancellationToken cancellationToken = default) where TMessage : RtmpMessage, new()
        {
            // if we don't have a buffer yet, let's snag one
            if (!_buffer.HasValue)
            {
                var result = await _input.ReadAsync(cancellationToken);
                _buffer = result.Buffer;
                _consumed = result.Buffer.Start;
                _examined = result.Buffer.End;
                _isCanceled = result.IsCanceled;
                _isCompleted = result.IsCompleted;
            }

            while (true)
            {
                // todo: fix input advancing (it doesn't work for c0)
                // discovery: it's actually more that we need to keep track of positioning on our end
                // we should also keep the buffer on hand if we aren't done with it yet
                // doesn't work: _input.AdvanceTo(end);
                var buffer = _buffer.Value;
                if (TryRead(buffer, out TMessage message, ref _consumed, ref _examined))
                {
                    _buffer = buffer.Slice(_consumed);
                    return message;
                }
                else
                {
                    _input.AdvanceTo(_consumed, _examined);
                }

                if (_isCompleted)
                {
                    _consumed = default;
                    _examined = default;
                }
            }
        }

        public async Task WriteAsync<TMessage>(TMessage message, CancellationToken cancellationToken = default) where TMessage : RtmpMessage
        {
            Write(message);
            await _output.FlushAsync(cancellationToken);
        }

        private bool TryRead<TMessage>(ReadOnlySequence<byte> buffer, out TMessage message, ref SequencePosition consumed, ref SequencePosition examined) where TMessage : RtmpMessage, new()
        {
            var sequenceReader = new SequenceReader<byte>(buffer);
            message = new TMessage();
            try
            {
                message.Read(sequenceReader, ref consumed, ref examined);
            }
            catch
            {
                consumed = default;
                examined = default;
                return false;
            }
            return true;
        }

        private void Write<TMessage>(TMessage message) where TMessage : RtmpMessage
        {
            var size = message.CalculateSize();
            var outputBuffer = _output.GetSpan();
            message.Write(ref outputBuffer);
            _output.Advance(size);
        }
    }
}
