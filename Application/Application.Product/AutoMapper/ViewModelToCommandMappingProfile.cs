using Application.Product.Cqrs.Category.Commands;
using Application.Product.Cqrs.Product.Commands;
using Application.Product.ViewModels.Crud;
using AutoMapper;

namespace Application.Product.AutoMapper;

public class ViewModelToCommandMappingProfile : Profile
{
    public ViewModelToCommandMappingProfile()
    {
        CreateMap<AddCategoryViewModel, CategoryAddCommand>();
        CreateMap<AddProductViewModel, ProductAddCommand>();
    }
}