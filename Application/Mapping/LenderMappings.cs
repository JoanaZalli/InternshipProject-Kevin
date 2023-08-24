using Application.Interfaces.Pagination;
using Application.UseCases.LenderCases.Results;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapping {
    public class LenderMappings : Profile {
        public LenderMappings() {
            CreateMap<PagedList<Lender>, PagedList<LenderQueryResult>>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items))
                .ReverseMap();

            CreateMap<Lender, LenderQueryResult>();
        }
    }
}
