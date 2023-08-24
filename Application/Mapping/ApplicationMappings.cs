using Application.Interfaces.Pagination;
using Application.UseCases.ApplicationJourney.Commands;
using Application.UseCases.ApplicationJourney.Results;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapping {
    public class ApplicationMappings : Profile {
        public ApplicationMappings() {
            CreateMap<CreateApplicationCommand, ApplicationEntity>().ReverseMap();

            CreateMap<UpdateApplicationCommand, ApplicationEntity>().ReverseMap();

            CreateMap<ApplicationEntity, ApplicationQueryResult>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.ApplicationStatus.Name))
                .ReverseMap();

            CreateMap<ApplicationEntity, ApplicationCommandResult>()
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.FinancePurposeDefinition))
                .ReverseMap();

            CreateMap<PagedList<ApplicationEntity>, PagedList<ApplicationQueryResult>>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items))
                .ReverseMap();
        }
    }
}
