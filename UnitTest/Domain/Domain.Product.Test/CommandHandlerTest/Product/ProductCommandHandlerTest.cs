using System;
using System.Threading;
using System.Threading.Tasks;
using Domain.Core.Enums;
using Domain.Core.Interfaces;
using Domain.Product.Cqrs.Product.Handlers;
using Domain.Product.Interfaces;
using Moq;
using Xunit;
using ProductDomain = Domain.Product.Entities.Product;

namespace Domain.Product.Test.CommandHandlerTest.Product;

[Collection(nameof(ProductCommandHandlerCollection))]
public class ProductCommandHandlerTest
{
    private readonly ProductCommandHandlerFixture _fixture;
    private readonly ProductCommandHandler _commandHandler;
    private readonly Factory _factory;

    public ProductCommandHandlerTest(ProductCommandHandlerFixture fixture)
    {
        _fixture = fixture;
        _commandHandler = _fixture.GetProductCommandHandler();
        _factory = new Factory();
    }

    [Fact]
    [Trait("ProductAddCommand", "Success")]
    public async Task ProductAddCommand_Success()
    {
        //Arrange
        var command = _factory.GenerateProductAddCommand();
        var categories = new[]
        {
            _factory.GenerateCategory(id: command.CategoryId),
            _factory.GenerateCategory()
        };
        
        //Setup
        _fixture.SetupGetCategoryByIdAsync(categories);
        
        //Act
        var result = await _commandHandler.Handle(command, CancellationToken.None);
        
        //Assert
        _fixture.Mocker.GetMock<ICategoryRepository>()
            .Verify(r => r.GetCategoryByIdAsync(It.Is<Guid>(x => x == command.CategoryId)), Times.Once);
        _fixture.Mocker
            .GetMock<IProductRepository>()
            .Verify(r => r.BeginTransactionAsync(), Times.Once);
        _fixture.Mocker
            .GetMock<IProductRepository>()
            .Verify(r => r.AddProductAsync(It.Is<ProductDomain>(x =>
                x.Name == command.Name &&
                x.Description == command.Description &&
                x.ListPrice == command.Price &&
                x.Stock == command.Stock &&
                x.CategoryId == command.CategoryId)), Times.Once);
        _fixture.Mocker
            .GetMock<IProductRepository>()
            .Verify(r => r.CommitTransactionAsync(), Times.Once);
        
        Assert.NotNull(result);
        Assert.IsAssignableFrom<Guid?>(result);
    }
    
    [Fact]
    [Trait("ProductAddCommand", "Invalid Product - Failure")]
    public async Task ProductAddCommand_InvalidProduct_Failure()
    {
        //Arrange
        var command = _factory.GenerateProductAddCommand(name: string.Empty, description: string.Empty,
            price: 0, stock: -1, categoryId: Guid.Empty);

        //Setup

        //Act
        var result = await _commandHandler.Handle(command, CancellationToken.None);
        
        //Assert
        _fixture.Mocker
            .GetMock<IMemoryBus>()
            .Verify(r => r.RaiseValidationError(It.Is<ErrorCode>(x => x == ErrorCode.DomainValidationError),
                It.Is<string>(x => x == "Product name must be informed.")), Times.Once);
        _fixture.Mocker
            .GetMock<IMemoryBus>()
            .Verify(r => r.RaiseValidationError(It.Is<ErrorCode>(x => x == ErrorCode.DomainValidationError),
                It.Is<string>(x => x == "Product name must be, at least, 3 characters long.")), Times.Once);
        _fixture.Mocker
            .GetMock<IMemoryBus>()
            .Verify(r => r.RaiseValidationError(It.Is<ErrorCode>(x => x == ErrorCode.DomainValidationError),
                It.Is<string>(x => x == "Product description must be informed.")), Times.Once);
        _fixture.Mocker
            .GetMock<IMemoryBus>()
            .Verify(r => r.RaiseValidationError(It.Is<ErrorCode>(x => x == ErrorCode.DomainValidationError),
                It.Is<string>(x => x == "Product listPrice must be greater than 0.")), Times.Once);
        _fixture.Mocker
            .GetMock<IMemoryBus>()
            .Verify(r => r.RaiseValidationError(It.Is<ErrorCode>(x => x == ErrorCode.DomainValidationError),
                It.Is<string>(x => x == "Product stock cannot be less than 0.")), Times.Once);
        _fixture.Mocker
            .GetMock<IMemoryBus>()
            .Verify(r => r.RaiseValidationError(It.Is<ErrorCode>(x => x == ErrorCode.DomainValidationError),
                It.Is<string>(x => x == "Category Id must be informed.")), Times.Once);
        
        Assert.Null(result);

        _fixture.Mocker.GetMock<ICategoryRepository>()
            .Verify(r => r.GetCategoryByIdAsync(It.Is<Guid>(x => x == command.CategoryId)), Times.Never);
        _fixture.Mocker
            .GetMock<IProductRepository>()
            .Verify(r => r.BeginTransactionAsync(), Times.Never);
        _fixture.Mocker
            .GetMock<IProductRepository>()
            .Verify(r => r.AddProductAsync(It.Is<ProductDomain>(x =>
                x.Name == command.Name &&
                x.Description == command.Description &&
                x.ListPrice == command.Price &&
                x.Stock == command.Stock &&
                x.CategoryId == command.CategoryId)), Times.Never);
        _fixture.Mocker
            .GetMock<IProductRepository>()
            .Verify(r => r.CommitTransactionAsync(), Times.Never);
    }
    
