namespace WhaTap.Trace.GoAgent
{
    class PackHeader
    {
        public const int UDP_PACKET_HEADER_SIZE = 9;

        private const int UDP_PACKET_HEADER_TYPE_POS = 0;
        private const int UDP_PACKET_HEADER_VER_POS = 1;
        private const int UDP_PACKET_HEADER_LEN_POS = 5;
        private const int UDP_PACKET_HEADER_VERSION = 30101;

        public static byte[] GetBytes(PacketType packetType, int bodySize)
        {
            byte[] header = new byte[UDP_PACKET_HEADER_SIZE];

            header[UDP_PACKET_HEADER_TYPE_POS] = (byte)packetType;
            header[UDP_PACKET_HEADER_VER_POS] = (byte)(UDP_PACKET_HEADER_VERSION >> 8);
            header[UDP_PACKET_HEADER_VER_POS + 1] = (byte)(UDP_PACKET_HEADER_VERSION & 0xFF);

            header[UDP_PACKET_HEADER_LEN_POS] = (byte)(bodySize >> 24);
            header[UDP_PACKET_HEADER_LEN_POS + 1] = (byte)((bodySize >> 16) & 0xFF);
            header[UDP_PACKET_HEADER_LEN_POS + 2] = (byte)((bodySize >> 8) & 0xFF);
            header[UDP_PACKET_HEADER_LEN_POS + 3] = (byte)(bodySize & 0xFF);

            return header;
        }
    }
}