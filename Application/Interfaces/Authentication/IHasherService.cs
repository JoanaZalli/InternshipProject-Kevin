namespace Application.Interfaces.Authentication {
    public interface IHasherService {
        bool VerifyPassword(string inputPassword, string passwordHash, string passwordSalt);
        Tuple<string, string> HashPassword(string password);
    }
}
