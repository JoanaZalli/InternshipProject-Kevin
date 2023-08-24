using Application.Interfaces.Authentication;
using System.Security.Cryptography;

namespace Application.Services {
    public class HasherService : IHasherService {

        private const int SaltSize = 128 / 8;
        private const int KeySize = 256 / 8;
        private const int Iterations = 10000;
        private static readonly HashAlgorithmName _hashAlgorithmName = HashAlgorithmName.SHA256;

        public Tuple<string, string> HashPassword(string password) {
            var salt = RandomNumberGenerator.GetBytes(SaltSize);
            var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, _hashAlgorithmName, KeySize);

            return new Tuple<string, string>(Convert.ToBase64String(hash), Convert.ToBase64String(salt));
        }

        public bool VerifyPassword(string inputPassword, string passwordHash, string passwordSalt) {
            var hash = Convert.FromBase64String(passwordHash);
            var salt = Convert.FromBase64String(passwordSalt);

            var inputHash = Rfc2898DeriveBytes.Pbkdf2(inputPassword, salt, Iterations, _hashAlgorithmName, KeySize);
            return CryptographicOperations.FixedTimeEquals(hash, inputHash);
        }
    }
}
