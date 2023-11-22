using System;
using System.Buffers;
using System.Security.Cryptography;
using System.Text;

namespace DotNetAtom.Providers;

internal class HashedAspNetPasswordHasher : IPasswordHasher
{
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
		var bIn = ArrayPool<byte>.Shared.Rent(Encoding.Unicode.GetMaxByteCount(password.Length));
		var bInLength = Encoding.Unicode.GetBytes(password, 0, password.Length, bIn, 0);

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

			bRet = kha.ComputeHash(bIn, 0, bInLength);
		}
		else
		{
			var bAllLength = bSalt.Length + bInLength;
			var bAll = ArrayPool<byte>.Shared.Rent(bAllLength);

			bSalt.AsSpan().CopyTo(bAll.AsSpan());
			bIn.AsSpan(0, bInLength).CopyTo(bAll.AsSpan(bSalt.Length));

			bRet = hm.ComputeHash(bAll, 0, bAllLength);

			ArrayPool<byte>.Shared.Return(bAll, true);
		}

		ArrayPool<byte>.Shared.Return(bIn, true);

		return Convert.ToBase64String(bRet);
	}
}
