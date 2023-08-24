using Application.Exceptions.ServerErrors;
using Application.Interfaces.Jobs;
using Application.Persistance;
using Domain.Entities;
using InternshipProject.Localizations;
using Microsoft.Extensions.Localization;
using System.Text.Json;

namespace Infrastructure.Services.Hangfire {
    public class JobService : IJobService {

        private readonly ICompanyProfileRepository _companyProfileRepository;
        private readonly IStringLocalizer<LocalizationResources> _localization;

        public JobService(IStringLocalizer<LocalizationResources> localization, ICompanyProfileRepository companyProfileRepository) {
            _localization = localization;
            _companyProfileRepository = companyProfileRepository;
        }

        public async Task RecurringCompanyUpdate() {

            var client = new HttpClient {
                BaseAddress = new Uri("https://finnhub.io/api/v1/stock/")
            };

            var profiles = await _companyProfileRepository.GetAllAsync();

            foreach (var profile in profiles) {

                var response = await client.GetAsync(client.BaseAddress
                    + $"profile2?symbol={profile.Name}&token=cj3rfthr01qlttl4igc0cj3rfthr01qlttl4igcg");

                if (response.IsSuccessStatusCode is false)
                    throw new HttpRequestException(response.ReasonPhrase);

                var stringResponse = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<CompanyProfile>(stringResponse,
                    new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase })
                    ?? throw new ThirdPartyException(_localization.GetString("FinhubError").Value);

                await _companyProfileRepository.UpdateAsync(profile.Id, result);
            }
        }
    }
}
