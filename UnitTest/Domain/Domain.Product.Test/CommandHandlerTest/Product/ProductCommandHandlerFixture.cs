using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Core.Interfaces;
using Domain.Product.Cqrs.Product.Handlers;
using Domain.Product.Interfaces;
using Moq;
using Moq.AutoMock;
using Xunit;
using CategoryDomain = Domain.Product.Entities.Category;
using ProductDomain = Domain.Product.Entities.Product;

namespace Domain.Product.Test.CommandHandlerTest.Product;

[CollectionDefinition(nameof(ProductCommandHandlerCollection))]
public class ProductCommandHandlerCollection : ICollectionFixture<ProductCommandHandlerFixture>{ }
public class ProductCommandHandlerFixture
{
    private ProductCommandHandler ProductCommandHandler { get; set; }
    public AutoMocker Mocker { get; set; }

    public ProductCommandHandler GetProductCommandHandler()
    {
        Mocker = new AutoMocker();
        
        ProductCommandHandler = Mocker.CreateInstance<ProductCommandHandler>();

        return ProductCommandHandler;
    }
    
    public void SetupGetCategoryByIdAsync(IEnumerable<CategoryDomain> categories)
    {
        Mocker
            .GetMock<ICategoryRepository>()
            .Setup(s => s.GetCategoryByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid id) => categories.FirstOrDefault(x => x.Id == id));
    }

    public void SetupGetProductByIdAsync(IEnumerable<ProductDomain> products)
    {
        Mocker
            .GetMock<IProductRepository>()
            .Setup(s => s.GetProductByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid id) => products.FirstOrDefault(x => x.Id == id));
    }
    
    public void SetupCommitTransactionAsyncException()
    {
        Mocker
            .GetMock<IProductRepository>()
            .Setup(s => s.CommitTransactionAsync())
            .Callback(() => throw new Exception());
    }

    public void SetupHasValidationErrors(bool has = true)
    {
        Mocker
            .GetMock<IMemoryBus>()
            .Setup(s => s.HasValidationErrors())
            .Returns(has);
    }
}