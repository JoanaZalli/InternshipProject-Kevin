using Application.Exceptions.ClientErrors;
using Application.Exceptions.ServerErrors;
using Application.Persistance;
using Application.Persistance.Common;
using FluentValidation;
using InternshipProject.Localizations;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace Application.UseCases.Permissions.Commands {

    public class DeletePermissionCommand : IRequest<bool> {
        public Guid PermissionId { get; set; }
    }

    public class DeletePermissionCommandHandler : IRequestHandler<DeletePermissionCommand, bool> {

        private readonly IPermissionRepository _permissionRepository;
        private readonly IStringLocalizer<LocalizationResources> _localization;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeletePermissionCommandHandler> _logger;

        public DeletePermissionCommandHandler(IPermissionRepository permissionRepository,
                                              IStringLocalizer<LocalizationResources> localizer,
                                              IUnitOfWork unitOfWork,
                                              ILogger<DeletePermissionCommandHandler> logger) {
            _permissionRepository = permissionRepository;
            _localization = localizer;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<bool> Handle(DeletePermissionCommand request, CancellationToken cancellationToken) {

            try {
                if (await _permissionRepository.ContainsAsync(request.PermissionId) is false)
                    throw new NotFoundException(_localization.GetString("PermissionDoesntExist").Value);

                await _permissionRepository.DeleteAsync(request.PermissionId);

                var flag = await _unitOfWork.SaveChangesAsync();
                if (flag is false)
                    throw new DatabaseException(_localization.GetString("DatabaseException").Value);

                return true;
            }
            catch (Exception ex) {
                _logger.LogError("Error in Delete Permission Command Handler", request);

                throw;
            }
        }
    }

    public class DeletePermissionCommandValidator : AbstractValidator<DeletePermissionCommand> {
        public DeletePermissionCommandValidator() {
            RuleFor(x => x.PermissionId)
                .NotEmpty().WithMessage("EmptyPermissionId");
        }
    }

}
