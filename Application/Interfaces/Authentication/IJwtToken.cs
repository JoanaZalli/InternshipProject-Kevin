namespace Application.Interfaces.Authentication {
    public interface IJwtToken {
        string GenerateToken(Guid UserId, string username, IEnumerable<string> roles);
        bool IsExpired(string accessToken);
        Guid GetUserId(string accessToken);
        IEnumerable<string> GetRoles(string accessToken);
    }
}
