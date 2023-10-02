using System;

namespace DotNetAtom.Utils
{
    /// <summary>
    /// Summary description for HexEncoding.
    /// </summary>
    public static class HexEncoding
    {
        public static int GetByteCount(string hexString)
        {
            var numHexChars = 0;
            char c;
            // remove all none A-F, 0-9, characters
            for (var i = 0; i < hexString.Length; i++)
            {
                c = hexString[i];
                if (IsHexDigit(c))
                    numHexChars++;
            }

            // if odd number of characters, discard last character
            if (numHexChars % 2 != 0)
            {
                numHexChars--;
            }

            return numHexChars / 2; // 2 characters per byte
        }

        /// <summary>
        /// Creates a byte array from the hexadecimal string. Each two characters are combined
        /// to create one byte. First two hexadecimal characters become first byte in returned array.
        /// Non-hexadecimal characters are ignored.
        /// </summary>
        /// <param name="hexString">string to convert to byte array</param>
        /// <param name="discarded">number of characters in string ignored</param>
        /// <returns>byte array, in the same left-to-right order as the hexString</returns>
        public static byte[] GetBytes(string hexString, out int discarded)
        {
            discarded = 0;
            var newString = "";
            char c;
            // remove all none A-F, 0-9, characters
            for (var i = 0; i < hexString.Length; i++)
            {
                c = hexString[i];
                if (IsHexDigit(c))
                    newString += c;
                else
                    discarded++;
            }

            // if odd number of characters, discard last character
            if (newString.Length % 2 != 0)
            {
                discarded++;
                newString = newString.Substring(0, newString.Length - 1);
            }

            var byteLength = newString.Length / 2;
            var bytes = new byte[byteLength];
            var j = 0;

            Span<char> span = stackalloc char[byteLength];

            for (var i = 0; i < bytes.Length; i++)
            {
                span[0] = newString[j];
                span[1] = newString[j + 1];
                bytes[i] = HexToByte(span);
                j = j + 2;
            }

            return bytes;
        }

        public static string ToString(byte[] bytes)
        {
            var hexString = "";
            for (var i = 0; i < bytes.Length; i++)
            {
                hexString += bytes[i].ToString("X2");
            }

            return hexString;
        }

        /// <summary>
        /// Determines if given string is in proper hexadecimal string format
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        public static bool InHexFormat(string hexString)
        {
            var hexFormat = true;

            foreach (var digit in hexString)
            {
                if (!IsHexDigit(digit))
                {
                    hexFormat = false;
                    break;
                }
            }

            return hexFormat;
        }

        /// <summary>
        /// Returns true is c is a hexadecimal digit (A-F, a-f, 0-9)
        /// </summary>
        /// <param name="c">Character to test</param>
        /// <returns>true if hex digit, false if not</returns>
        public static bool IsHexDigit(Char c)
        {
            int numChar;
            var numA = Convert.ToInt32('A');
            var num1 = Convert.ToInt32('0');
            c = Char.ToUpper(c);
            numChar = Convert.ToInt32(c);
            if (numChar >= numA && numChar < (numA + 6))
                return true;
            if (numChar >= num1 && numChar < (num1 + 10))
                return true;
            return false;
        }

        /// <summary>
        /// Converts 1 or 2 character string into equivalent byte value
        /// </summary>
        /// <param name="hex">1 or 2 character string</param>
        /// <returns>byte</returns>
        private static byte HexToByte(ReadOnlySpan<char> hex)
        {
            if (hex.Length is > 2 or <= 0)
            {
                throw new ArgumentException("hex must be 1 or 2 characters in length");
            }

#if NET
            return byte.Parse(hex, System.Globalization.NumberStyles.HexNumber);
#else
            return byte.Parse(hex.ToString(), System.Globalization.NumberStyles.HexNumber);
#endif
        }
    }
}