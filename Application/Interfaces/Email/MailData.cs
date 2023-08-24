namespace Application.Interfaces.Email {
    public class MailData {
        public string To { get; }
        public string Subject { get; }
        public string? Body { get; }

        public MailData(string to, string subject, string? body = null) {
            To = to;
            Subject = subject;
            Body = body;
        }
    }
}