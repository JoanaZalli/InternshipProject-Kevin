﻿using Application.Exceptions.ClientErrors;
using Application.Exceptions.ServerErrors;
using Application.Persistance;
using Application.Persistance.Common;
using Domain.Entities;
using FluentValidation;
using InternshipProject.Localizations;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Application.UseCases.Roles.Commands {

    public class RoleAssignationCommand : IRequest<bool> {
        public Guid UserId { get; set; }
        public List<Guid> Ids { get; set; } = null!;
    }

    public class RoleAssignationCommandHandler : IRequestHandler<RoleAssignationCommand, bool> {

        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IStringLocalizer<LocalizationResources> _localization;
        private readonly IUnitOfWork _unitOfWork;

        public RoleAssignationCommandHandler(IUserRepository userRepository,
                                             IRoleRepository roleRepository,
                                             IStringLocalizer<LocalizationResources> localizer,
                                             IUnitOfWork unitOfWork) {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _localization = localizer;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(RoleAssignationCommand request, CancellationToken cancellationToken) {

            if (request.Ids.Any() is false)
                await _userRepository.ClearRolesAsync(request.UserId);

            var roles = await _roleRepository.GetAllAsync();
            var roleIds = roles.Select(x => x.Id).AsEnumerable();

            // Check if provided id values exist in database
            var flag = request.Ids.All(item => roleIds.Contains(item));

            if (flag is false)
                throw new InvalidRequestException(_localization.GetString("InvalidRoles").Value);

            await _userRepository.UpdateRolesAsync(request.UserId, GetRoles(request.Ids, roles));

            var dbFlag = await _unitOfWork.SaveChangesAsync();
            if (dbFlag is false)
                throw new DatabaseException(_localization.GetString("DatabaseException").Value);

            return true;
        }

        // we are getting a list of Ids, we need to get the entities associated
        // with these ids in order to perform the insertion in the db
        private static List<Role> GetRoles(IEnumerable<Guid> ids, IEnumerable<Role> roles) {
            var list = new List<Role>();
            foreach (var role in roles)
                if (ids.Contains(role.Id))
                    list.Add(role);
            return list;
        }
    }

    public class RoleAssignationCommandValidator : AbstractValidator<RoleAssignationCommand> {
        public RoleAssignationCommandValidator() {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("EmptyUserId");
        }
    }
}