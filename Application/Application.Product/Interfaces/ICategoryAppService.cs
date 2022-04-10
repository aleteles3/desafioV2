using Application.Product.ViewModels.Crud;
using Application.Product.ViewModels.Filters;
using Application.Product.ViewModels.Grid;

namespace Application.Product.Interfaces;

public interface ICategoryAppService
{
    Task<CategoryViewModel> GetCategoryById(Guid id);
    Task<IEnumerable<CategoryViewModel>> GetCategories(CategoryFilterViewModel categoryFilterViewModel);
    Task<Guid?> AddCategory(AddCategoryViewModel addCategoryViewModel);
    Task UpdateCategory(UpdateCategoryViewModel updateCategoryViewModel);
    Task RemoveCategory(Guid categoryId);
}