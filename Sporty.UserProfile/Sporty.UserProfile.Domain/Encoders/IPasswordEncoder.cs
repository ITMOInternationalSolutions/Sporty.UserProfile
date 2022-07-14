namespace Sporty.UserProfile.Domain.Encoders;

public interface IPasswordEncoder
{
    public string GetRandomSalt();
    public string Encode(string rawPassword);
    public bool Matches(string rawPassword, string hashedPassword);
}