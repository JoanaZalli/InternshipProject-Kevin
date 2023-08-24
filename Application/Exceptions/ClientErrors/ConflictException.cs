using System.Net;

namespace Application.Exceptions.ClientErrors {
    public class ConflictException : Exception {

        public static readonly int status = (int)HttpStatusCode.Conflict;

        public ConflictException(string message) : base(message) {
        }
    }
}
