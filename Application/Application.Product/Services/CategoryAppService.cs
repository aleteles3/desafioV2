using Application.Core.Services;
using Application.Product.Interfaces;
using Application.Product.ViewModels.Crud;
using Application.Product.ViewModels.Filters;
using Application.Product.ViewModels.Grid;
using AutoMapper;
using Domain.Product.Cqrs.Category.Commands;
using Domain.Product.Interfaces;
using MediatR;

namespace Application.Product.Services;

public partial class CategoryAppService : AppServiceCore<ICategoryRepository>, ICategoryAppService
{
    public CategoryAppService(IMapper mapper, ICategoryRepository repository, IMediator mediator) 
        : base(mapper, repository, mediator) { }
    
    public async Task<CategoryViewModel> GetCategoryById(Guid id)
    {
        var result = await Repository.GetCategoryByIdAsync(id);

        return Mapper.Map<CategoryViewModel>(result);
    }

    public async Task<IEnumerable<CategoryViewModel>> GetCategories(CategoryFilterViewModel categoryFilterViewModel,
        int? start = null, int? length = null)
    {
        var predicate = CreateCategoryQueryPredicate(categoryFilterViewModel);

        var result = await Repository.GetCategoriesAsync(predicate, start, length);

        return Mapper.Map<IEnumerable<CategoryViewModel>>(result);
    }

    public async Task<Guid?> AddCategory(AddCategoryViewModel addCategoryViewModel)
    {
        var command = Mapper.Map<CategoryAddCommand>(addCategoryViewModel);

        return await Mediator.Send(command);
    }

    public Task UpdateCategory(UpdateCategoryViewModel updateCategoryViewModel)
    {
        throw new NotImplementedException();
    }

    public Task RemoveCategory(Guid categoryId)
    {
        throw new NotImplementedException();
    }
}