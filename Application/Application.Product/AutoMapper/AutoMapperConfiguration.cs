using AutoMapper;

namespace Application.Product.AutoMapper;

public class AutoMapperConfiguration
{
    public static MapperConfiguration RegisterMappings()
    {
        return new MapperConfiguration(x =>
        {
            x.AddProfile(new DomainToDtoMappingProfile());
        });
    }
}