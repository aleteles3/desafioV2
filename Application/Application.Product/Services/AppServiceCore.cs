using AutoMapper;
using Domain.Product.Interfaces;

namespace Application.Product.Services;

public class AppServiceCore<T> where T : ICoreRepository
{
    protected IMapper Mapper { get; set; }
    protected T Repository { get; set; }

    protected AppServiceCore(IMapper mapper, T repository)
    {
        Mapper = mapper;
        Repository = repository;
    }
}