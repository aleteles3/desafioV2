using Domain.Core.Commands;
using Domain.Core.Interfaces;
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

    public CategoryCommandHandler(ICategoryRepository categoryRepository, IMemoryBus memoryBus) : base(memoryBus)
    {
        _categoryRepository = categoryRepository;
    }
    
    public async Task<Guid?> Handle(CategoryAddCommand request, CancellationToken cancellationToken)
    {
        var category = new CategoryDomain(Guid.NewGuid(), request.Name);

        if (!category.IsValid())
        {
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
            NotifyValidationErrors("A fatal error occurred. The operation could not be completed.");
            await _categoryRepository.RollBackTransactionAsync();
            return null;
        }
    }

    public async Task<Unit> Handle(CategoryUpdateCommand request, CancellationToken cancellationToken)
    {
        var categories = (await _categoryRepository
            .GetCategoriesAsync(x => x.Id == request.Id || x.Name.ToUpper() == request.Name.ToUpper())).ToList();
        
        var category = categories.FirstOrDefault(x => x.Id == request.Id);
        if (category == null)
        {
            NotifyValidationErrors($"Category does not exist. Id: {request.Id}");
            return Unit.Value;
        }
        
        var categoriesWithTheSameName = categories.Where(x => x.Name.ToUpper() == request.Name.ToUpper());
        if (categoriesWithTheSameName.Any())
        {
            NotifyValidationErrors($"Category with the same name already exists. Name: {request.Name}");
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
            NotifyValidationErrors("A fatal error occurred. The operation could not be completed.");
            await _categoryRepository.RollBackTransactionAsync();
        }
        
        return Unit.Value;
    }

    public async Task<Unit> Handle(CategoryRemoveCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetCategoryByIdAsync(request.Id);

        if (category == null)
        {
            NotifyValidationErrors($"Category does not exist. Id: {request.Id}");
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
            NotifyValidationErrors("A fatal error occurred. The operation could not be completed.");
            await _categoryRepository.RollBackTransactionAsync();
        }

        return Unit.Value;
    }
}