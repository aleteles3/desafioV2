using Application.Product.Cqrs.Product.Commands;
using Application.Product.ViewModels.Crud;
using AutoMapper;
using Domain.Product.Cqrs.Category.Commands;

namespace Application.Product.AutoMapper;

public class ViewModelToCommandMappingProfile : Profile
{
    public ViewModelToCommandMappingProfile()
    {
        CreateMap<AddCategoryViewModel, CategoryAddCommand>();
        CreateMap<AddProductViewModel, ProductAddCommand>();
        CreateMap<UpdateCategoryViewModel, CategoryUpdateCommand>();
    }
}