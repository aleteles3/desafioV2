using System.ComponentModel.DataAnnotations;

namespace Application.Product.ViewModels.Crud;

public class UpdateCategoryViewModel
{
    [Required(ErrorMessage = "Category Id must be informed.")]
    public Guid? Id { get; set; }
    [Required(AllowEmptyStrings = false, ErrorMessage = "Category Name must be informed.")]
    [MinLength(3)]
    public string Name { get; set; }
}