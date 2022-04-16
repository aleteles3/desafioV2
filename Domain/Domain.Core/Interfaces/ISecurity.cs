namespace Domain.Core.Interfaces;

public interface ISecurity
{
    string EncryptString(string value, string salt);
}