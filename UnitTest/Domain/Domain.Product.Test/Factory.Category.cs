using System;
using Domain.Product.Cqrs.Category.Commands;
using Domain.Product.Entities;

namespace Domain.Product.Test;

public partial class Factory
{
    public Category GenerateCategory(Guid? id = null, string name = null)
    {
        return new Category(id ?? Guid.NewGuid(), name ?? "Name");
    }

    public CategoryAddCommand GenerateCategoryAddCommand(string name)
    {
        return new CategoryAddCommand(name);
    }

    public CategoryUpdateCommand GenerateCategoryUpdateCommand(Guid? id = null,
        string name = null)
    {
        return new CategoryUpdateCommand(id ?? Guid.NewGuid(), name ?? "Name");
    }

    public CategoryRemoveCommand GenerateCategoryRemoveCommand(Guid? id = null)
    {
        return new CategoryRemoveCommand(id ?? Guid.NewGuid());
    }
}