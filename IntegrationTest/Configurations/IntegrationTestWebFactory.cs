using Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationTest.Configurations {
    public class IntegrationTestWebFactory : WebApplicationFactory<Program> {

        protected override void ConfigureWebHost(IWebHostBuilder builder) {

            builder.ConfigureTestServices(services => {
                var desc = services.SingleOrDefault(s => s.ServiceType == typeof(DbContextOptions<DatabaseContext>));

                if (desc is not null)
                    services.Remove(desc);

                services.AddDbContext<DatabaseContext>(opts => {
                    opts.UseSqlServer("Server=localhost,1433;Database=InternshipDatabaseTesting1;User Id=sa;Password=Pass@word;Persist Security Info=False;TrustServerCertificate=True");
                });
            });

        }

    }
}
