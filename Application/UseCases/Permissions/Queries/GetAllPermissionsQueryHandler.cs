﻿using Application.Interfaces.Pagination;
using Application.Persistance;
using Application.UseCases.ViewPermissions.Results;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using MediatR;
using System.Linq.Expressions;

namespace Application.UseCases.ViewPermissions.Queries {
    public class GetAllPermissionsQuery : IRequest<PagedList<PermissionsResult>> {
        public string? Filter { get; set; }
        public string? SortColumn { get; set; }
        public string? SortOrder { get; set; }
        public int PageSize { get; set; }
        public int Page { get; set; }
    }

    public class GetAllPermissionsQueryHandler : IRequestHandler<GetAllPermissionsQuery, PagedList<PermissionsResult>> {

        private readonly IPermissionRepository _permissionRepository;
        private readonly IMapper _mapper;
        private readonly IPaginationService<Permission> _pagination;

        public GetAllPermissionsQueryHandler(IPermissionRepository permissionRepository,
                                             IMapper mapper,
                                             IPaginationService<Permission> pagination) {
            _permissionRepository = permissionRepository;
            _mapper = mapper;
            _pagination = pagination;
        }

        public async Task<PagedList<PermissionsResult>> Handle(GetAllPermissionsQuery request, CancellationToken cancellationToken) {

            var permissions = _permissionRepository.GetIQueryable();

            var tuple = _pagination.Validate(request.Page, request.PageSize, permissions.Count());
            request.Page = tuple.Item1;
            request.PageSize = tuple.Item2;

            permissions = Filter(permissions, request.Filter);
            permissions = Sort(permissions, request.SortOrder, request.SortColumn);
            var response = await _pagination.PaginateAsync(permissions, request.Page, request.PageSize);

            return _mapper.Map<PagedList<PermissionsResult>>(response);
        }

        private static IQueryable<Permission> Filter(IQueryable<Permission> permissions, string? filter) {
            if (string.IsNullOrWhiteSpace(filter) is false)
                permissions = permissions.Where(x =>
                    x.Name.Contains(filter));

            return permissions;
        }

        private static IQueryable<Permission> Sort(IQueryable<Permission> permissions, string? sortOrder, string? sortColumn) {

            Expression<Func<Permission, object>> key = sortColumn?.ToLower() switch {
                "name" => x => x.Name,
                _ => x => x.Id
            };

            if (sortOrder?.ToLower() == "desc")
                return permissions.OrderByDescending(key);
            else
                return permissions.OrderBy(key);
        }
    }

    public class GetAllPermissionsQueryValidator : AbstractValidator<GetAllPermissionsQuery> {
        public GetAllPermissionsQueryValidator() { }
    }
}
