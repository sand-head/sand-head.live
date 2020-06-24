import { createElement, Context, Copy } from '@bikeshaving/crank';
import { SignalRClient, SIGNALR_KEY } from './SignalR';

interface Message {
  username: string;
  identifier: string;
  message: string;
  timestamp: Date;
}
export function* ChatLog(this: Context) {
  const client = this.get(SIGNALR_KEY) as SignalRClient;
  const messages: Message[] = [];
  const signalrCallback = (username: string, identifier: string, message: string) => {
    messages.push({
      username,
      identifier,
      message,
      timestamp: new Date()
    });
    this.refresh();
  };

  try {
    client.on('messageReceived', signalrCallback);
    while (true) {
      yield (
        <div id="messages">
          {messages.map(msg => (
            <div crank-id={msg.identifier + '-' + msg.timestamp.getTime()}>{msg.username}#{msg.identifier}: {msg.message}</div>
          ))}
          <ScrollToBottom />
        </div>
      );
    }
  } finally {
    client.off('messageReceived', signalrCallback);
  }
}

function *ScrollToBottom(this: Context) {
  let ref: Element;
  Promise.resolve().then(() => this.refresh());
  ref = yield <span />;

  while (true) {
    new Promise((resolve) => setTimeout(resolve, 10)).then(() => ref.scrollIntoView(true));
    yield <Copy />;
  }
}