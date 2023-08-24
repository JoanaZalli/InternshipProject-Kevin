namespace Application.Interfaces.Email {
    public interface IMailService {
        Task<bool> SendAsync(MailData mailData, CancellationToken ct);
    }
}
