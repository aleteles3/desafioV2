using System.ComponentModel.DataAnnotations;

namespace Application.Users.ViewModels.Crud;

public class AddUserViewModel
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "User Login must be informed.")]
    public string Login { get; set; }
    [Required(AllowEmptyStrings = false, ErrorMessage = "User Password must be informed.")]
    public string Password { get; set; }
}