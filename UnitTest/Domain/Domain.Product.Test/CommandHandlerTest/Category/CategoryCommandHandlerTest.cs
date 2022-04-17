using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Domain.Core.Enums;
using Domain.Core.Interfaces;
using Domain.Product.Cqrs.Category.Handlers;
using Domain.Product.Interfaces;
using Moq;
using Xunit;
using CategoryDomain = Domain.Product.Entities.Category;

namespace Domain.Product.Test.CommandHandlerTest.Category;

[Collection(nameof(CategoryCommandHandlerCollection))]
public class CategoryCommandHandlerTest
{
    private readonly CategoryCommandHandler _commandHandler;
    private readonly Factory _factory;
    private readonly CategoryCommandHandlerFixture _fixture;

    public CategoryCommandHandlerTest(CategoryCommandHandlerFixture fixture)
    {
        _fixture = fixture;
        _commandHandler = fixture.GetCategoryCommandHandler();
        _factory = new Factory();
    }

    [Fact]
    [Trait("CategoryAddCommand", "Success")]
    public async Task CategoryAddCommand_Success()
    {
        //Arrange
        var command = _factory.GenerateCategoryAddCommand("Name");
        var existingCategories = new[]
        {
            _factory.GenerateCategory(name: "NotTheSame"),
            _factory.GenerateCategory(name: "NotTheSameEither"),
        };
        
        //Setup
        _fixture.SetupGetCategoriesAsync(existingCategories);
        
        //Act
        var result = await _commandHandler.Handle(command, CancellationToken.None);
        
        //Assert
        _fixture.Mocker
            .GetMock<ICategoryRepository>()
            .Verify(r => r.GetCategoriesAsync(It.IsAny<Expression<Func<CategoryDomain, bool>>>(),
                It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<Expression<Func<CategoryDomain, object>>[]>()),
                Times.Once);
        _fixture.Mocker
            .GetMock<ICategoryRepository>()
            .Verify(r => r.BeginTransactionAsync(), Times.Once);
        _fixture.Mocker
            .GetMock<ICategoryRepository>()
            .Verify(r => r.AddCategoryAsync(It.Is<CategoryDomain>(x =>
                x.Name == command.Name)), Times.Once);
        _fixture.Mocker
            .GetMock<ICategoryRepository>()
            .Verify(r => r.CommitTransactionAsync(), Times.Once);
        
        Assert.NotNull(result);
        Assert.IsAssignableFrom<Guid?>(result);
    }
    
    [Fact]
    [Trait("CategoryAddCommand", "Invalid Entity - Failure")]
    public async Task CategoryAddCommand_InvalidEntity_Failure()
    {
        //Arrange
        var command = _factory.GenerateCategoryAddCommand("");

        //Setup

        //Act
        var result = await _commandHandler.Handle(command, CancellationToken.None);
        
        //Assert
        _fixture.Mocker
            .GetMock<IMemoryBus>()
            .Verify(r => r.RaiseValidationError(It.Is<ErrorCode>(x => x == ErrorCode.DomainValidationError),
                It.Is<string>(x => x == "Category name must be informed.")), Times.Once);
        _fixture.Mocker
            .GetMock<IMemoryBus>()
            .Verify(r => r.RaiseValidationError(It.Is<ErrorCode>(x => x == ErrorCode.DomainValidationError),
                It.Is<string>(x => x == "Category name must be, at least, 3 characters long.")), Times.Once);
        
        Assert.Null(result);
        
        _fixture.Mocker
            .GetMock<ICategoryRepository>()
            .Verify(r => r.GetCategoriesAsync(It.IsAny<Expression<Func<CategoryDomain, bool>>>(),
                    It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<Expression<Func<CategoryDomain, object>>[]>()),
                Times.Never);
        _fixture.Mocker
            .GetMock<ICategoryRepository>()
            .Verify(r => r.BeginTransactionAsync(), Times.Never);
        _fixture.Mocker
            .GetMock<ICategoryRepository>()
            .Verify(r => r.AddCategoryAsync(It.Is<CategoryDomain>(x =>
                x.Name == command.Name)), Times.Never);
        _fixture.Mocker
            .GetMock<ICategoryRepository>()
            .Verify(r => r.CommitTransactionAsync(), Times.Never);
    }
    
