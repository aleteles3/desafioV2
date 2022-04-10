using Application.Product.ViewModels.Crud;
using Application.Product.ViewModels.Filters;
using Application.Product.ViewModels.Grid;
namespace Application.Product.Interfaces;

public interface IProductAppService
{
    Task<ProductViewModel> GetProductById(Guid id);
    Task<IEnumerable<ProductViewModel>> GetProducts(ProductFilterViewModel productFilterViewModel,
        int? start = null, int? length = null);
    Task<Guid?> AddProduct(AddProductViewModel addProductViewModel);
    Task UpdateProduct(UpdateProductViewModel updateProductViewModel);
    Task RemoveProduct(Guid id);
}