using Application.Interfaces.Pagination;
using Application.Persistance;
using Application.UseCases.ViewRoles.Results;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Application.UseCases.ViewPermissions.Queries {

    public class GetRoleQuery : IRequest<PagedList<RoleResult>> {
        public string? Filter { get; set; }
        public string? SortColumn { get; set; }
        public string? SortOrder { get; set; }
        public int PageSize { get; set; }
        public int Page { get; set; }
    }

    public class GetRoleQueryHandler : IRequestHandler<GetRoleQuery, PagedList<RoleResult>> {

        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;
        private readonly IPaginationService<Role> _pagination;
        private readonly ILogger<GetRoleQueryHandler> _logger;

        public GetRoleQueryHandler(IRoleRepository roleRepository,
                                   IMapper mapper,
                                   IPaginationService<Role> pagination,
                                   ILogger<GetRoleQueryHandler> logger) {
            _roleRepository = roleRepository;
            _mapper = mapper;
            _pagination = pagination;
            _logger = logger;
        }

        public async Task<PagedList<RoleResult>> Handle(GetRoleQuery request, CancellationToken cancellationToken) {

            try {
                var roles = _roleRepository.GetIQueryable();

                var tuple = _pagination.Validate(request.Page, request.PageSize, roles.Count());
                request.Page = tuple.Item1;
                request.PageSize = tuple.Item2;

                roles = Filter(roles, request.Filter);
                roles = Sort(roles, request.SortOrder, request.SortColumn);
                var response = await _pagination.PaginateAsync(roles, request.Page, request.PageSize);

                return _mapper.Map<PagedList<RoleResult>>(response);
            }
            catch (Exception ex) {
                _logger.LogError("Error in Get Role Query Handler", request);

                throw;
            }
        }

        private static IQueryable<Role> Filter(IQueryable<Role> roles, string? filter) {
            if (string.IsNullOrWhiteSpace(filter) is false)
                roles = roles.Where(x =>
                    x.Name.Contains(filter));

            return roles;
        }

        private static IQueryable<Role> Sort(IQueryable<Role> roles, string? sortOrder, string? sortColumn) {

            Expression<Func<Role, object>> key = sortColumn?.ToLower() switch {
                "name" => x => x.Name,
                _ => x => x.Id
            };

            if (sortOrder?.ToLower() == "desc")
                return roles.OrderByDescending(key);
            else
                return roles.OrderBy(key);
        }
    }

    public class GetRoleQueryValidator : AbstractValidator<GetRoleQuery> {
        public GetRoleQueryValidator() { }
    }
}
