using Application.Interfaces.Pagination;
using Application.UseCases.Roles.Commands;
using Application.UseCases.ViewRoles.Results;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapping {
    public class RoleMappings : Profile {
        public RoleMappings() {
            CreateMap<RoleResult, Role>().ReverseMap();

            CreateMap<CreateRoleCommand, Role>()
                           .ForMember(dest => dest.Id, opt => opt.Ignore())
                           .ReverseMap();

            CreateMap<PagedList<Role>, PagedList<RoleResult>>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items))
                .ReverseMap();
        }
    }
}