    [Fact]
    [Trait("CategoryAddCommand", "Category Already Exists - Failure")]
    public async Task CategoryAddCommand_CategoryAlreadyExists_Failure()
    {
        //Arrange
        var command = _factory.GenerateCategoryAddCommand("Name");
        var existingCategories = new[]
        {
            _factory.GenerateCategory(name: "NotTheSame"),
            _factory.GenerateCategory(name: command.Name),
        };
        
        //Setup
        _fixture.SetupGetCategoriesAsync(existingCategories);
        
        //Act
        var result = await _commandHandler.Handle(command, CancellationToken.None);
        
        //Assert
        _fixture.Mocker
            .GetMock<ICategoryRepository>()
            .Verify(r => r.GetCategoriesAsync(It.IsAny<Expression<Func<CategoryDomain, bool>>>(),
                    It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<Expression<Func<CategoryDomain, object>>[]>()),
                Times.Once);
        _fixture.Mocker
            .GetMock<IMemoryBus>()
            .Verify(r => r.RaiseValidationError(It.Is<ErrorCode>(x => x == ErrorCode.DomainValidationError),
                It.Is<string>(x => x == "Category with the same name already exists.")), Times.Once);
        
        Assert.Null(result);
        
        _fixture.Mocker
            .GetMock<ICategoryRepository>()
            .Verify(r => r.BeginTransactionAsync(), Times.Never);
        _fixture.Mocker
            .GetMock<ICategoryRepository>()
            .Verify(r => r.AddCategoryAsync(It.Is<CategoryDomain>(x =>
                x.Name == command.Name)), Times.Never);
        _fixture.Mocker
            .GetMock<ICategoryRepository>()
            .Verify(r => r.CommitTransactionAsync(), Times.Never);
    }
    
    [Fact]
    [Trait("CategoryAddCommand", "Database Exception - Failure")]
    public async Task CategoryAddCommand_DatabaseException_Failure()
    {
        //Arrange
        var command = _factory.GenerateCategoryAddCommand("Name");
        var existingCategories = new[]
        {
            _factory.GenerateCategory(name: "NotTheSame"),
            _factory.GenerateCategory(name: "NotTheSameEither"),
        };
        
        //Setup
        _fixture.SetupGetCategoriesAsync(existingCategories);
        _fixture.SetupCommitTransactionAsyncException();
        
        //Act
        var result = await _commandHandler.Handle(command, CancellationToken.None);
        
        //Assert
        _fixture.Mocker
            .GetMock<ICategoryRepository>()
            .Verify(r => r.GetCategoriesAsync(It.IsAny<Expression<Func<CategoryDomain, bool>>>(),
                    It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<Expression<Func<CategoryDomain, object>>[]>()),
                Times.Once);
        _fixture.Mocker
            .GetMock<ICategoryRepository>()
            .Verify(r => r.BeginTransactionAsync(), Times.Once);
        _fixture.Mocker
            .GetMock<ICategoryRepository>()
            .Verify(r => r.AddCategoryAsync(It.Is<CategoryDomain>(x =>
                x.Name == command.Name)), Times.Once);
        _fixture.Mocker
            .GetMock<ICategoryRepository>()
            .Verify(r => r.CommitTransactionAsync(), Times.Once);
        _fixture.Mocker
            .GetMock<IMemoryBus>()
            .Verify(r => r.RaiseValidationError(It.Is<ErrorCode>(x => x == ErrorCode.DomainValidationError),
                It.Is<string>(x => x == "A fatal error occurred. The operation could not be completed.")), Times.Once);
        _fixture.Mocker
            .GetMock<ICategoryRepository>()
            .Verify(r => r.RollBackTransactionAsync(), Times.Once);

        Assert.Null(result);
    }

    [Fact]
    [Trait("CategoryUpdateCommand", "Success")]
    public async Task CategoryUpdateCommand_Success()
    {
        //Arrange
        var command = _factory.GenerateCategoryUpdateCommand();
        var existingCategories = new[]
        {
            _factory.GenerateCategory(name: "NotTheSame"),
            _factory.GenerateCategory(name: "NotTheSameEither"),
            _factory.GenerateCategory(id: command.Id, name: "ToBeUpdated")
        };
        
        //Setup
        _fixture.SetupGetCategoriesAsync(existingCategories);
        
        //Act
        await _commandHandler.Handle(command, CancellationToken.None);
        
        //Assert
        _fixture.Mocker
            .GetMock<ICategoryRepository>()
            .Verify(r => r.GetCategoriesAsync(It.IsAny<Expression<Func<CategoryDomain, bool>>>(),
                    It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<Expression<Func<CategoryDomain, object>>[]>()),
                Times.Once);
        _fixture.Mocker
            .GetMock<ICategoryRepository>()
            .Verify(r => r.BeginTransactionAsync(), Times.Once);
        _fixture.Mocker
            .GetMock<ICategoryRepository>()
            .Verify(r => r.UpdateCategoryAsync(It.Is<CategoryDomain>(x =>
                x.Id == command.Id &&
                x.Name == command.Name)), Times.Once);
        _fixture.Mocker
            .GetMock<ICategoryRepository>()
            .Verify(r => r.CommitTransactionAsync(), Times.Once);
    }
    
