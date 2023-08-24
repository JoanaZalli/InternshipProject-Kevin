namespace Domain.Seeds {
    public static class DefinedRoles {
        public static class SuperAdmin {
            public static readonly Guid Id = Guid.Parse("1AFAC8E2-D840-40AA-A97F-C3F2BC5931B0");
            public const string Name = "SuperAdmin";
        }

        public static class LoanOfficer {
            public static readonly Guid Id = Guid.Parse("D6013A21-70D7-4C08-9DE9-482F339147A8");
            public const string Name = "LoanOfficer";
        }

        public static class Borrower {
            public static readonly Guid Id = Guid.Parse("B05D025A-62EE-4D6C-AEF4-9433CC52DCD0");
            public const string Name = "Borrower";
        }

        public static class RegisteredUser {
            public static readonly Guid Id = Guid.Parse("846D0436-FFCE-49A2-A8FF-BF22AEDF0A83");
            public const string Name = "RegisteredUser";
        }
    }
}
