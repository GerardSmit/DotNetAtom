using System;

public static class HexEncoding
{
	public static byte[] GetBytes(string hexString, out int discarded)
	{
		discarded = 0;
		var charSpan = hexString.AsSpan();

		// Use Span to avoid unnecessary allocations
		Span<byte> result = stackalloc byte[charSpan.Length / 2];
		Span<char> charBuffer = stackalloc char[2];

		var charIndex = 0;
		var resultIndex = 0;

		// Filter out non-hex characters
		foreach (var c in charSpan)
		{
			if (!IsHexDigit(c))
			{
				discarded++;
				continue;
			}

			charBuffer[charIndex] = c;
			charIndex++;

			if (charIndex == 2)
			{
				result[resultIndex] = HexToByte(charBuffer);
				resultIndex++;
				charIndex = 0;
			}
		}

		return result.Slice(0, resultIndex).ToArray();
	}

	public static bool IsHexDigit(char c)
	{
		return c is (>= '0' and <= '9')
			or (>= 'A' and <= 'F')
			or (>= 'a' and <= 'f');
	}

	private static byte HexToByte(ReadOnlySpan<char> hex)
	{
		if (hex.Length != 2)
		{
			throw new ArgumentException("Hex span must be exactly 2 characters.");
		}

		var high = HexCharToInt(hex[0]);
		var low = HexCharToInt(hex[1]);

		return (byte)((high << 4) + low);
	}

	private static int HexCharToInt(char c)
	{
		return c switch
		{
			>= '0' and <= '9' => c - '0',
			>= 'A' and <= 'F' => c - 'A' + 10,
			>= 'a' and <= 'f' => c - 'a' + 10,
			_ => throw new ArgumentException("Invalid hex character: " + c)
		};
	}
}