    [Fact]
    [Trait("CategoryUpdateCommand", "Category does not exist - Failure")]
    public async Task CategoryUpdateCommand_CategoryDoesNotExist_Failure()
    {
        //Arrange
        var command = _factory.GenerateCategoryUpdateCommand();
        var existingCategories = new[]
        {
            _factory.GenerateCategory(name: "NotTheSame"),
            _factory.GenerateCategory(name: "NotTheSameEither"),
            _factory.GenerateCategory(name: "ToBeUpdated")
        };
        
        //Setup
        _fixture.SetupGetCategoriesAsync(existingCategories);
        
        //Act
        await _commandHandler.Handle(command, CancellationToken.None);
        
        //Assert
        _fixture.Mocker
            .GetMock<ICategoryRepository>()
            .Verify(r => r.GetCategoriesAsync(It.IsAny<Expression<Func<CategoryDomain, bool>>>(),
                    It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<Expression<Func<CategoryDomain, object>>[]>()),
                Times.Once);
        _fixture.Mocker
            .GetMock<IMemoryBus>()
            .Verify(r => r.RaiseValidationError(It.Is<ErrorCode>(x => x == ErrorCode.DomainValidationError),
                It.Is<string>(x => x == $"Category does not exist. Id: {command.Id}")), Times.Once);

        _fixture.Mocker
            .GetMock<ICategoryRepository>()
            .Verify(r => r.BeginTransactionAsync(), Times.Never);
        _fixture.Mocker
            .GetMock<ICategoryRepository>()
            .Verify(r => r.UpdateCategoryAsync(It.Is<CategoryDomain>(x =>
                x.Id == command.Id &&
                x.Name == command.Name)), Times.Never);
        _fixture.Mocker
            .GetMock<ICategoryRepository>()
            .Verify(r => r.CommitTransactionAsync(), Times.Never);
    }
    
    [Fact]
    [Trait("CategoryUpdateCommand", "Category Already Exists - Failure")]
    public async Task CategoryUpdateCommand_CategoryAlreadyExists_Failure()
    {
        //Arrange
        var command = _factory.GenerateCategoryUpdateCommand();
        var existingCategories = new[]
        {
            _factory.GenerateCategory(name: "NotTheSame"),
            _factory.GenerateCategory(name: command.Name),
            _factory.GenerateCategory(id: command.Id, name: "ToBeUpdated")
        };
        
        //Setup
        _fixture.SetupGetCategoriesAsync(existingCategories);
        
        //Act
        await _commandHandler.Handle(command, CancellationToken.None);
        
        //Assert
        _fixture.Mocker
            .GetMock<ICategoryRepository>()
            .Verify(r => r.GetCategoriesAsync(It.IsAny<Expression<Func<CategoryDomain, bool>>>(),
                    It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<Expression<Func<CategoryDomain, object>>[]>()),
                Times.Once);
        _fixture.Mocker
            .GetMock<IMemoryBus>()
            .Verify(r => r.RaiseValidationError(It.Is<ErrorCode>(x => x == ErrorCode.DomainValidationError),
                It.Is<string>(x => x == $"Category with the same name already exists. Name: {command.Name}")), Times.Once);

        _fixture.Mocker
            .GetMock<ICategoryRepository>()
            .Verify(r => r.BeginTransactionAsync(), Times.Never);
        _fixture.Mocker
            .GetMock<ICategoryRepository>()
            .Verify(r => r.UpdateCategoryAsync(It.Is<CategoryDomain>(x =>
                x.Id == command.Id &&
                x.Name == command.Name)), Times.Never);
        _fixture.Mocker
            .GetMock<ICategoryRepository>()
            .Verify(r => r.CommitTransactionAsync(), Times.Never);
    }
    
