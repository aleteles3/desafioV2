using Application.Product.ViewModels.Grid;
using AutoMapper;
using Domain.Product.Entities;
using ProductDomain = Domain.Product.Entities.Product;

namespace Application.Product.AutoMapper;

public class DomainToDtoMappingProfile : Profile
{
    public DomainToDtoMappingProfile()
    {
        CreateMap<Category, CategoryViewModel>();
        CreateMap<ProductDomain, ProductViewModel>();
    }
}