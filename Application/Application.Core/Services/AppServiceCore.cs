using AutoMapper;
using Domain.Core.Interfaces;

namespace Application.Core.Services;

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