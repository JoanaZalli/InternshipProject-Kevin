using Application.Interfaces.Pagination;
using Application.Persistance;
using Application.UseCases.ViewPermissions.Results;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Application.UseCases.Permissions.Queries {

    public class RolePermissionsQuery : IRequest<PagedList<PermissionsResult>> {
        public Guid Id { get; set; }
        public string? Filter { get; set; }
        public string? SortColumn { get; set; }
        public string? SortOrder { get; set; }
        public int PageSize { get; set; }
        public int Page { get; set; }
    }

    public class RolePermissionsQueryHandler : IRequestHandler<RolePermissionsQuery, PagedList<PermissionsResult>> {

        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;
        private readonly IPaginationService<Permission> _pagination;
        private readonly ILogger<RolePermissionsQueryHandler> _logger;

        public RolePermissionsQueryHandler(IRoleRepository roleRepository,
                                           IMapper mapper,
                                           IPaginationService<Permission> pagination,
                                           ILogger<RolePermissionsQueryHandler> logger) {
            _roleRepository = roleRepository;
            _mapper = mapper;
            _pagination = pagination;
            _logger = logger;
        }

        public async Task<PagedList<PermissionsResult>> Handle(RolePermissionsQuery request, CancellationToken cancellationToken) {

            try {
                var permissions = _roleRepository.GetPermissions(request.Id);

                var tuple = _pagination.Validate(request.Page, request.PageSize, permissions.Count());
                request.Page = tuple.Item1;
                request.PageSize = tuple.Item2;

                permissions = Filter(permissions, request.Filter);
                permissions = Sort(permissions, request.SortOrder, request.SortColumn);
                var response = await _pagination.PaginateAsync(permissions, request.Page, request.PageSize);

                return _mapper.Map<PagedList<PermissionsResult>>(response);
            }
            catch (Exception ex) {
                _logger.LogError("Error in Role Permissions Query Handler", request);

                throw;
            }
        }

        private static IQueryable<Permission> Filter(IQueryable<Permission> permissions, string? filter) {
            if (string.IsNullOrWhiteSpace(filter) is false)
                permissions = permissions.Where(x =>
                    x.Name.Contains(filter));

            return permissions;
        }

        private static IQueryable<Permission> Sort(IQueryable<Permission> permissions, string? sortOrder, string? sortColumn) {

            Expression<Func<Permission, object>> key = sortColumn?.ToLower() switch {
                "name" => x => x.Name,
                _ => x => x.Id
            };

            if (sortOrder?.ToLower() == "desc")
                return permissions.OrderByDescending(key);
            else
                return permissions.OrderBy(key);
        }
    }

    public class RolePermissionsQueryValidator : AbstractValidator<RolePermissionsQuery> {
        public RolePermissionsQueryValidator() {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("EmptyId");
        }
    }
}
