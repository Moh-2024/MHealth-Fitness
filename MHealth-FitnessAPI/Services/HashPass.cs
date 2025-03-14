using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MHealth_FitnessAPI.Services
{
    using System;
    using System.Security.Cryptography;
    using System.Text;

    public class HashPass
    {
        // Generates a secure random salt (16 bytes)
        public static string GenerateSalt()
        {
            byte[] salt = new byte[16]; // 16 bytes (128-bit salt)
            RandomNumberGenerator.Fill(salt);
            return Convert.ToBase64String(salt); // Convert to Base64 for storage
        }

        // Hashes the password using SHA-256 with the salt
        public static string HashPasswordWithSalt(string password, string salt)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] combinedBytes = Encoding.UTF8.GetBytes(password + salt);
                byte[] hashBytes = sha256.ComputeHash(combinedBytes);
                return Convert.ToBase64String(hashBytes); // Convert hash for storage
            }
        }

        // Hashes the password and returns both hash and salt
        public static (string hashedPassword, string salt) HashPassword(string password)
        {
            string salt = GenerateSalt(); // Generate a new salt
            string hashedPassword = HashPasswordWithSalt(password, salt); // Hash with the salt
            return (hashedPassword, salt); // Return both values to store in the database
        }
    }

}
