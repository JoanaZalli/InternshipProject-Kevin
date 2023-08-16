﻿using Application.Exceptions.ServerErrors;
using Application.Persistance;
using Application.Persistance.Common;
using Application.UseCases.ViewRoles.Results;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using InternshipProject.Localizations;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Application.UseCases.Roles.Commands {

    public class CreateRoleCommand : IRequest<RoleResult> {
        public string Name { get; set; } = null!;
    }

    public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, RoleResult> {

        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStringLocalizer<LocalizationResources> _localization;

        public CreateRoleCommandHandler(IMapper mapper,
                                        IRoleRepository roleRepository,
                                        IUnitOfWork unitOfWork,
                                        IStringLocalizer<LocalizationResources> localization) {
            _mapper = mapper;
            _roleRepository = roleRepository;
            _unitOfWork = unitOfWork;
            _localization = localization;
        }

        public async Task<RoleResult> Handle(CreateRoleCommand request, CancellationToken cancellationToken) {
            var role = _mapper.Map<Role>(request);
            role.Id = Guid.NewGuid();

            await _roleRepository.CreateAsync(role);

            var flag = await _unitOfWork.SaveChangesAsync();
            if (flag is false)
                throw new DatabaseException(_localization.GetString("DatabaseException").Value);

            return _mapper.Map<RoleResult>(role);
        }
    }

    public class CreateRoleCommandValidator : AbstractValidator<CreateRoleCommand> {
        public CreateRoleCommandValidator() {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("EmptyRoleName");
        }
    }
}