    [Fact]
    [Trait("ProductAddCommand", "Category Does Not Exist - Failure")]
    public async Task ProductAddCommand_CategoryDoesNotExist_Failure()
    {
        //Arrange
        var command = _factory.GenerateProductAddCommand();
        var categories = new[]
        {
            _factory.GenerateCategory(),
            _factory.GenerateCategory()
        };
        
        //Setup
        _fixture.SetupGetCategoryByIdAsync(categories);
        
        //Act
        var result = await _commandHandler.Handle(command, CancellationToken.None);
        
        //Assert
        _fixture.Mocker.GetMock<ICategoryRepository>()
            .Verify(r => r.GetCategoryByIdAsync(It.Is<Guid>(x => x == command.CategoryId)), Times.Once);
        _fixture.Mocker
            .GetMock<IMemoryBus>()
            .Verify(r => r.RaiseValidationError(It.Is<ErrorCode>(x => x == ErrorCode.DomainValidationError),
                It.Is<string>(x => x == "Category does not exist.")), Times.Once);

        Assert.Null(result);
        
        _fixture.Mocker
            .GetMock<IProductRepository>()
            .Verify(r => r.BeginTransactionAsync(), Times.Never);
        _fixture.Mocker
            .GetMock<IProductRepository>()
            .Verify(r => r.AddProductAsync(It.Is<ProductDomain>(x =>
                x.Name == command.Name &&
                x.Description == command.Description &&
                x.ListPrice == command.Price &&
                x.Stock == command.Stock &&
                x.CategoryId == command.CategoryId)), Times.Never);
        _fixture.Mocker
            .GetMock<IProductRepository>()
            .Verify(r => r.CommitTransactionAsync(), Times.Never);
    }
    
    [Fact]
    [Trait("ProductAddCommand", "Database Exception - Failure")]
    public async Task ProductAddCommand_DatabaseException_Failure()
    {
        //Arrange
        var command = _factory.GenerateProductAddCommand();
        var categories = new[]
        {
            _factory.GenerateCategory(id: command.CategoryId),
            _factory.GenerateCategory()
        };
        
        //Setup
        _fixture.SetupGetCategoryByIdAsync(categories);
        _fixture.SetupCommitTransactionAsyncException();
        
        //Act
        var result = await _commandHandler.Handle(command, CancellationToken.None);
        
        //Assert
        _fixture.Mocker.GetMock<ICategoryRepository>()
            .Verify(r => r.GetCategoryByIdAsync(It.Is<Guid>(x => x == command.CategoryId)), Times.Once);
        _fixture.Mocker
            .GetMock<IProductRepository>()
            .Verify(r => r.BeginTransactionAsync(), Times.Once);
        _fixture.Mocker
            .GetMock<IProductRepository>()
            .Verify(r => r.AddProductAsync(It.Is<ProductDomain>(x =>
                x.Name == command.Name &&
                x.Description == command.Description &&
                x.ListPrice == command.Price &&
                x.Stock == command.Stock &&
                x.CategoryId == command.CategoryId)), Times.Once);
        _fixture.Mocker
            .GetMock<IProductRepository>()
            .Verify(r => r.CommitTransactionAsync(), Times.Once);
        _fixture.Mocker
            .GetMock<IMemoryBus>()
            .Verify(r => r.RaiseValidationError(It.Is<ErrorCode>(x => x == ErrorCode.DomainValidationError),
                It.Is<string>(x => x == "A fatal error occurred. The operation could not be completed.")), Times.Once);
        _fixture.Mocker
            .GetMock<IProductRepository>()
            .Verify(r => r.RollBackTransactionAsync(), Times.Once);

        Assert.Null(result);
    }

