namespace Application.Users.ViewModels.Auth;

public class TokenViewModel
{
    public string Token { get; set; }
    public DateTime ExpirationDate { get; set; }
}