using Application.Product.ViewModels.Crud;
using AutoMapper;
using Domain.Product.Cqrs.Category.Commands;
using Domain.Product.Cqrs.Product.Commands;

namespace Application.Product.AutoMapper;

public class ViewModelToCommandMappingProfile : Profile
{
    public ViewModelToCommandMappingProfile()
    {
        CreateMap<AddCategoryViewModel, CategoryAddCommand>();
        CreateMap<UpdateCategoryViewModel, CategoryUpdateCommand>();
        
        CreateMap<AddProductViewModel, ProductAddCommand>();
        CreateMap<UpdateProductViewModel, ProductUpdateCommand>();
    }
}