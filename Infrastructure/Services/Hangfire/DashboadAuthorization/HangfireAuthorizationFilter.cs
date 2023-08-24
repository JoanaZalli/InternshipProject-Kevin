using Hangfire.Annotations;
using Hangfire.Dashboard;

namespace Infrastructure.Services.Hangfire.DashboardAuthorization {
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter {

        public bool Authorize([NotNull] DashboardContext context) {
            return true;
        }
    }
}
