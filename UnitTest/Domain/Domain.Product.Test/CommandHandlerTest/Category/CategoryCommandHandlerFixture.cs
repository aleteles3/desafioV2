using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Domain.Product.Cqrs.Category.Handlers;
using Domain.Product.Interfaces;
using Moq;
using Moq.AutoMock;
using Xunit;
using CategoryDomain = Domain.Product.Entities.Category;

namespace Domain.Product.Test.CommandHandlerTest.Category;

[CollectionDefinition(nameof(CategoryCommandHandlerCollection))]
public class CategoryCommandHandlerCollection : ICollectionFixture<CategoryCommandHandlerFixture>{ }
public class CategoryCommandHandlerFixture
{
    private CategoryCommandHandler CategoryCommandHandler { get; set; }
    public AutoMocker Mocker { get; set; }

    public CategoryCommandHandler GetCategoryCommandHandler()
    {
        Mocker = new AutoMocker();
        
        CategoryCommandHandler = Mocker.CreateInstance<CategoryCommandHandler>();

        return CategoryCommandHandler;
    }

    public void SetupGetCategoriesAsync(IEnumerable<CategoryDomain> categories)
    {
        Mocker
            .GetMock<ICategoryRepository>()
            .Setup(s => s.GetCategoriesAsync(It.IsAny<Expression<Func<CategoryDomain, bool>>>(),
                It.IsAny<int?>(), It.IsAny<int?>(), It.IsAny<Expression<Func<CategoryDomain, object>>[]>()))
            .ReturnsAsync((Expression<Func<CategoryDomain, bool>> predicate, int? start, int? length,
                Expression<Func<CategoryDomain, object>>[] includes) => categories.Where(predicate.Compile()));
    }

    public void SetupGetCategoryByIdAsync(IEnumerable<CategoryDomain> categories)
    {
        Mocker
            .GetMock<ICategoryRepository>()
            .Setup(s => s.GetCategoryByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync((Guid id) => categories.FirstOrDefault(x => x.Id == id));
    }

    public void SetupCommitTransactionAsyncException()
    {
        Mocker
            .GetMock<ICategoryRepository>()
            .Setup(s => s.CommitTransactionAsync())
            .Callback(() => throw new Exception());
    }
}