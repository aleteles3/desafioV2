using System.ComponentModel.DataAnnotations;

namespace Application.Product.ViewModels.Crud;

public class UpdateProductViewModel
{
    [Required(ErrorMessage = "Product Id must be informed.")]
    public Guid? Id { get; set; }
    [Required(AllowEmptyStrings = false, ErrorMessage = "Product Name must be informed.")]
    [MinLength(3, ErrorMessage = "Product name must be, at least, 3 characters long.")]
    public string Name { get; set; }
    [Required(AllowEmptyStrings = false, ErrorMessage = "Product Name must be informed.")]
    public string Description { get; set; }
    [Required(ErrorMessage = "Product ListPrice must be informed.")]
    [Range(0, double.MaxValue)]
    public decimal? Price { get; set; }
    [Required(ErrorMessage = "Category Id must be informed.")]
    public Guid? CategoryId { get; set; }
}