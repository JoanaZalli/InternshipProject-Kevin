namespace Application.UseCases.Common {
    public class FinHubConnectionSettings {
        public const string SectionName = "FinHubConnection";
        public string AuthToken { get; set; } = null!;
    }
}
