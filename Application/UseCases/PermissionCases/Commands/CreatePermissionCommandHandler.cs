using Application.Exceptions.ServerErrors;
using Application.Persistance;
using Application.Persistance.Common;
using Application.UseCases.ViewPermissions.Results;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using InternshipProject.Localizations;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Permissions.Commands {

    public class CreatePermissionCommand : IRequest<PermissionsResult> {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }

    public class CreatePermissionCommandHandler : IRequestHandler<CreatePermissionCommand, PermissionsResult> {

        private readonly IPermissionRepository _permissionRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStringLocalizer<LocalizationResources> _localization;
        private readonly ILogger<CreatePermissionCommandHandler> _logger;


        public CreatePermissionCommandHandler(IPermissionRepository permissionRepository,
                                              IMapper mapper,
                                              IUnitOfWork unitOfWork,
                                              IStringLocalizer<LocalizationResources> localization,
                                              ILogger<CreatePermissionCommandHandler> logger) {
            _permissionRepository = permissionRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _localization = localization;
            _logger = logger;
        }

        public async Task<PermissionsResult> Handle(CreatePermissionCommand request, CancellationToken cancellationToken) {

            try {
                var permission = _mapper.Map<Permission>(request);
                permission.Id = Guid.NewGuid();

                await _permissionRepository.CreateAsync(permission);

                var flag = await _unitOfWork.SaveChangesAsync();
                if (flag is false)
                    throw new DatabaseException(_localization.GetString("DatabaseException").Value);

                return _mapper.Map<PermissionsResult>(permission);
            }
            catch (Exception ex) {
                _logger.LogError("Error in Create Permission Command Handler", request);

                throw;
            }
        }
    }

    public class CreatePermissionCommandValidator : AbstractValidator<CreatePermissionCommand> {
        public CreatePermissionCommandValidator() {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("EmptyPermissionName");

            RuleFor(x => x.Description)
                .MaximumLength(250).WithMessage("PermissionDescriptionRestriction");
        }
    }
}
