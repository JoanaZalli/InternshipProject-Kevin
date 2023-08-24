using Application.Interfaces.Pagination;
using Application.Persistance;
using Application.UseCases.UserCases.Results;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Application.UseCases.UserCases.Queries {

    public class GetAllUsersQuery : IRequest<PagedList<UserResult>> {
        public string? Filter { get; set; }
        public string? SortColumn { get; set; }
        public string? SortOrder { get; set; }
        public int PageSize { get; set; }
        public int Page { get; set; }
    }

    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, PagedList<UserResult>> {

        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IPaginationService<User> _pagination;
        private readonly ILogger<GetAllUsersQueryHandler> _logger;

        public GetAllUsersQueryHandler(IMapper mapper,
                                       IUserRepository userRepository,
                                       IPaginationService<User> pagination,
                                       ILogger<GetAllUsersQueryHandler> logger) {
            _mapper = mapper;
            _userRepository = userRepository;
            _pagination = pagination;
            _logger = logger;
        }

        public async Task<PagedList<UserResult>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken) {

            try {
                var users = _userRepository.GetIQueryable();

                var tuple = _pagination.Validate(request.Page, request.PageSize, users.Count());
                request.Page = tuple.Item1;
                request.PageSize = tuple.Item2;

                users = Filter(users, request.Filter);
                users = Sort(users, request.SortOrder, request.SortColumn);
                var response = await _pagination.PaginateAsync(users, request.Page, request.PageSize);

                return _mapper.Map<PagedList<UserResult>>(response);
            }
            catch (Exception ex) {
                _logger.LogError("Error in Get All Users Query Handler", request);

                throw;
            }
        }

        private static IQueryable<User> Filter(IQueryable<User> users, string? filter) {
            if (string.IsNullOrWhiteSpace(filter) is false)
                users = users.Where(x =>
                    x.FirstName.Contains(filter) ||
                    x.LastName.Contains(filter) ||
                    x.Username.Contains(filter) ||
                    x.Email.Contains(filter) ||
                    x.PhoneNumber.ToString().Contains(filter));

            return users;
        }

        private static IQueryable<User> Sort(IQueryable<User> users, string? sortOrder, string? sortColumn) {

            Expression<Func<User, object>> key = sortColumn?.ToLower() switch {
                "firstName" => x => x.FirstName,
                "lastName" => x => x.LastName,
                "username" => x => x.Username,
                "email" => x => x.Email,
                _ => x => x.Id
            };

            if (sortOrder?.ToLower() == "desc")
                return users.OrderByDescending(key);
            else
                return users.OrderBy(key);
        }
    }

    public class GetAllUsersQueryValidator : AbstractValidator<GetAllUsersQuery> {
        public GetAllUsersQueryValidator() {

        }
    }
}
