using System;

namespace WhaTap.Trace.GoAgent
{
    public static class GoPackError
    {
        public static bool TryGetArray(PacketType packetType, Step step, byte[] buffer, out int filledSize)
        {
            filledSize = 0;

            string errorType = step.ErrorType;
            string errorMessage = step.ErrorMsg;
            string stack = step.ErrorStack;

            var intPackArray = GoPackBase.GetCommonArray(step);
            var errorTypeArray = PackUtils.GetStringPack(errorType);
            var errorMessageArray = PackUtils.GetStringPack(errorMessage);
            var stackArray = PackUtils.GetStringPack(stack);

            int totalSize = PackHeader.UDP_PACKET_HEADER_SIZE + intPackArray.Length + errorTypeArray.Length + errorMessageArray.Length + stackArray.Length;

            if (buffer.Length < totalSize) return false;

            int offset = 0;

            Array.Copy(PackHeader.GetBytes(packetType, totalSize - PackHeader.UDP_PACKET_HEADER_SIZE), 0, buffer, offset, PackHeader.UDP_PACKET_HEADER_SIZE);
            offset += PackHeader.UDP_PACKET_HEADER_SIZE;

            Array.Copy(intPackArray, 0, buffer, offset, intPackArray.Length);
            offset += intPackArray.Length;
            Array.Copy(errorTypeArray, 0, buffer, offset, errorTypeArray.Length);
            offset += errorTypeArray.Length;
            Array.Copy(errorMessageArray, 0, buffer, offset, errorMessageArray.Length);
            offset += errorMessageArray.Length;
            Array.Copy(stackArray, 0, buffer, offset, stackArray.Length);

            filledSize = totalSize;

            return true;
        }
    }
}
