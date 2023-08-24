using Application.UseCases.ApplicationJourney.Commands;
using Application.UseCases.Authentication.Commands;
using Application.UseCases.BorrowerJourney.Commands;
using Application.UseCases.ForgotPassword.Commands;
using Application.UseCases.GenerateRefreshToken.Commands;
using Application.UseCases.LoanJourney.Commands;
using Application.UseCases.Permissions.Commands;
using Application.UseCases.ResendEmailVerification.Commands;
using Application.UseCases.Roles.Commands;
using AutoMapper;
using InternshipProject.Objects.Requests.ApplicationJourneyRequests;
using InternshipProject.Objects.Requests.AuthenticationRequests;
using InternshipProject.Objects.Requests.BorrowerJourneyRequests;
using InternshipProject.Objects.Requests.LoanRequests;
using InternshipProject.Objects.Requests.ManagementRequests;
using InternshipProject.Objects.Requests.PermissionRequests;
using InternshipProject.Objects.Requests.RoleRequests;

namespace InternshipProject.Mapping {
    public class CommandMappings : Profile {
        public CommandMappings() {
            CreateMap<RegisterRequest, RegisterCommand>().ReverseMap();

            CreateMap<LogInRequest, LoginCommand>().ReverseMap();

            CreateMap<ResendVerificationEmailRequest, ResendEmailVerificationCommand>().ReverseMap();

            CreateMap<ForgotPasswordRequest, ForgotPasswordCommand>().ReverseMap();

            CreateMap<PermissionRequest, CreatePermissionCommand>().ReverseMap();

            CreateMap<RoleRequest, CreateRoleCommand>().ReverseMap();

            CreateMap<AssignationRequest, RoleAssignationCommand>()
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<AssignationRequest, PermissionAssignationCommand>()
                .ForMember(dest => dest.RoleId, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<RefreshTokenRequest, RefreshTokenCommand>().ReverseMap();

            CreateMap<BorrowerRequest, CreateBorrowerCommmand>().ReverseMap();

            CreateMap<BorrowerRequest, UpdateBorrowerCommand>().ReverseMap();

            CreateMap<UpdateApplicationRequest, UpdateApplicationCommand>().ReverseMap();

            CreateMap<CreateApplicationRequest, CreateApplicationCommand>().ReverseMap();

            CreateMap<CreateLoanRequest, CreateLoanCommand>().ReverseMap();
        }
    }
}
