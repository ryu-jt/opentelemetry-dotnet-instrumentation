using System;

namespace WhaTap.Trace.GoAgent
{
    public sealed class GoPackHttpCall
    {
        public static bool TryGetArray(PacketType packetType, Step step, byte[] packetBuffer, out int filledSize)
        {
            filledSize = 0;

            string httpcUrl = step.Url;
            string mcallee = string.Empty;
            string errorType = step.ErrorType;
            string errorMessage = step.ErrorMsg;
            string stack = step.ErrorStack;

            var intPackArray = GoPackBase.GetCommonArray(step);
            var httpcUrlArray = PackUtils.GetStringPack(httpcUrl);
            var mcalleeArray = PackUtils.GetStringPack(mcallee);
            var errorTypeArray = PackUtils.GetStringPack(errorType);
            var errorMessageArray = PackUtils.GetStringPack(errorMessage);
            var stackArray = PackUtils.GetStringPack(stack);

            int totalSize = PackHeader.UDP_PACKET_HEADER_SIZE + intPackArray.Length + httpcUrlArray.Length + mcalleeArray.Length
                            + errorTypeArray.Length + errorMessageArray.Length + stackArray.Length;

            if (totalSize > packetBuffer.Length) return false;

            int offset = 0;

            Array.Copy(PackHeader.GetBytes(packetType, totalSize - PackHeader.UDP_PACKET_HEADER_SIZE), 0, packetBuffer, offset, PackHeader.UDP_PACKET_HEADER_SIZE);
            offset += PackHeader.UDP_PACKET_HEADER_SIZE;

            Array.Copy(intPackArray, 0, packetBuffer, offset, intPackArray.Length);
            offset += intPackArray.Length;

            Array.Copy(httpcUrlArray, 0, packetBuffer, offset, httpcUrlArray.Length);
            offset += httpcUrlArray.Length;

            Array.Copy(mcalleeArray, 0, packetBuffer, offset, mcalleeArray.Length);
            offset += mcalleeArray.Length;

            Array.Copy(errorTypeArray, 0, packetBuffer, offset, errorTypeArray.Length);
            offset += errorTypeArray.Length;

            Array.Copy(errorMessageArray, 0, packetBuffer, offset, errorMessageArray.Length);
            offset += errorMessageArray.Length;

            Array.Copy(stackArray, 0, packetBuffer, offset, stackArray.Length);
            offset += stackArray.Length;

            filledSize = offset;

            return true;
        }
    }
}
