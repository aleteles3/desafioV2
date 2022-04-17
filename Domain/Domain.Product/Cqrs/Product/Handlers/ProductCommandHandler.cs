using Domain.Core.Commands;
using Domain.Core.Interfaces;
using Domain.Product.Cqrs.Product.Commands;
using Domain.Product.Interfaces;
using MediatR;
using ProductDomain = Domain.Product.Entities.Product;

namespace Domain.Product.Cqrs.Product.Handlers;

public class ProductCommandHandler : CommandHandler,
    IRequestHandler<ProductAddCommand, Guid?>, 
    IRequestHandler<ProductUpdateCommand>, 
    IRequestHandler<ProductRemoveCommand>
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;

    public ProductCommandHandler(IProductRepository productRepository, ICategoryRepository categoryRepository,
        IMemoryBus memoryBus) : base(memoryBus)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
    }
    
    public async Task<Guid?> Handle(ProductAddCommand request, CancellationToken cancellationToken)
    {
        var product = new ProductDomain(Guid.NewGuid(), request.Name, request.Description, request.Price.Value,
            request.Stock.Value, request.CategoryId.Value);

        if (!product.IsValid())
        {
            NotifyValidationErrors(product.ValidationResult);

            return null;
        }

        var category = await _categoryRepository.GetCategoryByIdAsync(product.CategoryId);
        if (category == null)
        {
            NotifyValidationErrors("Category does not exist.");

            return null;
        }

        try
        {
            await _productRepository.BeginTransactionAsync();
            await _productRepository.AddProductAsync(product);
            await _productRepository.CommitTransactionAsync();

            return product.Id;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            NotifyValidationErrors("A fatal error occurred. The operation could not be completed.");
            await _productRepository.RollBackTransactionAsync();
            return null;
        }
    }

    public async Task<Unit> Handle(ProductUpdateCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetProductByIdAsync(request.Id);

        if (product == null)
        {
            NotifyValidationErrors($"Product does not exist. Id: {request.Id}");
            return Unit.Value;
        }

        await UpdateProductProperties(request, product);

        if (HasValidationErrors())
            return Unit.Value;

        if (!product.IsValid())
        {
            NotifyValidationErrors(product.ValidationResult);
            return Unit.Value;
        }

        try
        {
            await _productRepository.BeginTransactionAsync();
            await _productRepository.UpdateProductAsync(product);
            await _productRepository.CommitTransactionAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            NotifyValidationErrors("A fatal error occurred. The operation could not be completed.");
            await _productRepository.RollBackTransactionAsync();
        }
        
        return Unit.Value;
    }

    public async Task<Unit> Handle(ProductRemoveCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetProductByIdAsync(request.Id);
        
        if (product == null)
        {
            NotifyValidationErrors($"Product does not exist. Id: {request.Id}");
            return Unit.Value;
        }

        try
        {
            await _productRepository.BeginTransactionAsync();
            await _productRepository.RemoveProductAsync(product);
            await _productRepository.CommitTransactionAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            NotifyValidationErrors("A fatal error occurred. The operation could not be completed.");
            await _productRepository.RollBackTransactionAsync();
        }
        
        return Unit.Value;
    }

    private async Task UpdateProductProperties(ProductUpdateCommand request, ProductDomain product)
    {
        if (request.CategoryId != null)
        {
            var category = await _categoryRepository.GetCategoryByIdAsync(request.CategoryId.Value);

            if (category == null)
            {
                NotifyValidationErrors($"Category does not exist. Id: {request.CategoryId}");
                return;
            }
            
            product.SetCategoryId(request.CategoryId.Value);
        }
        
        if (!string.IsNullOrWhiteSpace(request.Name))
            product.SetName(request.Name);
        if (!string.IsNullOrWhiteSpace(request.Description))
            product.SetDescription(request.Description);
        if (request.Price != null)
            product.SetPrice(request.Price.Value);
    }
}