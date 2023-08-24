using System.Net;

namespace Application.Exceptions.ClientErrors {
    public class NotFoundException : Exception {

        public static readonly int status = (int)HttpStatusCode.NotFound;

        public NotFoundException(string message) : base(message) {
        }
    }
}
