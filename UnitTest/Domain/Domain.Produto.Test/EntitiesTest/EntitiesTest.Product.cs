using System;
using System.Linq;
using Xunit;

namespace Domain.Produto.Test.EntitiesTest
{
    public partial class EntitiesTest
    {
        [Fact(DisplayName = "Creating Product - Validating Properties - Success")]
        public void CreatingProduct_ValidatingProperties_Success()
        {
            //Arrange
            var product = _factory.GenerateProduct(Guid.NewGuid(), "Product Name", "Product Description", 9001, 2,
                Guid.NewGuid());
            
            //Act
            var isValid = product.IsValid();
            
            //Assert
            Assert.True(isValid);
            Assert.Empty(product.ValidationResult.Errors);
        }
        
        [Fact(DisplayName = "Creating Product - Validating Properties - Failure")]
        public void CreatingProduct_ValidatingProperties_Failure()
        {
            //Arrange
            var product = _factory.GenerateProduct(Guid.NewGuid(), string.Empty, string.Empty, 0, -1,
                Guid.Empty);
            
            //Act
            var isValid = product.IsValid();
            
            //Assert
            Assert.False(isValid);
            Assert.NotEmpty(product.ValidationResult.Errors);
            
            var errorMessages = product.ValidationResult.Errors.Select(x => x.ErrorMessage).ToList();
            Assert.Contains("Product name must be informed.", errorMessages);
            Assert.Contains("Product name must be, at least, 3 characters long.", errorMessages);
            Assert.Contains("Product description must be informed.", errorMessages);
            Assert.Contains("Product price must be greater than 0", errorMessages);
            Assert.Contains("Product stock cannot be less than 0.", errorMessages);
            Assert.Contains("Category Id must be informed.", errorMessages);
        }
    }
}