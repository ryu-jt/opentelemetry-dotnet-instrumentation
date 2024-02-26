using System;

namespace WhaTap.Trace.GoAgent
{
    public sealed class GoPackMessage
    {
        public static bool TryGetArray(PacketType packetType, Step step, byte[] packetBuffer, out int filledSize)
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

            if (totalSize > packetBuffer.Length) return false;

            int offset = 0;
            Array.Copy(PackHeader.GetBytes(packetType, totalSize - PackHeader.UDP_PACKET_HEADER_SIZE), 0, packetBuffer, offset, PackHeader.UDP_PACKET_HEADER_SIZE);
            offset += PackHeader.UDP_PACKET_HEADER_SIZE;

            Array.Copy(intPackArray, 0, packetBuffer, offset, intPackArray.Length);
            offset += intPackArray.Length;

            Array.Copy(hashArray, 0, packetBuffer, offset, hashArray.Length);
            offset += hashArray.Length;

            Array.Copy(valueArray, 0, packetBuffer, offset, valueArray.Length);
            offset += valueArray.Length;

            Array.Copy(descArray, 0, packetBuffer, offset, descArray.Length);
            offset += descArray.Length;

            filledSize = offset;

            return true;
        }
    }
}
