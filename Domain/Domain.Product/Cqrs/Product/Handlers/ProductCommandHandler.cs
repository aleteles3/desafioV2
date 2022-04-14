using Application.Product.Cqrs.Product.Commands;
using Domain.Product.Interfaces;
using MediatR;
using ProductDomain = Domain.Product.Entities.Product;

namespace Application.Product.Cqrs.Product.Handlers;

public class ProductCommandHandler : 
    IRequestHandler<ProductAddCommand, Guid?>, 
    IRequestHandler<ProductUpdateCommand>, 
    IRequestHandler<ProductRemoveCommand>
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;

    public ProductCommandHandler(IProductRepository productRepository, ICategoryRepository categoryRepository)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
    }
    
    public async Task<Guid?> Handle(ProductAddCommand request, CancellationToken cancellationToken)
    {
        var product = new ProductDomain(Guid.NewGuid(), request.Name, request.Description, request.Price,
            request.Stock, request.CategoryId);

        if (!product.IsValid())
        {
            //ToDo Create memory Bus to store the errors
            foreach (var error in product.ValidationResult.Errors)
                Console.WriteLine(error);

            return null;
        }

        var category = await _categoryRepository.GetCategoryByIdAsync(product.CategoryId);
        if (category == null)
        {
            Console.WriteLine("Category does not exist.");

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
            
            await _categoryRepository.RollBackTransactionAsync();
            
            return null;
        }
    }

    public async Task<Unit> Handle(ProductUpdateCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<Unit> Handle(ProductRemoveCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}