using System;
using System.Linq;
using Xunit;

namespace Domain.Product.Test.EntitiesTest
{
    public partial class EntitiesTest
    {
        private readonly Factory _factory;
        
        public EntitiesTest()
        {
            _factory = new Factory();
        }

        [Fact(DisplayName = "Creating Category - Validating Properties - Success")]
        public void CreatingCategory_ValidatingProperties_Success()
        {
            //Arrange
            var category = _factory.GenerateCategory(Guid.NewGuid(), "Category Name");
            
            //Act
            var isValid = category.IsValid();
            
            //Assert
            Assert.True(isValid);
            Assert.Empty(category.ValidationResult.Errors);
        }
        
        [Fact(DisplayName = "Creating Category - Validating Properties - Failure")]
        public void CreatingCategory_ValidatingProperties_Failure()
        {
            //Arrange
            var category = _factory.GenerateCategory(Guid.NewGuid(), string.Empty);
            
            //Act
            var isValid = category.IsValid();
            
            //Assert
            Assert.False(isValid);
            Assert.NotEmpty(category.ValidationResult.Errors);

            var errorMessages = category.ValidationResult.Errors.Select(x => x.ErrorMessage).ToList();
            Assert.Contains("Category name must be informed.", errorMessages);
            Assert.Contains("Category name must be, at least, 3 characters long.", errorMessages);
        }
    }
}