namespace Domain.Seeds {
    public static class DefinedPermissions {

        public static class IsRegistered {
            public static readonly Guid Id = Guid.Parse("6FC1CBAF-B307-4EFA-8CA9-2CCED12A6028");
            public const string Name = "IsRegistered";
        }
        public static class IsSuperAdmin {
            public static readonly Guid Id = Guid.Parse("FF9F62DE-AB2A-4BA2-A15D-11A1F214AC66");
            public const string Name = "IsSuperAdmin";
        }

        public static class CanReadBorrowers {
            public static readonly Guid Id = Guid.Parse("F2A1FBA5-A23E-4686-9C0F-3E636B1AC3E4");
            public const string Name = "CanReadBorrowers";
        }
        public static class CanReadOwnBorrowers {
            public static readonly Guid Id = Guid.Parse("2C9DAD73-3625-4ECB-9522-C0F59A003E17");
            public const string Name = "CanReadOwnBorrowers";
        }
        public static class CanAddBorrower {
            public static readonly Guid Id = Guid.Parse("D3E011D6-53D4-46C7-8866-840593334476");
            public const string Name = "CanAddBorrower";
        }
        public static class CanUpdateBorrower {
            public static readonly Guid Id = Guid.Parse("96C36A8C-BE53-442D-A2F6-B2BCD71B3524");
            public const string Name = "CanUpdateBorrower";
        }

        public static class CanReadApplications {
            public static readonly Guid Id = Guid.Parse("4D74CE4F-B8D9-4C87-8F48-365C00DC612C");
            public const string Name = "CanReadApplications";
        }
        public static class CanReadOwnApplications {
            public static readonly Guid Id = Guid.Parse("A333628C-0918-47DD-9C84-30342E0E95E3");
            public const string Name = "CanReadOwnApplications";
        }
        public static class CanAddApplication {
            public static readonly Guid Id = Guid.Parse("6FE53FC9-8FDE-45F2-B3EB-F988E7ABD00D");
            public const string Name = "CanAddApplication";
        }
        public static class CanUpdateApplication {
            public static readonly Guid Id = Guid.Parse("3B12B41C-CDD3-45C8-8466-31750B8D3E3C");
            public const string Name = "CanUpdateApplication";
        }

        public static class CanGenerateMatrix {
            public static readonly Guid Id = Guid.Parse("CD978177-AA39-45F5-B6A6-783B9795196C");
            public const string Name = "CanGenerateMatrix";
        }
        public static class CanCreateMatrix {
            public static readonly Guid Id = Guid.Parse("2FE0991B-7A0B-4700-8F2C-036782B973BC");
            public const string Name = "CanCreateMatrix";
        }
        public static class CanUpdateMatrix {
            public static readonly Guid Id = Guid.Parse("63F2AFD8-C671-4DEC-93AF-DE756E2E6E8A");
            public const string Name = "CanUpdateMatrix";
        }
        public static class CanDeleteMatrix {
            public static readonly Guid Id = Guid.Parse("37E38648-4F5D-48AD-9FAB-E8DFE5C6E42C");
            public const string Name = "CanDeleteMatrix";
        }

        public static class CanAddLoan {
            public static readonly Guid Id = Guid.Parse("4F29D160-B6C3-4BFF-9BAE-D1E6BE1DAC8B");
            public const string Name = "CanAddLoan";
        }
        public static class CanUpdateLoan {
            public static readonly Guid Id = Guid.Parse("00E181E4-0549-4EBC-8730-77C901BFE676");
            public const string Name = "CanUpdateLoan";
        }
        public static class CanReadLoans {
            public static readonly Guid Id = Guid.Parse("37704915-FF65-432F-8767-F5320C0DDEA5");
            public const string Name = "CanReadLoans";
        }
        public static class CanChangeLoanStatus {
            public static readonly Guid Id = Guid.Parse("5B3AFD8D-F998-455F-B251-0B38B752C663");
            public const string Name = "CanChangeLoanStatus";
        }
        public static class CanReadStatuses {
            public static readonly Guid Id = Guid.Parse("91C8BA0C-269B-4462-802C-CB6F3729FC9F");
            public const string Name = "CanReadStatuses";
        }

        public static class CanReadLenders {
            public static readonly Guid Id = Guid.Parse("3843C5CF-6AD5-4920-8BDD-838DE39315E2");
            public const string Name = "CanReadLenders";
        }

        public static class CanReadCompanyTypes {
            public static readonly Guid Id = Guid.Parse("11B88057-19B4-4FCC-AADA-3BDB20877FAF");
            public const string Name = "CanReadCompanyTypes";
        }

        public static class CanReadProducts {
            public static readonly Guid Id = Guid.Parse("52B5A66F-70F9-42E7-AA25-E48171D634A5");
            public const string Name = "CanReadProducts";
        }

        public static class GenerateEligibles {
            public static readonly Guid Id = Guid.Parse("7ED78266-C462-4C30-8860-DCD405880646");
            public const string Name = "GenerateEligibles";
        }

        public static class CanReadUsers {
            public static readonly Guid Id = Guid.Parse("943FDE30-8F60-4401-A63D-C4218C930882");
            public const string Name = "CanReadUsers";
        }
    }
}
