using Application.Interfaces.Authentication;
using Application.Interfaces.Email;
using Application.Interfaces.Excel;
using Application.Interfaces.Jobs;
using Application.Interfaces.Pagination;
using Application.Persistance;
using Application.Persistance.Common;
using Application.Services;
using Infrastructure.Persistence.Context;
using Infrastructure.Persistence.Repositories;
using Infrastructure.Persistence.Repositories.Common;
using Infrastructure.Services.Authentication;
using Infrastructure.Services.Common;
using Infrastructure.Services.Email;
using Infrastructure.Services.Excel;
using Infrastructure.Services.Hangfire;
using Infrastructure.Services.Pagination;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Infrastructure {
    public static class DependencyInjection {
        public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration) {
            AddScopes(services);
            AddDatabaseConnection(services, configuration);
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            AddAuthentication(services, configuration);
            ConfigureEmailService(services, configuration);
            return services;
        }

        private static void AddScopes(IServiceCollection services) {
            services.AddScoped<IJwtToken, JwtToken>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPermissionRepository, PermissionRepository>();
            services.AddScoped<IUserVerificationAndResetRepository, UserVerificationAndResetRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IHasherService, HasherService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IMailBodyService, MailBodyService>();
            services.AddScoped<ICompanyTypeRepository, CompanyTypeRepository>();
            services.AddScoped<IBorrowerRepository, BorrowerRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ICompanyProfileRepository, CompanyProfileRepository>();
            services.AddScoped<IApplicationRepository, ApplicationRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IExcelService, ExcelService>();
            services.AddScoped<ILenderMatrixRepository, LenderMatrixRepository>();
            services.AddScoped<ILenderRepository, LenderRepository>();
            services.AddScoped<ILoanRepository, LoanRepository>();
            services.AddScoped<IApplicationStatusRepository, ApplicationStatusRepository>();
            services.AddScoped<ILoanStatusRepository, LoanStatusRepository>();
            services.AddScoped<IJobService, JobService>();
            services.AddScoped(typeof(IPaginationService<>), typeof(PaginationService<>));
        }

        private static void AddDatabaseConnection(IServiceCollection services, IConfiguration configuration) {
            var connString = configuration.GetConnectionString("DbConnection");
            services.AddDbContext<DatabaseContext>(options =>
                options.UseSqlServer(connString, b => b.MigrationsAssembly("Infrastructure")));
        }

        private static void AddAuthentication(IServiceCollection services, IConfiguration configuration) {

            // object model the section defined in appsettings.json
            var JwtSettings = new JwtSettings();
            configuration.Bind(JwtSettings.SectionName, JwtSettings);
            services.AddSingleton(Options.Create(JwtSettings));

            services.AddAuthentication(opts => {
                opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(opts =>
                opts.TokenValidationParameters = new TokenValidationParameters {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = JwtSettings.Issuer,
                    ValidAudience = JwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(JwtSettings.Secret))
                }
                );
        }

        private static void ConfigureEmailService(IServiceCollection services, IConfiguration configuration) {
            services.Configure<MailSettings>(configuration.GetSection(nameof(MailSettings)));
            services.AddTransient<IMailService, MailService>();
        }
    }
}
