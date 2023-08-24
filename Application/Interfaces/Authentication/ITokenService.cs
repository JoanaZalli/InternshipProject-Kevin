namespace Application.Interfaces.Authentication {
    public interface ITokenService {
        Task<string> GenerateVerificationTokenAsync();
        Task<string> GeneratePasswordTokenAsync();
        Task<string> GenerateRefreshTokenAsync();
    }
}
