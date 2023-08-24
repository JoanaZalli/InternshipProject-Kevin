using Application.Interfaces.Pagination;
using Application.Persistance;
using Application.UseCases.ViewRoles.Results;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Application.UseCases.Roles.Queries {

    public class UserRoleQuery : IRequest<PagedList<RoleResult>> {
        public Guid UserId { get; set; }
        public string? Filter { get; set; }
        public string? SortColumn { get; set; }
        public string? SortOrder { get; set; }
        public int PageSize { get; set; }
        public int Page { get; set; }
    }

    public class UserRoleQueryHandler : IRequestHandler<UserRoleQuery, PagedList<RoleResult>> {

        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IPaginationService<Role> _pagination;
        private readonly ILogger<UserRoleQueryHandler> _logger;

        public UserRoleQueryHandler(IMapper mapper,
                                    IUserRepository userRepository,
                                    IPaginationService<Role> pagination,
                                    ILogger<UserRoleQueryHandler> logger) {
            _mapper = mapper;
            _userRepository = userRepository;
            _pagination = pagination;
            _logger = logger;
        }

        public async Task<PagedList<RoleResult>> Handle(UserRoleQuery request, CancellationToken cancellationToken) {

            try {
                var roles = _userRepository.GetRoles(request.UserId);

                var tuple = _pagination.Validate(request.Page, request.PageSize, roles.Count());
                request.Page = tuple.Item1;
                request.PageSize = tuple.Item2;

                roles = Filter(roles, request.Filter);
                roles = Sort(roles, request.SortOrder, request.SortColumn);
                var response = await _pagination.PaginateAsync(roles, request.Page, request.PageSize);

                return _mapper.Map<PagedList<RoleResult>>(response);
            }
            catch (Exception ex) {
                _logger.LogError("Error in User Role Query Handler", request);

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

    public class UserRoleQueryValidator : AbstractValidator<UserRoleQuery> {
        public UserRoleQueryValidator() {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("EmptyId");
        }
    }
}
