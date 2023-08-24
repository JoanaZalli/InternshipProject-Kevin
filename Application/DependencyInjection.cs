using Application.UseCases.Common;
using Application.Validator;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace Application {
    public static class DependencyInjection {
        public static IServiceCollection AddApplicationLayer(this IServiceCollection services, IConfiguration configuration) {
            ConfigureAutoMapper(services);
            ConfigureMediatR(services);
            ConfigureFluentValidation(services);
            BindFinhubConnection(services, configuration);

            return services;
        }

        private static void ConfigureMediatR(IServiceCollection services) {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        }

        private static void ConfigureFluentValidation(IServiceCollection services) {
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        }

        private static void ConfigureAutoMapper(IServiceCollection services) {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
        }

        private static void BindFinhubConnection(IServiceCollection services, IConfiguration configuration) {
            // object model the section defined in appsettings.json
            var finHub = new FinHubConnectionSettings();
            configuration.Bind(FinHubConnectionSettings.SectionName, finHub);
            services.AddSingleton(Options.Create(finHub));
        }
    }
}