    [Fact]
    [Trait("ProductUpdateCommand", "Success")]
    public async Task ProductUpdateCommand_Success()
    {
        //Arrange
        var command = _factory.GenerateProductUpdateCommand();
        var existingProducts = new[]
        {
            _factory.GenerateProduct(),
            _factory.GenerateProduct(id: command.Id)
        };
        var existingCategories = new[]
        {
            _factory.GenerateCategory(id: command.CategoryId),
            _factory.GenerateCategory()
        };
        
        //Setup
        _fixture.SetupGetProductByIdAsync(existingProducts);
        _fixture.SetupGetCategoryByIdAsync(existingCategories);
        
        //Act
        await _commandHandler.Handle(command, CancellationToken.None);
        
        //Assert
        _fixture.Mocker.GetMock<IProductRepository>()
            .Verify(r => r.GetProductByIdAsync(It.Is<Guid>(x => x == command.Id)), Times.Once);
        _fixture.Mocker.GetMock<ICategoryRepository>()
            .Verify(r => r.GetCategoryByIdAsync(It.Is<Guid>(x => x == command.CategoryId)), Times.Once);
        _fixture.Mocker.GetMock<IMemoryBus>()
            .Verify(r => r.HasValidationErrors(), Times.Once);
        _fixture.Mocker
            .GetMock<IProductRepository>()
            .Verify(r => r.BeginTransactionAsync(), Times.Once);
        _fixture.Mocker
            .GetMock<IProductRepository>()
            .Verify(r => r.UpdateProductAsync(It.Is<ProductDomain>(x =>
                x.Id == command.Id &&
                x.Name == command.Name &&
                x.Description == command.Description &&
                x.ListPrice == command.Price &&
                x.Stock == command.Stock &&
                x.CategoryId == command.CategoryId)), Times.Once);
        _fixture.Mocker
            .GetMock<IProductRepository>()
            .Verify(r => r.CommitTransactionAsync(), Times.Once);
    }
    
    [Fact]
    [Trait("ProductUpdateCommand", "Product Does Not Exist - Failure")]
    public async Task ProductUpdateCommand_ProductDoesNotExist_Failure()
    {
        //Arrange
        var command = _factory.GenerateProductUpdateCommand();
        var existingProducts = new[]
        {
            _factory.GenerateProduct(),
            _factory.GenerateProduct()
        };

        //Setup
        _fixture.SetupGetProductByIdAsync(existingProducts);

        //Act
        await _commandHandler.Handle(command, CancellationToken.None);
        
        //Assert
        _fixture.Mocker.GetMock<IProductRepository>()
            .Verify(r => r.GetProductByIdAsync(It.Is<Guid>(x => x == command.Id)), Times.Once);
        _fixture.Mocker
            .GetMock<IMemoryBus>()
            .Verify(r => r.RaiseValidationError(It.Is<ErrorCode>(x => x == ErrorCode.DomainValidationError),
                It.Is<string>(x => x == $"Product does not exist. Id: {command.Id}")), Times.Once);

        _fixture.Mocker.GetMock<ICategoryRepository>()
            .Verify(r => r.GetCategoryByIdAsync(It.Is<Guid>(x => x == command.CategoryId)), Times.Never);
        _fixture.Mocker
            .GetMock<IProductRepository>()
            .Verify(r => r.BeginTransactionAsync(), Times.Never);
        _fixture.Mocker
            .GetMock<IProductRepository>()
            .Verify(r => r.UpdateProductAsync(It.Is<ProductDomain>(x =>
                x.Id == command.Id &&
                x.Name == command.Name &&
                x.Description == command.Description &&
                x.ListPrice == command.Price &&
                x.Stock == command.Stock &&
                x.CategoryId == command.CategoryId)), Times.Never);
        _fixture.Mocker
            .GetMock<IProductRepository>()
            .Verify(r => r.CommitTransactionAsync(), Times.Never);
    }
    
