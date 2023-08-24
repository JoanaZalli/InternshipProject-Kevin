using System.Net;

namespace Application.Exceptions.ServerErrors {
    public class DatabaseException : Exception {

        public static readonly int status = (int)HttpStatusCode.ServiceUnavailable;

        public DatabaseException(string message) : base(message) { }
    }
}
