using Infrastructure.Persistence.Context;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationTest.Configurations {
    public abstract class BaseIntegrationTest : IClassFixture<IntegrationTestWebFactory> {

        private readonly IServiceScope _scope;
        protected readonly IMediator _mediator;
        protected readonly DatabaseContext _dbContext;

        protected BaseIntegrationTest(IntegrationTestWebFactory factory) {
            _scope = factory.Services.CreateScope();

            _mediator = _scope.ServiceProvider.GetRequiredService<IMediator>();

            _dbContext = _scope.ServiceProvider.GetRequiredService<DatabaseContext>();
        }
    }
}