using Application.Interfaces.Pagination;
using Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Pagination {
    public class PaginationService<T> : IPaginationService<T> where T : BaseEntity {
        public async Task<PagedList<T>> PaginateAsync(IQueryable<T> query, int page, int pageSize) {
            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            bool hasNext = page * pageSize < totalCount;
            bool hasPrev = page > 1;

            return new PagedList<T> {
                Items = items,
                Page = page,
                PageSize = pageSize,
                TotalCount = totalCount,
                HasNextPage = hasNext,
                HasPreviousPage = hasPrev
            };
        }

        public Tuple<int, int> Validate(int page, int pageSize, int total) {

            if (pageSize > 50)
                pageSize = 50;
            else if (pageSize < 3)
                pageSize = 3;

            if (page < 1 || (total > 0 && page > Math.Ceiling((double)total / pageSize)))
                page = 1;

            return Tuple.Create(page, pageSize);
        }
    }
}
