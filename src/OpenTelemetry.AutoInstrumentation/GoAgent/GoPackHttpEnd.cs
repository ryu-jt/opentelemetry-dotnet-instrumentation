using System;

namespace WhaTap.Trace.GoAgent
{
    public sealed class GoPackHttpEnd
    {
        public static bool TryGetArray(PacketType packetType, Step step, byte[] packetBuffer, out int filledSize)
        {
            filledSize = 0;

            string host = step.Host;
            string uri = step.Url;

            var intPackArray = GoPackBase.GetCommonArrayForEndPack(step);
            var hostArray = PackUtils.GetStringPack(host);
            var uriArray = PackUtils.GetStringPack(uri);

            long mtid = 0;
            int mdepth = 0;
            long mcaller = 0;
            var mtidArray = PackUtils.IntTo8BytesArray(mtid, EndianType.Big);
            var mdepthArray = PackUtils.IntTo4BytesArray(mdepth, EndianType.Big);
            var mcallerArray = PackUtils.IntTo8BytesArray(mcaller, EndianType.Big);

            int totalSize = PackHeader.UDP_PACKET_HEADER_SIZE + intPackArray.Length + hostArray.Length + uriArray.Length
                            + mtidArray.Length + mdepthArray.Length + mcallerArray.Length;

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

            Array.Copy(mtidArray, 0, packetBuffer, offset, mtidArray.Length);
            offset += mtidArray.Length;

            Array.Copy(mdepthArray, 0, packetBuffer, offset, mdepthArray.Length);
            offset += mdepthArray.Length;

            Array.Copy(mcallerArray, 0, packetBuffer, offset, mcallerArray.Length);
            offset += mcallerArray.Length;

            filledSize = offset;

            return true;
        }
    }
}