    [Fact]
    [Trait("ProductUpdateCommand", "Category Does Not Exist - Failure")]
    public async Task ProductUpdateCommand_CategoryDoesNotExist_Failure()
    {
        //Arrange
        var command = _factory.GenerateProductUpdateCommand();
        var existingProducts = new[]
        {
            _factory.GenerateProduct(),
            _factory.GenerateProduct(id: command.Id)
        };
        var existingCategories = new[]
        {
            _factory.GenerateCategory(),
            _factory.GenerateCategory()
        };
        
        //Setup
        _fixture.SetupGetProductByIdAsync(existingProducts);
        _fixture.SetupGetCategoryByIdAsync(existingCategories);
        _fixture.SetupHasValidationErrors();
        
        //Act
        await _commandHandler.Handle(command, CancellationToken.None);
        
        //Assert
        _fixture.Mocker.GetMock<IProductRepository>()
            .Verify(r => r.GetProductByIdAsync(It.Is<Guid>(x => x == command.Id)), Times.Once);
        _fixture.Mocker.GetMock<ICategoryRepository>()
            .Verify(r => r.GetCategoryByIdAsync(It.Is<Guid>(x => x == command.CategoryId)), Times.Once);
        _fixture.Mocker.GetMock<IMemoryBus>()
            .Verify(r => r.RaiseValidationError(It.Is<ErrorCode>(x => x == ErrorCode.DomainValidationError),
                It.Is<string>(x => x == $"Category does not exist. Id: {command.CategoryId}")), Times.Once);
        _fixture.Mocker.GetMock<IMemoryBus>()
            .Verify(r => r.HasValidationErrors(), Times.Once);
        
        _fixture.Mocker
            .GetMock<IProductRepository>()
            .Verify(r => r.BeginTransactionAsync(), Times.Never);
        _fixture.Mocker
            .GetMock<IProductRepository>()
            .Verify(r => r.UpdateProductAsync(It.Is<ProductDomain>(x =>
                x.Id == command.Id &&
                x.Name == command.Name &&
                x.Description == command.Description &&
                x.ListPrice == command.Price &&
                x.Stock == command.Stock &&
                x.CategoryId == command.CategoryId)), Times.Never);
        _fixture.Mocker
            .GetMock<IProductRepository>()
            .Verify(r => r.CommitTransactionAsync(), Times.Never);
    }
    
