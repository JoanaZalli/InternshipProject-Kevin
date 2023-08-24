using Application.Interfaces.Pagination;
using Application.UseCases.CompanyTypeCases.Results;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapping {
    public class CompanyTypeMappings : Profile {
        public CompanyTypeMappings() {

            CreateMap<CompanyTypeResult, CompanyType>().ReverseMap();

            CreateMap<PagedList<CompanyType>, PagedList<CompanyTypeResult>>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items))
                .ReverseMap();
        }
    }
}
