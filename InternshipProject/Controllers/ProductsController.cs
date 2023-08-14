﻿using Application.UseCases.ProductCases.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace InternshipProject.Controllers {
    public class ProductsController : ControllerBase {

        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator) {
            _mediator = mediator;
        }

        [SwaggerOperation(Summary = "Get products")]
        //[Authorize(Policy = $"{PermissionSeeds.CanReadApplications}, {PermissionSeeds.IsSuperAdmin}")]
        [HttpGet("products")]
        public async Task<IActionResult> GetProducts() {
            var result = await _mediator.Send(new GetProductsQuery { });
            return Ok(result);
        }
    }
}