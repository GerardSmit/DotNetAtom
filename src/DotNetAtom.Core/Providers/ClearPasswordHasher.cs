namespace DotNetAtom.Providers;

public class ClearPasswordHasher : IPasswordHasher
{
	public int Format => 0;

	public bool Validate(int format, string hashedPassword, string password, string passwordSalt)
	{
		return string.Equals(HashPassword(format, password, passwordSalt), hashedPassword);
	}

	public string HashPassword(int format, string password, string passwordSalt)
	{
		return password;
	}
}
