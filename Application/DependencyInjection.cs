﻿using Application.UseCases.Authentication;
using Application.UseCases.Authentication.Commands;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public static class DependencyInjection {
        public static IServiceCollection AddApplicationLayer(this IServiceCollection services) {
            AddScopes(services);
            AddMediatR(services);
            return services;
        } 

        private static void AddScopes(IServiceCollection services) {
            
        }

        private static void AddMediatR(IServiceCollection services) {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        }
    }
}
