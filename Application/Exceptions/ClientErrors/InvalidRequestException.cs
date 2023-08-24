using System.Net;

namespace Application.Exceptions.ClientErrors {

    public class InvalidRequestException : Exception {

        public static readonly int status = (int)HttpStatusCode.BadRequest;

        public InvalidRequestException(string message) : base(message) { }
    }
}
