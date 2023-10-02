using System;
using System.Security.Cryptography;
using System.Text;
using DotNetAtom.Options;
using DotNetAtom.Utils;
using Microsoft.Extensions.Options;

namespace DotNetAtom.Providers;

internal class TripleDESPasswordHasher(IOptions<MachineKeyOptions> options) : IPasswordHasher
{
    private readonly SymmetricCryptography _cryptography = GetSymmetricCryptography(options.Value.DecryptionKey);

    public bool Validate(int format, string hashedPassword, string passwordSalt, string password)
    {
        return string.Equals(HashPassword(format, password, passwordSalt), hashedPassword);
    }

    public string HashPassword(int format, string password, string passwordSalt)
    {
        var bIn = Encoding.Unicode.GetBytes(password);
        var bSalt = Convert.FromBase64String(passwordSalt);
        var bAll = new byte[bSalt.Length + bIn.Length];

        Buffer.BlockCopy(bSalt, 0, bAll, 0, bSalt.Length);
        Buffer.BlockCopy(bIn, 0, bAll, bSalt.Length, bIn.Length);

        return Convert.ToBase64String(_cryptography.Encrypt(bAll));
    }

    private static SymmetricCryptography GetSymmetricCryptography(string keyString)
    {
        var provider = TripleDES.Create();
        var key = HexEncoding.GetBytes(keyString, out _);
        var iv = new byte[8];

        return new SymmetricCryptography(provider, iv, key);
    }
}
