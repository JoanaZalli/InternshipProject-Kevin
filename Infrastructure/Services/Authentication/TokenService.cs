using Application.Interfaces.Authentication;
using Application.Persistance;
using System.Security.Cryptography;

namespace Application.Services {
    public class TokenService : ITokenService {

        private readonly IUserVerificationAndResetRepository _userVerificationAndResetRepository;

        public TokenService(IUserVerificationAndResetRepository userVerificationAndResetRepository) {
            _userVerificationAndResetRepository = userVerificationAndResetRepository;
        }

        public async Task<string> GenerateVerificationTokenAsync() {
            string token;

            do {
                token = Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
            }
            while (await _userVerificationAndResetRepository.ContainsVerificationTokenAsync(token)); // check if such token already exists
            return token;
        }

        public async Task<string> GeneratePasswordTokenAsync() {
            string token;

            do {
                token = Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
            }
            while (await _userVerificationAndResetRepository.ContainsPasswordTokenAsync(token)); // check if such token already exists
            return token;
        }

        public async Task<string> GenerateRefreshTokenAsync() {
            string token;

            do {
                token = Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
            }
            while (await _userVerificationAndResetRepository.ContainsRefreshTokenAsync(token)); // check if such token already exists
            return token;
        }
    }
}
