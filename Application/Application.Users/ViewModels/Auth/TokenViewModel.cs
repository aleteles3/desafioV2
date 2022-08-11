namespace Application.Users.ViewModels.Auth;

public class TokenViewModel
{
    public string Token { get; set; }
    public DateTime TokenExpirationDate { get; set; }
    public string RefreshToken { get; set; }
    public DateTime RefreshTokenExpirationDate { get; set; }
}