using System;
using System.Security.Cryptography;
using System.Text;
using DotNetAtom.Crypto;
using DotNetAtom.Options;
using Microsoft.Extensions.Options;

namespace DotNetAtom.Providers;

internal class HashedAspNetPasswordHasher(IOptions<MachineOptions> options) : IPasswordHasher
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
	public int Format => 1;

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

		using HashAlgorithm hm = SHA1.Create(); // TODO: Make this configurable

		if (hm is KeyedHashAlgorithm kha)
		{
			if (kha.Key.Length == bSalt.Length)
			{
				kha.Key = bSalt;
			}
			else if (kha.Key.Length < bSalt.Length)
			{
				var bKey = new byte[kha.Key.Length];
				Buffer.BlockCopy(bSalt, 0, bKey, 0, bKey.Length);
				kha.Key = bKey;
			}
			else
			{
				var bKey = new byte[kha.Key.Length];
				for (var iter = 0; iter < bKey.Length;)
				{
					var len = Math.Min(bSalt.Length, bKey.Length - iter);
					Buffer.BlockCopy(bSalt, 0, bKey, iter, len);
					iter += len;
				}

				kha.Key = bKey;
			}

			bRet = kha.ComputeHash(bIn);
		}
		else
		{
			var bAll = new byte[bSalt.Length + bIn.Length];
			Buffer.BlockCopy(bSalt, 0, bAll, 0, bSalt.Length);
			Buffer.BlockCopy(bIn, 0, bAll, bSalt.Length, bIn.Length);
			bRet = hm.ComputeHash(bAll);
		}

		return Convert.ToBase64String(bRet);
	}
}
