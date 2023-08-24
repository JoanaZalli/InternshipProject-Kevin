using Application.Interfaces.Pagination;
using Application.Persistance;
using Application.UseCases.BlockedAccounts.Results;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace Application.UseCases.BlockedAccounts.Queries {

    public class BlockedAccountsQuery : IRequest<PagedList<BlockedAccountResult>> {
        public string? Filter { get; set; }
        public string? SortColumn { get; set; }
        public string? SortOrder { get; set; }
        public int PageSize { get; set; }
        public int Page { get; set; }
    }


    public class BlockedAccountsQueryHandler : IRequestHandler<BlockedAccountsQuery, PagedList<BlockedAccountResult>> {

        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IPaginationService<User> _pagination;
        private readonly ILogger<BlockedAccountsQueryHandler> _logger;

        public BlockedAccountsQueryHandler(IMapper mapper,
                                           IUserRepository userRepository,
                                           IPaginationService<User> pagination,
                                           ILogger<BlockedAccountsQueryHandler> logger) {
            _mapper = mapper;
            _userRepository = userRepository;
            _pagination = pagination;
            _logger = logger;
        }

        public async Task<PagedList<BlockedAccountResult>> Handle(BlockedAccountsQuery request, CancellationToken cancellationToken) {

            try {
                var users = _userRepository.GetBlockedAccountsAsync();

                var tuple = _pagination.Validate(request.Page, request.PageSize, users.Count());
                request.Page = tuple.Item1;
                request.PageSize = tuple.Item2;

                users = Filter(users, request.Filter);
                users = Sort(users, request.SortOrder, request.SortColumn);
                var response = await _pagination.PaginateAsync(users, request.Page, request.PageSize);

                return _mapper.Map<PagedList<BlockedAccountResult>>(response);
            }
            catch (Exception ex) {
                _logger.LogError("Error in Blocked Accounts Query Handler", request);

                throw;
            }

        }

        private static IQueryable<User> Filter(IQueryable<User> users, string? filter) {
            if (string.IsNullOrWhiteSpace(filter) is false)
                users = users.Where(x =>
                    x.Username.Contains(filter) ||
                    x.Email.ToString().Contains(filter));

            return users;
        }

        private static IQueryable<User> Sort(IQueryable<User> lenders, string? sortOrder, string? sortColumn) {

            Expression<Func<User, object>> key = sortColumn?.ToLower() switch {
                "username" => x => x.Username,
                "email" => x => x.Email,
                _ => x => x.Id
            };

            if (sortOrder?.ToLower() == "desc")
                return lenders.OrderByDescending(key);
            else
                return lenders.OrderBy(key);
        }

    }

    public class BlockedAccountsQueryValidator : AbstractValidator<BlockedAccountsQuery> {
        public BlockedAccountsQueryValidator() { }
    }
}
