using Application.UseCases.ForgotUsername.Queries;
using AutoMapper;
using InternshipProject.Objects.Requests.AuthenticationRequests;

namespace InternshipProject.Mapping {
    public class QueryMappings : Profile {
        public QueryMappings() {
            CreateMap<ForgotUsernameRequest, ForgotUsernameQuery>().ReverseMap();
        }
    }
}
