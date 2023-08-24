namespace Domain.Seeds {
    public static class DefinedCompanyTypes {

        public static class SoleProprietorship {
            public static readonly Guid Id = Guid.Parse("7DB974F8-6321-4B35-8EF4-65EDDA9FE1D6");
            public const string Name = "Sole Proprietorship";
            public const string Description = "Description";
            public const int FiscalCodeLength = 16;
        }

        public static class Other {
            public static readonly Guid Id = Guid.Parse("A7EEA608-196C-4A52-A5C7-9694E0EB190B");
            public const string Name = "Other";
            public const string Description = "Description";
            public const int FiscalCodeLength = 11;

        }

        public static class PartnershipLimitedByShares {
            public static readonly Guid Id = Guid.Parse("67D5FD0E-F3A4-4AA7-BEF8-8A587BCB475E");
            public const string Name = "Partnership Limited by Shares";
            public const string Description = "Description";
        }

        public static class LimitedPartnership {
            public static readonly Guid Id = Guid.Parse("B2B5CE14-79B1-402E-92F5-2536BCE91DDA");
            public const string Name = "Limited Partnership";
            public const string Description = "Description";
        }

        public static class CooperativeSociety {
            public static readonly Guid Id = Guid.Parse("BE9A7220-0773-4D4D-8EF7-B5FBF480E952");
            public const string Name = "Cooperative Society";
            public const string Description = "Description";
        }

        public static class GeneralPartnership {
            public static readonly Guid Id = Guid.Parse("AFBB07DD-70AD-471D-8448-67539B17B872");
            public const string Name = "General Partnership";
            public const string Description = "Description";
        }
    }
}
