﻿
namespace InternshipProject.Objects.Requests.BorrowerJourneyRequests {
    public class BorrowerRequest {
        public string CompanyName { get; set; } = null!;
        public string CompanyType { get; set; } = null!;
        public string VATNumber { get; set; } = null!;
        public string FiscalCode { get; set; } = null!;
    }
}
