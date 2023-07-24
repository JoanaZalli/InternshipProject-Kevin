﻿namespace Application.UseCases.Authentication.Results {
    public class LoginResult {
        public Guid Id { get; set; }
        public string Token { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
    }
}
