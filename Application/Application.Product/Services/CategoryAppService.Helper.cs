using System.Linq.Expressions;
using Application.Product.ViewModels.Filters;
using Domain.Product.Entities;

namespace Application.Product.Services;

public partial class CategoryAppService
{
    private Expression<Func<Category, bool>> CreateCategoryQueryPredicate(
        CategoryFilterViewModel categoryFilterViewModel)
    {
        throw new NotImplementedException();
    }
}