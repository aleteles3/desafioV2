using Application.Product.ViewModels.Grid;
using AutoMapper;
using Domain.Product.Entities;
using ProductDomain = Domain.Product.Entities.Product;

namespace Application.Product.AutoMapper;

public class DomainToViewModelMappingProfile : Profile
{
    public DomainToViewModelMappingProfile()
    {
        CreateMap<Category, CategoryViewModel>();
        CreateMap<ProductDomain, ProductViewModel>();
    }
}