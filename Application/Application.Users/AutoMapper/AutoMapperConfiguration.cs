using AutoMapper;

namespace Application.User.AutoMapper;

public class AutoMapperConfiguration
{
    public static MapperConfiguration RegisterMappings()
    {
        return new MapperConfiguration(x =>
        {
            x.AddProfile(new ViewModelToCommandMappingProfile());
        });
    }
}