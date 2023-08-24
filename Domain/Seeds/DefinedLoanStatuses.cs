namespace Domain.Seeds {
    public static class DefinedLoanStatuses {
        public static class Created {
            public static readonly Guid Id = Guid.Parse("45E0D7E5-0C30-4EE0-B0C6-23379A9BD138");
            public const string Name = "Created";
        }

        public static class Erased {
            public static readonly Guid Id = Guid.Parse("2DC1C106-6C3D-4AF4-98E3-3AF497A097F1");
            public const string Name = "Erased";
        }

        public static class Defaulted {
            public static readonly Guid Id = Guid.Parse("E8DB4469-C42E-4CDC-9848-8AE2B7ECCCCE");
            public const string Name = "Defaulted";
        }

        public static class Disbursed {
            public static readonly Guid Id = Guid.Parse("F28AF2B0-4535-49E7-BB11-00054166A910");
            public const string Name = "Disbursed";
        }

        public static class Guaranted {
            public static readonly Guid Id = Guid.Parse("60724BC1-B7CE-4C2A-9663-A0B5B1BD252C");
            public const string Name = "Guaranteed";
        }

        public static class Rejected {
            public static readonly Guid Id = Guid.Parse("95269826-EE81-4F28-9783-9BC0D9996300");
            public const string Name = "Rejected";
        }

        public static class Repaid {
            public static readonly Guid Id = Guid.Parse("5318A2E8-6A76-49E6-8B2F-97A8E09E5C8C");
            public const string Name = "Repaid";
        }
    }
}