    [Fact]
    [Trait("ProductUpdateCommand", "Invalid Product - Failure")]
    public async Task ProductUpdateCommand_InvalidProduct_Failure()
    {
        //Arrange
        var command = _factory.GenerateProductUpdateCommand(name: "oh", price: 0);
        var existingProducts = new[]
        {
            _factory.GenerateProduct(),
            _factory.GenerateProduct(id: command.Id)
        };
        var existingCategories = new[]
        {
            _factory.GenerateCategory(id: command.CategoryId),
            _factory.GenerateCategory()
        };
        
        //Setup
        _fixture.SetupGetProductByIdAsync(existingProducts);
        _fixture.SetupGetCategoryByIdAsync(existingCategories);
        
        //Act
        await _commandHandler.Handle(command, CancellationToken.None);
        
        //Assert
        _fixture.Mocker.GetMock<IProductRepository>()
            .Verify(r => r.GetProductByIdAsync(It.Is<Guid>(x => x == command.Id)), Times.Once);
        _fixture.Mocker.GetMock<ICategoryRepository>()
            .Verify(r => r.GetCategoryByIdAsync(It.Is<Guid>(x => x == command.CategoryId)), Times.Once);
        _fixture.Mocker.GetMock<IMemoryBus>()
            .Verify(r => r.HasValidationErrors(), Times.Once);
        
        _fixture.Mocker
            .GetMock<IMemoryBus>()
            .Verify(r => r.RaiseValidationError(It.Is<ErrorCode>(x => x == ErrorCode.DomainValidationError),
                It.Is<string>(x => x == "Product name must be, at least, 3 characters long.")), Times.Once);
        _fixture.Mocker
            .GetMock<IMemoryBus>()
            .Verify(r => r.RaiseValidationError(It.Is<ErrorCode>(x => x == ErrorCode.DomainValidationError),
                It.Is<string>(x => x == "Product listPrice must be greater than 0.")), Times.Once);

        _fixture.Mocker
            .GetMock<IProductRepository>()
            .Verify(r => r.BeginTransactionAsync(), Times.Never);
        _fixture.Mocker
            .GetMock<IProductRepository>()
            .Verify(r => r.UpdateProductAsync(It.Is<ProductDomain>(x =>
                x.Id == command.Id &&
                x.Name == command.Name &&
                x.Description == command.Description &&
                x.ListPrice == command.Price &&
                x.Stock == command.Stock &&
                x.CategoryId == command.CategoryId)), Times.Never);
        _fixture.Mocker
            .GetMock<IProductRepository>()
            .Verify(r => r.CommitTransactionAsync(), Times.Never);
    }
    
    [Fact]
    [Trait("ProductUpdateCommand", "Database Exception - Failure")]
    public async Task ProductUpdateCommand_DatabaseException_Failure()
    {
        //Arrange
        var command = _factory.GenerateProductUpdateCommand();
        var existingProducts = new[]
        {
            _factory.GenerateProduct(),
            _factory.GenerateProduct(id: command.Id)
        };
        var existingCategories = new[]
        {
            _factory.GenerateCategory(id: command.CategoryId),
            _factory.GenerateCategory()
        };
        
        //Setup
        _fixture.SetupGetProductByIdAsync(existingProducts);
        _fixture.SetupGetCategoryByIdAsync(existingCategories);
        _fixture.SetupCommitTransactionAsyncException();
        
        //Act
        await _commandHandler.Handle(command, CancellationToken.None);
        
        //Assert
        _fixture.Mocker.GetMock<IProductRepository>()
            .Verify(r => r.GetProductByIdAsync(It.Is<Guid>(x => x == command.Id)), Times.Once);
        _fixture.Mocker.GetMock<ICategoryRepository>()
            .Verify(r => r.GetCategoryByIdAsync(It.Is<Guid>(x => x == command.CategoryId)), Times.Once);
        _fixture.Mocker.GetMock<IMemoryBus>()
            .Verify(r => r.HasValidationErrors(), Times.Once);
        _fixture.Mocker
            .GetMock<IProductRepository>()
            .Verify(r => r.BeginTransactionAsync(), Times.Once);
        _fixture.Mocker
            .GetMock<IProductRepository>()
            .Verify(r => r.UpdateProductAsync(It.Is<ProductDomain>(x =>
                x.Id == command.Id &&
                x.Name == command.Name &&
                x.Description == command.Description &&
                x.ListPrice == command.Price &&
                x.Stock == command.Stock &&
                x.CategoryId == command.CategoryId)), Times.Once);
        _fixture.Mocker
            .GetMock<IProductRepository>()
            .Verify(r => r.CommitTransactionAsync(), Times.Once);
        _fixture.Mocker
            .GetMock<IMemoryBus>()
            .Verify(r => r.RaiseValidationError(It.Is<ErrorCode>(x => x == ErrorCode.DomainValidationError),
                It.Is<string>(x => x == "A fatal error occurred. The operation could not be completed.")), Times.Once);
        _fixture.Mocker
            .GetMock<IProductRepository>()
            .Verify(r => r.RollBackTransactionAsync(), Times.Once);
    }

