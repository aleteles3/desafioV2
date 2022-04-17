using System;
using Domain.Product.Cqrs.Product.Commands;
using ProductDomain = Domain.Product.Entities.Product;

namespace Domain.Product.Test
{
    public partial class Factory
    {
        public ProductDomain GenerateProduct(Guid? id = null, string name = null, 
            string description = null, decimal? price = null, int? stock = null, 
            Guid? categoryId = null)
        {
            return new ProductDomain(id ?? Guid.NewGuid(), name ?? "Name", description ?? "Description",
                price ?? 100, stock ?? 15, categoryId ?? Guid.NewGuid());
        }

        public ProductAddCommand GenerateProductAddCommand(string name = null, string description = null,
            decimal? price = null, int? stock = null, Guid? categoryId = null)
        {
            return new ProductAddCommand(name ?? "Name", description ?? "description", price ?? 100,
                stock ?? 15, categoryId ?? Guid.NewGuid());
        }
        
        public ProductUpdateCommand GenerateProductUpdateCommand(Guid? id = null, string name = null, 
            string description = null, decimal? price = null, int? stock = null, Guid? categoryId = null)
        {
            return new ProductUpdateCommand(id ?? Guid.NewGuid(), name ?? "Name", 
                description ?? "description", price ?? 100, stock ?? 15, categoryId ?? Guid.NewGuid());
        }

        public ProductRemoveCommand GenerateProductRemoveCommand(Guid? id = null)
        {
            return new ProductRemoveCommand(id ?? Guid.NewGuid());
        }
    }
}