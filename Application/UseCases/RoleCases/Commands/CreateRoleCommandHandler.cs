using Application.Exceptions.ServerErrors;
using Application.Persistance;
using Application.Persistance.Common;
using Application.UseCases.ViewRoles.Results;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using InternshipProject.Localizations;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Roles.Commands {

    public class CreateRoleCommand : IRequest<RoleResult> {
        public string Name { get; set; } = null!;
    }

    public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, RoleResult> {

        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStringLocalizer<LocalizationResources> _localization;
        private readonly ILogger<CreateRoleCommandHandler> _logger;

        public CreateRoleCommandHandler(IMapper mapper,
                                        IRoleRepository roleRepository,
                                        IUnitOfWork unitOfWork,
                                        IStringLocalizer<LocalizationResources> localization,
                                        ILogger<CreateRoleCommandHandler> logger) {
            _mapper = mapper;
            _roleRepository = roleRepository;
            _unitOfWork = unitOfWork;
            _localization = localization;
            _logger = logger;
        }

        public async Task<RoleResult> Handle(CreateRoleCommand request, CancellationToken cancellationToken) {

            try {
                var role = _mapper.Map<Role>(request);
                role.Id = Guid.NewGuid();

                await _roleRepository.CreateAsync(role);

                var flag = await _unitOfWork.SaveChangesAsync();
                if (flag is false)
                    throw new DatabaseException(_localization.GetString("DatabaseException").Value);

                return _mapper.Map<RoleResult>(role);
            }
            catch (Exception ex) {
                _logger.LogError("Error in Create Role Command Handler", request);

                throw;
            }
        }
    }

    public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand> {
        public CreateRoleCommandValidator() {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("EmptyRoleName");
        }
    }
}