    [Fact]
    [Trait("ProductRemoveCommand", "Success")]
    public async Task ProductRemoveCommand_Success()
    {
        //Arrange
        var command = _factory.GenerateProductRemoveCommand();
        var existingProducts = new[]
        {
            _factory.GenerateProduct(id: command.Id),
            _factory.GenerateProduct()
        };
        
        //Setup
        _fixture.SetupGetProductByIdAsync(existingProducts);
        
        //Act
        await _commandHandler.Handle(command, CancellationToken.None);
        
        //Assert
        _fixture.Mocker.GetMock<IProductRepository>()
            .Verify(r => r.GetProductByIdAsync(It.Is<Guid>(x => x == command.Id)), Times.Once);
        _fixture.Mocker
            .GetMock<IProductRepository>()
            .Verify(r => r.BeginTransactionAsync(), Times.Once);
        _fixture.Mocker
            .GetMock<IProductRepository>()
            .Verify(r => r.RemoveProductAsync(It.Is<ProductDomain>(x => x.Id == command.Id)), Times.Once);
        _fixture.Mocker
            .GetMock<IProductRepository>()
            .Verify(r => r.CommitTransactionAsync(), Times.Once);
    }
    
    [Fact]
    [Trait("ProductRemoveCommand", "Product Doest Not Exist - Failure")]
    public async Task ProductRemoveCommand_ProductDoesNotExist_Failure()
    {
        //Arrange
        var command = _factory.GenerateProductRemoveCommand();
        var existingProducts = new[]
        {
            _factory.GenerateProduct(),
            _factory.GenerateProduct()
        };
        
        //Setup
        _fixture.SetupGetProductByIdAsync(existingProducts);
        
        //Act
        await _commandHandler.Handle(command, CancellationToken.None);
        
        //Assert
        _fixture.Mocker.GetMock<IProductRepository>()
            .Verify(r => r.GetProductByIdAsync(It.Is<Guid>(x => x == command.Id)), Times.Once);
        _fixture.Mocker
            .GetMock<IMemoryBus>()
            .Verify(r => r.RaiseValidationError(It.Is<ErrorCode>(x => x == ErrorCode.DomainValidationError),
                It.Is<string>(x => x == $"Product does not exist. Id: {command.Id}")), Times.Once);
        
        _fixture.Mocker
            .GetMock<IProductRepository>()
            .Verify(r => r.BeginTransactionAsync(), Times.Never);
        _fixture.Mocker
            .GetMock<IProductRepository>()
            .Verify(r => r.RemoveProductAsync(It.Is<ProductDomain>(x => x.Id == command.Id)), Times.Never);
        _fixture.Mocker
            .GetMock<IProductRepository>()
            .Verify(r => r.CommitTransactionAsync(), Times.Never);
    }
    
    [Fact]
    [Trait("ProductRemoveCommand", "Database Exception - Failure")]
    public async Task ProductRemoveCommand_DatabaseException_Failure()
    {
        //Arrange
        var command = _factory.GenerateProductRemoveCommand();
        var existingProducts = new[]
        {
            _factory.GenerateProduct(id: command.Id),
            _factory.GenerateProduct()
        };
        
        //Setup
        _fixture.SetupGetProductByIdAsync(existingProducts);
        _fixture.SetupCommitTransactionAsyncException();
        
        //Act
        await _commandHandler.Handle(command, CancellationToken.None);
        
        //Assert
        _fixture.Mocker.GetMock<IProductRepository>()
            .Verify(r => r.GetProductByIdAsync(It.Is<Guid>(x => x == command.Id)), Times.Once);
        _fixture.Mocker
            .GetMock<IProductRepository>()
            .Verify(r => r.BeginTransactionAsync(), Times.Once);
        _fixture.Mocker
            .GetMock<IProductRepository>()
            .Verify(r => r.RemoveProductAsync(It.Is<ProductDomain>(x => x.Id == command.Id)), Times.Once);
        _fixture.Mocker
            .GetMock<IProductRepository>()
            .Verify(r => r.CommitTransactionAsync(), Times.Once);
        _fixture.Mocker
            .GetMock<IMemoryBus>()
            .Verify(r => r.RaiseValidationError(It.Is<ErrorCode>(x => x == ErrorCode.DomainValidationError),
                It.Is<string>(x => x == "A fatal error occurred. The operation could not be completed.")), Times.Once);
        _fixture.Mocker
            .GetMock<IProductRepository>()
            .Verify(r => r.RollBackTransactionAsync(), Times.Once);
    }
}