    [Fact]
    [Trait("CategoryUpdateCommand", "InvalidCategory - Failure")]
    public async Task CategoryUpdateCommand_InvalidCategory_Failure()
    {
        //Arrange
        var command = _factory.GenerateCategoryUpdateCommand(name: string.Empty);
        var existingCategories = new[]
        {
            _factory.GenerateCategory(name: "NotTheSame"),
            _factory.GenerateCategory(name: "NotTheSameEither"),
            _factory.GenerateCategory(id: command.Id, name: "ToBeUpdated")
        };
        
        //Setup
        _fixture.SetupGetCategoriesAsync(existingCategories);
        
        //Act
        await _commandHandler.Handle(command, CancellationToken.None);
        
        //Assert
        _fixture.Mocker
            .GetMock<ICategoryRepository>()
            .Verify(r => r.GetCategoriesAsync(It.IsAny<Expression<Func<CategoryDomain, bool>>>(),
                    It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<Expression<Func<CategoryDomain, object>>[]>()),
                Times.Once);
        _fixture.Mocker
            .GetMock<IMemoryBus>()
            .Verify(r => r.RaiseValidationError(It.Is<ErrorCode>(x => x == ErrorCode.DomainValidationError),
                It.Is<string>(x => x == "Category name must be informed.")), Times.Once);
        _fixture.Mocker
            .GetMock<IMemoryBus>()
            .Verify(r => r.RaiseValidationError(It.Is<ErrorCode>(x => x == ErrorCode.DomainValidationError),
                It.Is<string>(x => x == "Category name must be, at least, 3 characters long.")), Times.Once);

        _fixture.Mocker
            .GetMock<ICategoryRepository>()
            .Verify(r => r.BeginTransactionAsync(), Times.Never);
        _fixture.Mocker
            .GetMock<ICategoryRepository>()
            .Verify(r => r.UpdateCategoryAsync(It.Is<CategoryDomain>(x =>
                x.Id == command.Id &&
                x.Name == command.Name)), Times.Never);
        _fixture.Mocker
            .GetMock<ICategoryRepository>()
            .Verify(r => r.CommitTransactionAsync(), Times.Never);
    }
    
    [Fact]
    [Trait("CategoryUpdateCommand", "Database Exception - Failure")]
    public async Task CategoryUpdateCommand_DatabaseException_Failure()
    {
        //Arrange
        var command = _factory.GenerateCategoryUpdateCommand();
        var existingCategories = new[]
        {
            _factory.GenerateCategory(name: "NotTheSame"),
            _factory.GenerateCategory(name: "NotTheSameEither"),
            _factory.GenerateCategory(id: command.Id, name: "ToBeUpdated")
        };
        
        //Setup
        _fixture.SetupGetCategoriesAsync(existingCategories);
        _fixture.SetupCommitTransactionAsyncException();
        
        //Act
        await _commandHandler.Handle(command, CancellationToken.None);
        
        //Assert
        _fixture.Mocker
            .GetMock<ICategoryRepository>()
            .Verify(r => r.GetCategoriesAsync(It.IsAny<Expression<Func<CategoryDomain, bool>>>(),
                    It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<Expression<Func<CategoryDomain, object>>[]>()),
                Times.Once);
        _fixture.Mocker
            .GetMock<ICategoryRepository>()
            .Verify(r => r.BeginTransactionAsync(), Times.Once);
        _fixture.Mocker
            .GetMock<ICategoryRepository>()
            .Verify(r => r.UpdateCategoryAsync(It.Is<CategoryDomain>(x =>
                x.Id == command.Id &&
                x.Name == command.Name)), Times.Once);
        _fixture.Mocker
            .GetMock<ICategoryRepository>()
            .Verify(r => r.CommitTransactionAsync(), Times.Once);
        _fixture.Mocker
            .GetMock<IMemoryBus>()
            .Verify(r => r.RaiseValidationError(It.Is<ErrorCode>(x => x == ErrorCode.DomainValidationError),
                It.Is<string>(x => x == "A fatal error occurred. The operation could not be completed.")), Times.Once);
        _fixture.Mocker
            .GetMock<ICategoryRepository>()
            .Verify(r => r.RollBackTransactionAsync(), Times.Once);
    }

