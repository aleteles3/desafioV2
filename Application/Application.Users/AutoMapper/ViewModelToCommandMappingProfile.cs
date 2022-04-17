using Application.Users.ViewModels.Crud;
using AutoMapper;
using Domain.User.Cqrs.User.Commands;

namespace Application.User.AutoMapper;

public class ViewModelToCommandMappingProfile : Profile
{
    public ViewModelToCommandMappingProfile()
    {
        CreateMap<AddUserViewModel, UserAddCommand>();
    }
}