using System;
using System.Buffers;
using System.Security.Cryptography;
using System.Text;
using DotNetAtom.Crypto;
using DotNetAtom.Options;
using Microsoft.Extensions.Options;

namespace DotNetAtom.Providers;

internal class EncryptedAspNetPasswordHasher(IOptions<MachineOptions> options) : IPasswordHasher
{
	private readonly SymmetricCryptography? _cryptography = GetSymmetricCryptography(options.Value.DecryptionKey);

	private static SymmetricCryptography? GetSymmetricCryptography(string? keyString)
	{
		if (keyString is null || keyString.Length == 0)
		{
			return null;
		}

		var key = HexEncoding.GetBytes(keyString, out _);
		var iv = new byte[8];

		return new SymmetricCryptography(TripleDES.Create(), key, iv);
	}

	/// <inheritdoc />
	public int Format => 2;

	/// <inheritdoc />
	public bool Validate(int format, string hashedPassword, string password, string passwordSalt)
	{
		return string.Equals(HashPassword(format, password, passwordSalt), hashedPassword);
	}

	/// <inheritdoc />
	public string HashPassword(int format, string password, string passwordSalt)
	{
		if (_cryptography is null)
		{
			throw new InvalidOperationException("DecryptionKey is not configured.");
		}

		var bSalt = Convert.FromBase64String(passwordSalt);
		var bAll = ArrayPool<byte>.Shared.Rent(bSalt.Length + Encoding.Unicode.GetMaxByteCount(password.Length));

		bSalt.AsSpan().CopyTo(bAll);
		var bInLength = Encoding.Unicode.GetBytes(password, 0, password.Length, bAll, bSalt.Length);
		var bRet = _cryptography.Encrypt(bAll, 0, bSalt.Length + bInLength);

		return Convert.ToBase64String(bRet);
	}
}
