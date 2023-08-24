namespace Application.Interfaces.Pagination {
    public interface IPaginationService<T> {
        Task<PagedList<T>> PaginateAsync(IQueryable<T> query, int page, int pageSize);
        Tuple<int, int> Validate(int page, int pageSize, int total);
    }
}
