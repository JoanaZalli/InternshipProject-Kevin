namespace Application.Interfaces.Email {
    public interface IMailBodyService {
        Task<string> GetVerificationMailBodyAsync(string email, string token);
        Task<string> GetForgotUsernameMailBodyAsync(string email);
        Task<string> GetForgotPasswordMailBodyAsync(string email, string token);
        Task<string> GetSuccessfulPasswordChangeMailBodyAsync();
    }
}
