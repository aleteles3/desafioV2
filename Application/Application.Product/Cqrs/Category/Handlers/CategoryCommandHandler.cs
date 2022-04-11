using Application.Product.Cqrs.Category.Commands;
using Domain.Product.Interfaces;
using MediatR;
using CategoryDomain = Domain.Product.Entities.Category;

namespace Application.Product.Cqrs.Category.Handlers;

public class CategoryCommandHandler :
    IRequestHandler<CategoryAddCommand, Guid?>,
    IRequestHandler<CategoryUpdateCommand>,
    IRequestHandler<CategoryRemoveCommand>
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryCommandHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }
    
    public async Task<Guid?> Handle(CategoryAddCommand request, CancellationToken cancellationToken)
    {
        var category = new CategoryDomain(Guid.NewGuid(), request.Name);

        if (!category.IsValid())
        {
            //ToDo Create memory Bus to store the errors
            foreach (var error in category.ValidationResult.Errors)
                Console.WriteLine(error);

            return null;
        }

        var categories = await _categoryRepository.GetCategoriesAsync(x =>
            x.Name.ToUpper().Equals(request.Name.ToUpper()));

        if (categories.Any())
        {
            Console.WriteLine("Category with the same name already exists.");

            return null;
        }

        try
        {
            await _categoryRepository.BeginTransactionAsync();
            await _categoryRepository.AddCategoryAsync(category);
            await _categoryRepository.CommitTransactionAsync();

            return category.Id;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);

            await _categoryRepository.RollBackTransactionAsync();

            return null;
        }
    }

    public Task<Unit> Handle(CategoryUpdateCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Unit> Handle(CategoryRemoveCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}