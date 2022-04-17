using Application.Core.Services;
using Application.Product.Interfaces;
using Application.Product.ViewModels.Crud;
using Application.Product.ViewModels.Filters;
using Application.Product.ViewModels.Grid;
using AutoMapper;
using Domain.Core.Interfaces;
using Domain.Product.Cqrs.Product.Commands;
using Domain.Product.Interfaces;
using MediatR;

namespace Application.Product.Services;

public partial class ProductAppService : AppServiceCore<IProductRepository>, IProductAppService
{
    public ProductAppService(IMapper mapper, IProductRepository repository, IMediator mediator, IMemoryBus memoryBus) 
        : base(mapper, repository, mediator, memoryBus) { }

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

    public async Task<Guid?> AddProduct(AddProductViewModel addProductViewModel)
    {
        var command = Mapper.Map<ProductAddCommand>(addProductViewModel);

        return await Mediator.Send(command);
    }

    public async Task UpdateProduct(UpdateProductViewModel updateProductViewModel)
    {
        var command = Mapper.Map<ProductUpdateCommand>(updateProductViewModel);

        await Mediator.Send(command);
    }

    public async Task RemoveProduct(Guid id)
    {
        var command = new ProductRemoveCommand(id);

        await Mediator.Send(command);
    }
}