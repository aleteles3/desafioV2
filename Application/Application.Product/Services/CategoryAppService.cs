using Application.Product.Interfaces;
using Application.Product.ViewModels.Crud;
using Application.Product.ViewModels.Filters;
using Application.Product.ViewModels.Grid;
using AutoMapper;
using Domain.Product.Interfaces;

namespace Application.Product.Services;

public partial class CategoryAppService : AppServiceCore<ICategoryRepository>, ICategoryAppService
{
    protected CategoryAppService(IMapper mapper, ICategoryRepository repository) : base(mapper, repository) { }
    
    public async Task<CategoryViewModel> GetCategoryById(Guid id)
    {
        var category = await Repository.GetCategoryByIdAsync(id);

        return Mapper.Map<CategoryViewModel>(category);
    }

    public Task<IEnumerable<CategoryViewModel>> GetCategories(CategoryFilterViewModel categoryFilterViewModel)
    {
        throw new NotImplementedException();
    }

    public Task<Guid?> AddCategory(AddCategoryViewModel addCategoryViewModel)
    {
        throw new NotImplementedException();
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