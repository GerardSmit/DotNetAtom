namespace DotNetAtom.Providers;

public interface IPasswordHasher
{
    int Format { get; }

    bool Validate(int format, string hashedPassword, string password, string passwordSalt);

    string HashPassword(int format, string password, string passwordSalt);
}
