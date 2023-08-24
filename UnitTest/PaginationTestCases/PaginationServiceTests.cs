using Application.Interfaces.Pagination;
using Domain.Common;
using Infrastructure.Services.Pagination;

namespace UnitTest.Pagination {
    public class PaginationServiceTests {

        private readonly IPaginationService<UserData> _paginationService;

        public PaginationServiceTests() {
            _paginationService = new PaginationService<UserData>();
        }

        [Theory]
        [InlineData(1, 5, 50)]
        public void Validate_BetweenLimits_ShouldReturnOriginals(int page, int pageSize, int total) {

            var result = _paginationService.Validate(page, pageSize, total);
            var resultPage = result.Item1;
            var resultPageSize = result.Item2;

            Assert.Equal(page, resultPage);
            Assert.Equal(pageSize, resultPageSize);
        }

        [Theory]
        [InlineData(1, -8, 50)]
        public void Validate_PageSizeSmallerThanLimit_ShouldReturn3(int page, int pageSize, int total) {

            var result = _paginationService.Validate(page, pageSize, total);
            var resultPage = result.Item1;
            var resultPageSize = result.Item2;

            Assert.Equal(page, resultPage);
            Assert.Equal(3, resultPageSize);
        }

        [Theory]
        [InlineData(1, 65, 50)]
        public void Validate_PageSizeBiggerThanLimit_ShouldReturn50(int page, int pageSize, int total) {

            var result = _paginationService.Validate(page, pageSize, total);
            var resultPage = result.Item1;
            var resultPageSize = result.Item2;

            Assert.Equal(page, resultPage);
            Assert.Equal(50, resultPageSize);
        }

        [Theory]
        [InlineData(-15, 14, 50)]
        public void Validate_NegativePage_ShouldReturn1(int page, int pageSize, int total) {

            var result = _paginationService.Validate(page, pageSize, total);
            var resultPage = result.Item1;
            var resultPageSize = result.Item2;

            Assert.Equal(1, resultPage);
            Assert.Equal(pageSize, resultPageSize);
        }

        [Theory]
        [InlineData(100, 14, 50)]
        public void Validate_BlankPage_ShouldReturn1(int page, int pageSize, int total) {

            var result = _paginationService.Validate(page, pageSize, total);
            var resultPage = result.Item1;
            var resultPageSize = result.Item2;

            Assert.Equal(1, resultPage);
            Assert.Equal(pageSize, resultPageSize);
        }

    }

    public class UserData : BaseEntity {
    }
}
