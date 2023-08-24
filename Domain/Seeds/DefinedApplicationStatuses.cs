namespace Domain.Seeds {
    public static class DefinedApplicationStatuses {
        public static class InCharge {
            public static readonly Guid Id = Guid.Parse("B1F0C15D-7C24-4277-991C-5A30A40ABACC");
            public const string Name = "In Charge";
        }

        public static class Canceled {
            public static readonly Guid Id = Guid.Parse("58749E4C-3BD5-4BF4-B86F-E9B0303A015E");
            public const string Name = "Loan Canceled";
        }

        public static class Defaulted {
            public static readonly Guid Id = Guid.Parse("287C9A29-9D88-48CB-9AA7-95B33B6FB197");
            public const string Name = "Loan Defaulted";
        }

        public static class Disbursed {
            public static readonly Guid Id = Guid.Parse("25275DA5-388E-46DB-A169-99E1C58D0A7B");
            public const string Name = "Loan Disbursed";
        }

        public static class Guaranted {
            public static readonly Guid Id = Guid.Parse("23EADAC2-3BBB-421B-9A5B-AFF07EB74C41");
            public const string Name = "Loan Guaranteed";
        }

        public static class Rejected {
            public static readonly Guid Id = Guid.Parse("2FBDE7F8-A9F1-4857-AC10-F818DB1DC8B0");
            public const string Name = "Loan Rejected";
        }

        public static class Repaid {
            public static readonly Guid Id = Guid.Parse("2C656D64-8BCD-4C96-BB96-CD04C231199D");
            public const string Name = "Loan Repaid";
        }
    }
}
