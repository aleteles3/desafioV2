using Application.Product.ViewModels.Crud;
using AutoMapper;
using Domain.Product.Cqrs.Category.Commands;
using Domain.Product.Cqrs.Order.Commands;
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

        CreateMap<AddOrderViewModel, OrderAddCommand>()
            .ForMember(x => x.UserId, opt => opt.MapFrom((_, _, _, context) => context.Items["UserId"]))
            .ForMember(x => x.OrderItemAddCommands, opt => opt.MapFrom(src => src.AddOrderItems));
        CreateMap<AddOrderItemViewModel, OrderItemAddCommand>();
    }
}