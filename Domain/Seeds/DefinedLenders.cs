namespace Domain.Seeds {
    public static class DefinedLenders {

        public static class PmiBtech {
            public static readonly Guid Id = Guid.Parse("8D1AC5ED-0E1E-4DE1-A7FC-A7DF9E095653");
            public const string Name = "PMI BTECH";
            public const int RequestedAmount = 100000;
            public const int MinTenor = 30;
            public const int MaxTenor = 65;
        }

        public static class Azif {
            public static readonly Guid Id = Guid.Parse("7F83C404-EFEE-4900-98EE-38D3C95DAF56");
            public const string Name = "AZIF";
            public const int RequestedAmount = 400000;
            public const int MinTenor = 40;
            public const int MaxTenor = 60;
        }

        public static class Logitech {
            public static readonly Guid Id = Guid.Parse("0F3C377F-89AD-4FD6-AF55-62F783B0EA52");
            public const string Name = "LOGITECH";
            public const int RequestedAmount = 100000;
            public const int MinTenor = 30;
            public const int MaxTenor = 60;
        }

    }
}
