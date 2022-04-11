using AutoMapper;
using Domain.Core.Interfaces;
using MediatR;

namespace Application.Core.Services;

public class AppServiceCore<T> where T : ICoreRepository
{
    protected IMapper Mapper { get; set; }
    protected T Repository { get; set; }
    protected IMediator Mediator { get; set; }

    protected AppServiceCore(IMapper mapper, T repository, IMediator mediator)
    {
        Mapper = mapper;
        Repository = repository;
        Mediator = mediator;
    }
}