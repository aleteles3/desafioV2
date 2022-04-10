using Application.Core.Services;
using Application.Product.Interfaces;
using Application.Product.ViewModels.Crud;
using Application.Product.ViewModels.Filters;
using Application.Product.ViewModels.Grid;
using AutoMapper;
using Domain.Product.Interfaces;

namespace Application.Product.Services;

public partial class ProductAppService : AppServiceCore<IProductRepository>, IProductAppService
{
    protected ProductAppService(IMapper mapper, IProductRepository repository) : base(mapper, repository) { }

    public async Task<ProductViewModel> GetProductById(Guid id)
    {
        var result = await Repository.GetProductByIdAsync(id);

        return Mapper.Map<ProductViewModel>(result);
    }

    public async Task<IEnumerable<ProductViewModel>> GetProducts(ProductFilterViewModel productFilterViewModel,
        int? start = null, int? length = null)
    {
        var predicate = CreateProductQueryPredicate(productFilterViewModel);

        var result = await Repository.GetProductsAsync(predicate, start, length);

        return Mapper.Map<IEnumerable<ProductViewModel>>(result);
    }

    public Task<Guid?> AddProduct(AddProductViewModel addProductViewModel)
    {
        throw new NotImplementedException();
    }

    public Task UpdateProduct(UpdateProductViewModel updateProductViewModel)
    {
        throw new NotImplementedException();
    }

    public Task RemoveProduct(Guid id)
    {
        throw new NotImplementedException();
    }
}