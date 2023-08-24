using System.Net;

namespace Application.Exceptions.ClientErrors {
    public class ForbiddenException : Exception {

        public static readonly int status = (int)HttpStatusCode.Forbidden;

        public ForbiddenException(string message) : base(message) {
        }
    }
}
