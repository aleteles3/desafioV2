using System;
using Domain.Domain.Produto.Entities;

namespace UnitTest.Domain.Domain.Produto.Test
{
    public class Factory
    {
        public Category GenerateCategory(Guid id, string name)
        {
            return new Category(id, name);
        }

        public Product GenerateProduct(Guid id, string name, string description, decimal price,
            int stock, Guid categoryId)
        {
            return new Product(id, name, description, price, stock, categoryId);
        }
    }
}