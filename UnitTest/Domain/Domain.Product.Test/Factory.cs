using System;
using ProductDomain = Domain.Product.Entities.Product;

namespace Domain.Product.Test
{
    public partial class Factory
    {
        public ProductDomain GenerateProduct(Guid id, string name, string description, decimal price,
            int stock, Guid categoryId)
        {
            return new ProductDomain(id, name, description, price, stock, categoryId);
        }
    }
}