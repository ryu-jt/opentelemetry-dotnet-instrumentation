using System;
using System.Text;
using WhaTap.Trace.Utils;

enum EndianType
{
    Big = 0,
    Little = 1,
}

namespace WhaTap.Trace.GoAgent
{
    static class PackUtils
    {
        const int STRING_MAX_SIZE = 32768;

        public static byte[] GetStringPack(string data)
        {
            try
            {
                if (data == null) data = string.Empty;
                else if (data.Length > STRING_MAX_SIZE) data = data.Substring(0, STRING_MAX_SIZE);

                const int HeaderSize = 2;

                byte[] data_array = Encoding.UTF8.GetBytes(data);
                byte[] result = new byte[data_array.Length + HeaderSize];

                IntTo2BytesArray(data_array.Length, EndianType.Big).CopyTo(result, 0);
                data_array.CopyTo(result, HeaderSize);

                return result;
            }
            catch (Exception ex)
            {
                Logger.Instance.Error("PackUtils.GetStringPack {ex}");
                return new byte[] { 0, 0 };
            }
        }

        public static byte[] IntTo2BytesArray(int data, EndianType type)
        {
            var result = new Byte[2];

            if (type == EndianType.Big)
            {
                result[1] = (byte)data;
                result[0] = (byte)(data >> 8);
            }
            else
            {
                result[0] = (byte)data;
                result[1] = (byte)(data >> 8);
            }

            return result;
        }

        public static byte[] IntTo4BytesArray(int data, EndianType type)
        {
            var result = new Byte[4];

            if (type == EndianType.Big)
            {
                result[3] = (byte)data;
                result[2] = (byte)(data >> 8);
                result[1] = (byte)(data >> 16);
                result[0] = (byte)(data >> 24);
            }
            else
            {
                result[0] = (byte)data;
                result[1] = (byte)(data >> 8);
                result[2] = (byte)(data >> 16);
                result[3] = (byte)(data >> 24);
            }

            return result;
        }

        public static byte[] IntTo8BytesArray(Int64 data, EndianType type)
        {
            var result = new Byte[8];

            if (type == EndianType.Big)
            {
                result[7] = (byte)data;
                result[6] = (byte)(data >> 8);
                result[5] = (byte)(data >> 16);
                result[4] = (byte)(data >> 24);
                result[3] = (byte)(data >> 32);
                result[2] = (byte)(data >> 40);
                result[1] = (byte)(data >> 48);
                result[0] = (byte)(data >> 56);
            }
            else
            {
                result[0] = (byte)data;
                result[1] = (byte)(data >> 8);
                result[2] = (byte)(data >> 16);
                result[3] = (byte)(data >> 24);
                result[4] = (byte)(data >> 32);
                result[5] = (byte)(data >> 40);
                result[6] = (byte)(data >> 48);
                result[7] = (byte)(data >> 56);
            }

            return result;
        }
    }
}
