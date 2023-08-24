using System.Net;

namespace Application.Exceptions.ClientErrors {
    public class UnauthorizedException : Exception {

        public static readonly int status = (int)HttpStatusCode.Unauthorized;

        public UnauthorizedException(string message) : base(message) {
        }
    }
}
