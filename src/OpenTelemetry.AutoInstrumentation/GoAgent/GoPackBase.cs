using System;
using System.IO;
using WhaTap.Trace.Utils;

namespace WhaTap.Trace.GoAgent
{
    public abstract class GoPackBase
    {
        public static byte[] GetCommonArray(Step step)
        {
            return ConvertStepToBytesArray(step);
        }

        public static byte[] GetCommonArrayForEndPack(Step step)
        {
            return ConvertStepToBytesArray(step, true);
        }

        private static byte[] ConvertStepToBytesArray(Step step, bool adjustStartTime = false)
        {
            long txid = step.TraceId;
            int elapsed = Convert.ToInt32(step.Duration.TotalMilliseconds);
            DateTime startTime = adjustStartTime
                ? step.StartTime.UtcDateTime.AddMilliseconds(step.Duration.TotalMilliseconds)
                : step.StartTime.UtcDateTime;

            long cpu = 0;
            long mem = 0;
            int pid = 1234;
            long threadId = 7;

            return CreateByteArray(txid, startTime, elapsed, cpu, mem, pid, threadId);
        }

        private static byte[] CreateByteArray(long txid, DateTime startTime, int elapsed, long cpu, long mem, int pid, long threadId)
        {
            var txidArray = PackUtils.IntTo8BytesArray(txid, EndianType.Big);
            var timeArray = PackUtils.IntTo8BytesArray(TimeUtils.GetUnixTimestampMillisFrom(startTime), EndianType.Big);
            var elapsedArray = PackUtils.IntTo4BytesArray(elapsed, EndianType.Big);
            var cpuArray = PackUtils.IntTo8BytesArray(cpu, EndianType.Big);
            var memArray = PackUtils.IntTo8BytesArray(mem, EndianType.Big);
            var pidArray = PackUtils.IntTo4BytesArray(pid, EndianType.Big);
            var threadIdArray = PackUtils.IntTo8BytesArray(threadId, EndianType.Big);

            int totalSize = txidArray.Length + timeArray.Length + elapsedArray.Length
                + cpuArray.Length + memArray.Length + pidArray.Length + threadIdArray.Length;

            using (MemoryStream ms = new MemoryStream(totalSize))
            {
                ms.Write(txidArray, 0, txidArray.Length);
                ms.Write(timeArray, 0, timeArray.Length);
                ms.Write(elapsedArray, 0, elapsedArray.Length);
                ms.Write(cpuArray, 0, cpuArray.Length);
                ms.Write(memArray, 0, memArray.Length);
                ms.Write(pidArray, 0, pidArray.Length);
                ms.Write(threadIdArray, 0, threadIdArray.Length);
                return ms.ToArray();
            }
        }
    }
}
