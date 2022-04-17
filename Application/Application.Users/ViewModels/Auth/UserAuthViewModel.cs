using System.ComponentModel.DataAnnotations;

namespace Application.User.ViewModels.Auth;

public class UserAuthViewModel
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "Login must be informed.")]
    public string Login { get; set; }
    [Required(AllowEmptyStrings = false, ErrorMessage = "Password must be informed.")]
    public string Password { get; set; }
}