    [Fact]
    [Trait("CategoryRemoveCommand", "Success")]
    public async Task CategoryRemoveCommand_Success()
    {
        //Arrange
        var command = _factory.GenerateCategoryRemoveCommand();
        var existingCategories = new[]
        {
            _factory.GenerateCategory(name: "NotTheSame"),
            _factory.GenerateCategory(name: "NotTheSameEither"),
            _factory.GenerateCategory(id: command.Id, name: "ToBeRemoved")
        };
        
        //Setup
        _fixture.SetupGetCategoryByIdAsync(existingCategories);
        
        //Act
        await _commandHandler.Handle(command, CancellationToken.None);
        
        //Assert
        _fixture.Mocker
            .GetMock<ICategoryRepository>()
            .Verify(r => r.BeginTransactionAsync(), Times.Once);
        _fixture.Mocker
            .GetMock<ICategoryRepository>()
            .Verify(r => r.RemoveCategoryAsync(It.Is<CategoryDomain>(x =>
                x.Id == command.Id)), Times.Once);
        _fixture.Mocker
            .GetMock<ICategoryRepository>()
            .Verify(r => r.CommitTransactionAsync(), Times.Once);
    }
    
    [Fact]
    [Trait("CategoryRemoveCommand", "Category does not exist - Failure")]
    public async Task CategoryRemoveCommand_CategoryDoesNotExist_Failure()
    {
        //Arrange
        var command = _factory.GenerateCategoryRemoveCommand();
        var existingCategories = new[]
        {
            _factory.GenerateCategory(name: "NotTheSame"),
            _factory.GenerateCategory(name: "NotTheSameEither"),
            _factory.GenerateCategory(name: "ToBeRemoved")
        };
        
        //Setup
        _fixture.SetupGetCategoryByIdAsync(existingCategories);
        
        //Act
        await _commandHandler.Handle(command, CancellationToken.None);
        
        //Assert
        _fixture.Mocker
            .GetMock<IMemoryBus>()
            .Verify(r => r.RaiseValidationError(It.Is<ErrorCode>(x => x == ErrorCode.DomainValidationError),
                It.Is<string>(x => x == $"Category does not exist. Id: {command.Id}")), Times.Once);

        _fixture.Mocker
            .GetMock<ICategoryRepository>()
            .Verify(r => r.BeginTransactionAsync(), Times.Never);
        _fixture.Mocker
            .GetMock<ICategoryRepository>()
            .Verify(r => r.RemoveCategoryAsync(It.Is<CategoryDomain>(x =>
                x.Id == command.Id)), Times.Never);
        _fixture.Mocker
            .GetMock<ICategoryRepository>()
            .Verify(r => r.CommitTransactionAsync(), Times.Never);
    }
    
    [Fact]
    [Trait("CategoryRemoveCommand", "Database Exception - Failure")]
    public async Task CategoryRemoveCommand_DatabaseException_Failure()
    {
        //Arrange
        var command = _factory.GenerateCategoryRemoveCommand();
        var existingCategories = new[]
        {
            _factory.GenerateCategory(name: "NotTheSame"),
            _factory.GenerateCategory(name: "NotTheSameEither"),
            _factory.GenerateCategory(id: command.Id, name: "ToBeRemoved")
        };
        
        //Setup
        _fixture.SetupGetCategoryByIdAsync(existingCategories);
        _fixture.SetupCommitTransactionAsyncException();
        
        //Act
        await _commandHandler.Handle(command, CancellationToken.None);
        
        //Assert
        _fixture.Mocker
            .GetMock<ICategoryRepository>()
            .Verify(r => r.BeginTransactionAsync(), Times.Once);
        _fixture.Mocker
            .GetMock<ICategoryRepository>()
            .Verify(r => r.RemoveCategoryAsync(It.Is<CategoryDomain>(x =>
                x.Id == command.Id)), Times.Once);
        _fixture.Mocker
            .GetMock<ICategoryRepository>()
            .Verify(r => r.CommitTransactionAsync(), Times.Once);
        _fixture.Mocker
            .GetMock<IMemoryBus>()
            .Verify(r => r.RaiseValidationError(It.Is<ErrorCode>(x => x == ErrorCode.DomainValidationError),
                It.Is<string>(x => x == "A fatal error occurred. The operation could not be completed.")), Times.Once);
        _fixture.Mocker
            .GetMock<ICategoryRepository>()
            .Verify(r => r.RollBackTransactionAsync(), Times.Once);
    }
}