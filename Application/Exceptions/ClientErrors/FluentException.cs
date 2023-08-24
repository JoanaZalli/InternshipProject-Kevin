namespace Application.Exceptions.ClientErrors {
    public class FluentException : Exception {
        public FluentException(IReadOnlyDictionary<string, string[]> errorsDictionary)
            : base("Validation Failure") {
            ErrorsDictionary = errorsDictionary;
        }

        public IReadOnlyDictionary<string, string[]> ErrorsDictionary { get; }
    }
}
