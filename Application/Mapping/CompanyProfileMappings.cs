using Application.UseCases.BorrowerJourney.Results;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapping {
    public class CompanyProfileMappings : Profile {
        public CompanyProfileMappings() {

            CreateMap<CompanyProfileResult, CompanyProfile>().ReverseMap();

        }
    }
}
