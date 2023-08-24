using System.Net;

namespace Application.Exceptions.ServerErrors {
    public class ServerException : Exception {

        public static readonly int status = (int)HttpStatusCode.InternalServerError;

        public ServerException() : base() { }
    }
}
