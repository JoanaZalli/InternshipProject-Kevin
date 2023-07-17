﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services.Authentication {
    public class PermissionAuthorizationPolicyProvider
        : DefaultAuthorizationPolicyProvider {
        public PermissionAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options) : base(options) {
        }


        /*  Automatic alternative to manual policy registration
         *  
         *  services.AddAuthorization(options =>
         *  {
         *      options.AddPolicy("MyPolicy", policy =>
         *      {
         *          policy.Requirements.Add(new PermissionRequirement("MyPermission"));
         *      });
         *  });
         */


        public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName) {
                    var policy = await base.GetPolicyAsync(policyName);

                    if (policy is not null)
                        return policy;

                    return new AuthorizationPolicyBuilder()
                        .AddRequirements(new PermissionRequirement(policyName))
                        .Build();
                }
            }
        }
