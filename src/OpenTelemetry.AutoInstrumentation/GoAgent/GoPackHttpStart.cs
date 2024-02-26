using System;

namespace WhaTap.Trace.GoAgent
{
    public sealed class GoPackHttpStart
    {
        public static bool TryGetArray(PacketType packetType, Step step, byte[] packetBuffer, out int filledSize)
        {
            filledSize = 0;

            string host = step.Host;
            string uri = step.Url;
            string uAgent = step.HttpRequestUagent;
            string referrer = step.HttpRequestRef;

            string ipAddr = step.HttpRequestIpAddr != string.Empty ? step.HttpRequestIpAddr : step.OutHost;
            if (ipAddr == "::1") ipAddr = "127.0.0.1";

            string userId = step.UserId;
            string isStaticContents = string.Empty;

            var intPackArray = GoPackBase.GetCommonArray(step);
            var hostArray = PackUtils.GetStringPack(host);
            var uriArray = PackUtils.GetStringPack(uri);
            var ipAddrArray = PackUtils.GetStringPack(ipAddr);
            var uAgentArray = PackUtils.GetStringPack(uAgent);
            var refArray = PackUtils.GetStringPack(referrer);
            var userIdArray = PackUtils.GetStringPack(userId);
            var isStaticContentsArray = PackUtils.GetStringPack(isStaticContents);

            int totalSize = PackHeader.UDP_PACKET_HEADER_SIZE + intPackArray.Length + hostArray.Length + uriArray.Length
                            + ipAddrArray.Length + uAgentArray.Length + refArray.Length
                            + userIdArray.Length + isStaticContentsArray.Length;

            if (totalSize > packetBuffer.Length) return false;

            int offset = 0;

            Array.Copy(PackHeader.GetBytes(packetType, totalSize - PackHeader.UDP_PACKET_HEADER_SIZE), 0, packetBuffer, offset, PackHeader.UDP_PACKET_HEADER_SIZE);
            offset += PackHeader.UDP_PACKET_HEADER_SIZE;

            Array.Copy(intPackArray, 0, packetBuffer, offset, intPackArray.Length);
            offset += intPackArray.Length;

            Array.Copy(hostArray, 0, packetBuffer, offset, hostArray.Length);
            offset += hostArray.Length;

            Array.Copy(uriArray, 0, packetBuffer, offset, uriArray.Length);
            offset += uriArray.Length;

            Array.Copy(ipAddrArray, 0, packetBuffer, offset, ipAddrArray.Length);
            offset += ipAddrArray.Length;

            Array.Copy(uAgentArray, 0, packetBuffer, offset, uAgentArray.Length);
            offset += uAgentArray.Length;

            Array.Copy(refArray, 0, packetBuffer, offset, refArray.Length);
            offset += refArray.Length;

            Array.Copy(userIdArray, 0, packetBuffer, offset, userIdArray.Length);
            offset += userIdArray.Length;

            Array.Copy(isStaticContentsArray, 0, packetBuffer, offset, isStaticContentsArray.Length);
            offset += isStaticContentsArray.Length;

            filledSize = offset;

            return true;
        }
    }
}
