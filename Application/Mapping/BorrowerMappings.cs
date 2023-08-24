using Application.Interfaces.Pagination;
using Application.UseCases.BorrowerJourney.Commands;
using Application.UseCases.BorrowerJourney.Results;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapping {
    public class BorrowerMappings : Profile {
        public BorrowerMappings() {
            CreateMap<CreateBorrowerCommmand, Borrower>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.CompanyTypeId, opt => opt.Ignore())
                .ForMember(dest => dest.CompanyType, opt => opt.Ignore())
                .ForMember(dest => dest.CompanyProfile, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<BorrowerCommandResult, Borrower>().ReverseMap();

            CreateMap<BorrowerQueryResult, Borrower>().ReverseMap();

            CreateMap<UpdateBorrowerCommand, Borrower>().ReverseMap()
                .ForMember(dest => dest.CompanyTypeId, opt => opt.Ignore());

            CreateMap<PagedList<Borrower>, PagedList<BorrowerQueryResult>>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items))
                .ReverseMap();
        }
    }
}
