using System;

namespace WhaTap.Trace.Utils
{
    public static class StringUtils
    {
        public static string RemoveEmptyLines(string input)
        {
            string[] lines = input.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            string modifiedInput = string.Join("\n", lines).Trim();
            return modifiedInput;
        }

        public static bool? ToBoolean(string value)
        {
            if (value == null) return null;

            switch (value.ToUpperInvariant())
            {
                case "TRUE":
                case "YES":
                case "1":
                    return true;
                case "FALSE":
                case "NO":
                case "0":
                    return false;
                default:
                    return null;
            }
        }
    }
}
