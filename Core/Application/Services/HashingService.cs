using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;

namespace Application.Services
{
    // TODO: As for me, the hashing logic fits better into the Infrastructure layer
    public class HashingService
    {
        private readonly IConfiguration _configuration;
        public HashingService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public byte[] GetHash(string password)
        {
            using var hmac = new HMACSHA512(System.Text.Encoding.UTF8.GetBytes(_configuration["PasswordSalt"]));
            return hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }
}
