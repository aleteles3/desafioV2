using System.Linq.Expressions;
using Application.Product.ViewModels.Filters;
using PredicateExtensions;
using ProductDomain = Domain.Product.Entities.Product;

namespace Application.Product.Services;

public partial class ProductAppService
{
    private Expression<Func<ProductDomain, bool>> CreateProductQueryPredicate(
        ProductFilterViewModel productFilterViewModel)
    {
        var predicate = PredicateExtensions.PredicateExtensions.Begin<ProductDomain>(true);

        if (productFilterViewModel.Id != null)
            predicate = predicate.And(x => x.Id == productFilterViewModel.Id);
        if (!string.IsNullOrWhiteSpace(productFilterViewModel.Name))
            predicate = predicate.And(x => x.Name.Contains(productFilterViewModel.Name, StringComparison.InvariantCultureIgnoreCase));
        if (productFilterViewModel.PriceStart != null)
            predicate = predicate.And(x => x.Price >= productFilterViewModel.PriceStart);
        if (productFilterViewModel.PriceEnd != null)
            predicate = predicate.And(x => x.Price <= productFilterViewModel.PriceEnd);
        if (productFilterViewModel.CategoryId != null)
            predicate = predicate.And(x => x.CategoryId == productFilterViewModel.CategoryId);
        if (productFilterViewModel.DateIncStart != null)
            predicate = predicate.And(x => x.DateInc >= productFilterViewModel.DateIncStart.Value.Date);
        if (productFilterViewModel.DateIncEnd != null)
            predicate = predicate.And(x => x.DateInc <= productFilterViewModel.DateIncEnd.Value.Date.AddDays(1));

        return predicate;
    }
}