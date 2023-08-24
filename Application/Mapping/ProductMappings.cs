using Application.Interfaces.Pagination;
using Application.UseCases.ProductCases.Results;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapping {
    public class ProductMappings : Profile {
        public ProductMappings() {

            CreateMap<ProductsResult, Product>().ReverseMap();

            CreateMap<PagedList<Product>, PagedList<ProductsResult>>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items))
                .ReverseMap();
        }
    }
}
