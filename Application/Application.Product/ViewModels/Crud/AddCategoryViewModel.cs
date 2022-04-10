using System.ComponentModel.DataAnnotations;

namespace Application.Product.ViewModels.Crud;

public class AddCategoryViewModel
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "Category Name must be informed.")]
    [MinLength(3)]
    public string Name { get; set; }
}