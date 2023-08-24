namespace Domain.Seeds {
    public static class DefinedUsers {
        public static class SA {
            public static readonly Guid Id = Guid.Parse("1B2031FF-DF77-4CE4-A2F0-00E60546F243");
            public const string FirstName = "Kevin";
            public const string LastName = "Shemili";
            public const string Username = "kevinshemili1";
            public const string Email = "kevin.shemili@cardoai.com";
            public const bool IsEmailConfirmed = true;
            public const string PhoneNumber = "683363203";
            public const string Prefix = "+355";
            public const string PasswordHash = "nsslp9QXF6wOvaGzfIHsoI+23nH+e8+l1SD8bv0IFrI=";
            public const string PasswordSalt = "jWRLoRafDBcFS72uPEqyqg==";
        }

        public static class Loan_Officer {
            public static readonly Guid Id = Guid.Parse("75AEEBBA-0D7D-4F8B-A95D-4D9551167C56");
            public const string FirstName = "KevinLoan";
            public const string LastName = "ShemiliLoan";
            public const string Username = "kevinOfficer1";
            public const string Email = "kevin.shemili@officer.com";
            public const bool IsEmailConfirmed = true;
            public const string PhoneNumber = "683363203";
            public const string Prefix = "+355";
            public const string PasswordHash = "nsslp9QXF6wOvaGzfIHsoI+23nH+e8+l1SD8bv0IFrI=";
            public const string PasswordSalt = "jWRLoRafDBcFS72uPEqyqg==";
        }
    }
}
