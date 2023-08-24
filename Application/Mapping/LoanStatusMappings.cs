using Application.Interfaces.Pagination;
using Application.UseCases.StatusCases.Results;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapping {
    public class LoanStatusMappings : Profile {
        public LoanStatusMappings() {

            CreateMap<LoanStatusResult, LoanStatus>().ReverseMap();

            CreateMap<PagedList<LoanStatus>, PagedList<LoanStatusResult>>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items))
                .ReverseMap();
        }
    }
}
