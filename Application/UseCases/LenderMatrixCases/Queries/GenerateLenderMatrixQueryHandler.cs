﻿using Application.Interfaces.Excel;
using Application.Persistance;
using Domain.Exceptions;
using FluentValidation;
using InternshipProject.Localizations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Application.UseCases.LenderMatrixCases.Queries {

    public class GenerateLenderMatrixQuery : IRequest<FileStreamResult> {
        public Guid ProductId { get; set; }
        public Guid LenderId { get; set; }
        public bool IsFilled { get; set; }
    }

    public class GenerateLenderMatrixQueryHandler : IRequestHandler<GenerateLenderMatrixQuery, FileStreamResult> {

        private readonly IProductRepository _productRepository;
        private readonly ILenderRepository _lenderRepository;
        private readonly IExcelService _excelService;
        private readonly IStringLocalizer<LocalizationResources> _localization;
        private readonly ILenderMatrixRepository _lenderMatrixRepository;

        public GenerateLenderMatrixQueryHandler(ILenderRepository lenderRepository, IProductRepository productRepository, IExcelService excelService, IStringLocalizer<LocalizationResources> localization, ILenderMatrixRepository lenderMatrixRepository) {
            _lenderRepository = lenderRepository;
            _productRepository = productRepository;
            _excelService = excelService;
            _localization = localization;
            _lenderMatrixRepository = lenderMatrixRepository;
        }

        public async Task<FileStreamResult> Handle(GenerateLenderMatrixQuery request, CancellationToken cancellationToken) {

            if (await _productRepository.ContainsAsync(request.ProductId) is false)
                throw new NoSuchEntityExistsException(string.Format(_localization.GetString("ProductTypeDoesntExist").Value,
                                                                        request.ProductId));

            if (await _lenderRepository.ContainsAsync(request.LenderId) is false)
                throw new NoSuchEntityExistsException(string.Format(_localization.GetString("LenderDoesntExist").Value,
                                                                        request.LenderId));

            var product = await _productRepository.GetByIdAsync(request.ProductId);
            var lender = await _lenderRepository.GetByIdAsync(request.LenderId);

            if (request.IsFilled is false)
                return _excelService.GenerateMatrixTemplate(lender, product);
            else {
                if (await _lenderMatrixRepository.ContainsAsync(request.LenderId, request.ProductId) is false)
                    throw new NoSuchEntityExistsException(_localization.GetString("NoExcelRowExists").Value);

                var matrices = await _lenderMatrixRepository.GetMatricesAsync(request.LenderId, request.ProductId);
                var lenderName = lender.Name;
                var productName = product.Name;

                return _excelService.GenerateMatrixTemplate(matrices, lenderName, productName);
            }
        }
    }

    public class GenerateLenderMatrixQueryValidator : AbstractValidator<GenerateLenderMatrixQuery> {
        public GenerateLenderMatrixQueryValidator() {
            RuleFor(x => x.ProductId)
                .NotEmpty().WithMessage("EmptyId");

            RuleFor(x => x.LenderId)
                .NotEmpty().WithMessage("EmptyId");
        }
    }

}
