using System;
using System.Threading;

namespace WhaTap.Trace.Utils
{
    public class ThreadLocalRandom
    {
        public static long GetTraceId()
        {
            return GenerateNext64();
        }

        public static ulong GetStepId()
        {
            return (ulong)GenerateNext64();
        }

        private static long GenerateNext64()
        {
            Random random = _random.Value;
            long result = random.Next();
            result = (result << 32) | (long)random.Next();
            return result;
        }

        private static int seed = ((int)DateTime.Now.Ticks) ^ System.Diagnostics.Process.GetCurrentProcess().Id;

        private static ThreadLocal<Random> _random =
            new ThreadLocal<Random>(() => new Random(Interlocked.Increment(ref seed)), trackAllValues: false);
    }
}
