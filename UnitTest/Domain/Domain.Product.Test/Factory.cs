using System;
using Domain.Product.Entities;
using ProductDomain = Domain.Product.Entities.Product;

namespace Domain.Product.Test
{
    public class Factory
    {
        public Category GenerateCategory(Guid id, string name)
        {
            return new Category(id, name);
        }

        public ProductDomain GenerateProduct(Guid id, string name, string description, decimal price,
            int stock, Guid categoryId)
        {
            return new ProductDomain(id, name, description, price, stock, categoryId);
        }
    }
}