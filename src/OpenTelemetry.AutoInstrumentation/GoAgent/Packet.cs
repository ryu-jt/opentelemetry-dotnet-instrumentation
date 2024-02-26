using System;

namespace WhaTap.Trace.GoAgent
{
    internal class Packet : IDisposable
    {
        public PacketType Type { get; }
        public Step Step { get; }

        public Packet(PacketType type, Step step)
        {
            Type = type;
            Step = step;
        }

        ~Packet()
        {
            Dispose();
        }

        public void Dispose()
        {
            Step?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
