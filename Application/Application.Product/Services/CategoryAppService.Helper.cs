using System.Linq;
using System.Linq.Expressions;
using Application.Product.ViewModels.Filters;
using Domain.Product.Entities;
using PredicateExtensions;

namespace Application.Product.Services;

public partial class CategoryAppService
{
    private Expression<Func<Category, bool>> CreateCategoryQueryPredicate(
        CategoryFilterViewModel categoryFilterViewModel)
    {
        var predicate = PredicateExtensions.PredicateExtensions.Begin<Category>(true);

        if (categoryFilterViewModel.Id != null)
            predicate = predicate.And(x => x.Id == categoryFilterViewModel.Id);
        if (!string.IsNullOrWhiteSpace(categoryFilterViewModel.Name))
            predicate = predicate.And(x => x.Name.Contains(categoryFilterViewModel.Name, StringComparison.InvariantCultureIgnoreCase));
        if (categoryFilterViewModel.DateIncStart != null)
            predicate = predicate.And(x => x.DateInc >= categoryFilterViewModel.DateIncStart.Value.Date);
        if (categoryFilterViewModel.DateIncEnd != null)
            predicate = predicate.And(x => x.DateInc <= categoryFilterViewModel.DateIncEnd.Value.Date.AddDays(1));

        return predicate;
    }
}