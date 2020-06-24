using System;
using System.Collections.Generic;
using System.Net;

namespace SandHeadLiveApi.Infrastructure
{
    public class UserIdentifierCache
    {
        private Dictionary<IPAddress, int> _generatedIds;

        public UserIdentifierCache()
        {
            _generatedIds = new Dictionary<IPAddress, int>();
        }

        public string GetOrCreate(IPAddress ip)
        {
            if (_generatedIds.TryGetValue(ip, out var existingId))
                return existingId.ToString().PadLeft(4, '0');

            // generates a random identifier based on IP
            var ipBytes = ip.GetAddressBytes();
            Array.Reverse(ipBytes);
            var uintAddress = BitConverter.ToUInt32(ipBytes, 0);
            var intAddress = (int)Math.Round((double)uintAddress / uint.MaxValue * int.MaxValue);
            var random = new Random(intAddress);
            var newId = random.Next(9999) + 1;

            _generatedIds.TryAdd(ip, newId);
            return newId.ToString().PadLeft(4, '0');
        }
    }
}
