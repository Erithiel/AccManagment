namespace AccManagment.API.Services;

public interface IHashedPassword
{
    public string HashPassword(string password);
    public bool VerifyPassword(string enteredPassword, string storedHash);
}