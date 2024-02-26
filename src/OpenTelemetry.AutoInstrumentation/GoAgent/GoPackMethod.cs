using System;

namespace WhaTap.Trace.GoAgent
{
    public sealed class GoPackMethod
    {
        public static bool TryGetArray(PacketType packetType, Step step, byte[] buffer, out int filledSize)
        {
            filledSize = 0;

            string hash = step.MessageHash;
            string value = step.MessageValue;
            string desc = step.MessageDesc;

            var intPackArray = GoPackBase.GetCommonArray(step);
            var hashArray = PackUtils.GetStringPack(hash);
            var valueArray = PackUtils.GetStringPack(value);
            var descArray = PackUtils.GetStringPack(desc);

            int totalSize = PackHeader.UDP_PACKET_HEADER_SIZE + intPackArray.Length + hashArray.Length + valueArray.Length + descArray.Length;

            if (buffer.Length < totalSize) return false;

            int offset = 0;

            byte[] headerBytes = PackHeader.GetBytes(packetType, totalSize - PackHeader.UDP_PACKET_HEADER_SIZE);
            Array.Copy(headerBytes, 0, buffer, offset, headerBytes.Length);
            offset += headerBytes.Length;

            Array.Copy(intPackArray, 0, buffer, offset, intPackArray.Length);
            offset += intPackArray.Length;
            Array.Copy(hashArray, 0, buffer, offset, hashArray.Length);
            offset += hashArray.Length;
            Array.Copy(valueArray, 0, buffer, offset, valueArray.Length);
            offset += valueArray.Length;
            Array.Copy(descArray, 0, buffer, offset, descArray.Length);
            offset += descArray.Length;

            filledSize = totalSize;

            return true;
        }
    }
}
