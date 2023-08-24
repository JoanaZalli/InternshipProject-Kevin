using System.Net;

namespace Application.Exceptions.ClientErrors {
    public class BlockedException : Exception {

        public static readonly int status = (int)HttpStatusCode.TooManyRequests;

        public BlockedException(string message) : base(message) {
        }
    }
}
