namespace Dapper.WebAPI.Helpers
{
    public class PasswordEncrypt
    {
        public string GeneratePasswordHash(string? password)
        {
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);
            return passwordHash;
        }
    }
}
