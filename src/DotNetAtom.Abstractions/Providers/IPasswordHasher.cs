namespace DotNetAtom.Providers;

public interface IPasswordHasher
{
    bool Validate(int format, string hashedPassword, string passwordSalt, string password);

    string HashPassword(int format, string password, string passwordSalt);
}
