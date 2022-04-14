using Domain.Core.Commands;
using Domain.Product.Cqrs.Category.Commands;
using Domain.Product.Interfaces;
using MediatR;
using CategoryDomain = Domain.Product.Entities.Category;

namespace Domain.Product.Cqrs.Category.Handlers;

public class CategoryCommandHandler : CommandHandler,
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
            NotifyValidationErrors(category.ValidationResult);
            return null;
        }

        var categories = await _categoryRepository.GetCategoriesAsync(x =>
            x.Name.ToUpper().Equals(request.Name.ToUpper()));

        if (categories.Any())
        {
            NotifyValidationErrors("Category with the same name already exists.");
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

    public async Task<Unit> Handle(CategoryUpdateCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetCategoryByIdAsync(request.Id);

        if (category == null)
        {
            NotifyValidationErrors("Category does not exist.");
            return Unit.Value;
        }
        
        category.SetName(request.Name);

        if (!category.IsValid())
        {
            NotifyValidationErrors(category.ValidationResult);
            return Unit.Value;
        }

        try
        {
            await _categoryRepository.BeginTransactionAsync();
            await _categoryRepository.UpdateCategoryAsync(category);
            await _categoryRepository.CommitTransactionAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            await _categoryRepository.RollBackTransactionAsync();
        }
        
        return Unit.Value;
    }

    public async Task<Unit> Handle(CategoryRemoveCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetCategoryByIdAsync(request.Id);

        if (category == null)
        {
            Console.WriteLine("Category does not exist.");
            return Unit.Value;
        }

        try
        {
            await _categoryRepository.BeginTransactionAsync();
            await _categoryRepository.RemoveCategoryAsync(category);
            await _categoryRepository.CommitTransactionAsync();

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            await _categoryRepository.RollBackTransactionAsync();
        }

        return Unit.Value;
    }
}