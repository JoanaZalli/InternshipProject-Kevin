namespace Domain.Seeds {
    public static class DefinedProducts {

        public static class FixedRatePreAmortization {
            public static readonly Guid Id = Guid.Parse("5FF6A3BE-482E-4826-B027-B7AEA05DE030");
            public const string Name = "Installments with pre-amortization at a fixed rate";
            public const string Description = "Description";
            public const decimal ReferenceRate = 0.0025M;
            public const decimal FinanceMinAmount = 10000.00M;
            public const decimal FinanceMaxAmount = 2000000.00M;
        }

        public static class VariableRatePreAmortization {
            public static readonly Guid Id = Guid.Parse("B2C0E6AE-2A83-4FD3-ACCE-DD1C647B1B1C");
            public const string Name = "Installment with variable rate pre-amortization";
            public const string Description = "Description";
            public const decimal ReferenceRate = 0.0025M;
            public const decimal FinanceMinAmount = 10000.00M;
            public const decimal FinanceMaxAmount = 2000000.00M;
        }
    }
}
