using System;

namespace WhaTap.Trace.GoAgent
{
    public sealed class GoPackDBSql
    {
        public static bool TryGetArray(PacketType packetType, Step step, byte[] packetBuffer, out int filledSize)
        {
            filledSize = 0;

            string dbConn = step.DbConn;
            string sqlQuery = step.ResourceName;
            string errorType = step.ErrorType;
            string errorMessage = step.ErrorMsg;
            string stack = step.ErrorStack;

            var intPackArray = GoPackBase.GetCommonArray(step);
            var dbConnArray = PackUtils.GetStringPack(dbConn);
            var sqlQueryArray = PackUtils.GetStringPack(sqlQuery);
            var errorTypeArray = PackUtils.GetStringPack(errorType);
            var errorMessageArray = PackUtils.GetStringPack(errorMessage);
            var stackArray = PackUtils.GetStringPack(stack);

            int totalSize = PackHeader.UDP_PACKET_HEADER_SIZE + intPackArray.Length + dbConnArray.Length
                            + sqlQueryArray.Length + errorTypeArray.Length + errorMessageArray.Length + stackArray.Length;

            if (totalSize > packetBuffer.Length) return false;

            int offset = 0;

            Array.Copy(PackHeader.GetBytes(packetType, totalSize - PackHeader.UDP_PACKET_HEADER_SIZE), 0, packetBuffer, offset, PackHeader.UDP_PACKET_HEADER_SIZE);
            offset += PackHeader.UDP_PACKET_HEADER_SIZE;

            Array.Copy(intPackArray, 0, packetBuffer, offset, intPackArray.Length);
            offset += intPackArray.Length;

            Array.Copy(dbConnArray, 0, packetBuffer, offset, dbConnArray.Length);
            offset += dbConnArray.Length;

            Array.Copy(sqlQueryArray, 0, packetBuffer, offset, sqlQueryArray.Length);
            offset += sqlQueryArray.Length;

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
