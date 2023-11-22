using System;
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
		var bIn = Encoding.Unicode.GetBytes(password);
		var bSalt = Convert.FromBase64String(passwordSalt);
		byte[]? bRet;

		if (_cryptography is null)
		{
			throw new InvalidOperationException("DecryptionKey is not configured.");
		}

		byte[] bAll = new byte[bSalt.Length + bIn.Length];
		Buffer.BlockCopy(bSalt, 0, bAll, 0, bSalt.Length);
		Buffer.BlockCopy(bIn, 0, bAll, bSalt.Length, bIn.Length);
		bRet = _cryptography.Encrypt(bAll);

		return Convert.ToBase64String(bRet);
	}
}
