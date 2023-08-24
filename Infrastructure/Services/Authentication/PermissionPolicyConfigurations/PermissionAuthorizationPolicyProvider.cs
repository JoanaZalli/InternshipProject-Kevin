using Infrastructure.Services.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Infrastructure.Services.Authentication.PermissionPolicyConfigurations {
    public class PermissionAuthorizationPolicyProvider
        : DefaultAuthorizationPolicyProvider {
        public PermissionAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options) : base(options) {
        }


        /*  Automatic alternative to manual policy registration
         *  The alternative would be manually declaring each new permission
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
