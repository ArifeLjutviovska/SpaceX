namespace SpacexServer.Api.Common.Security
{
    using System.Security.Cryptography;

    /// <summary>
    /// Provides secure password hashing and verification using PBKDF2 (HMAC-SHA512).
    /// Uses a cryptographic salt and multiple iterations to enhance security.
    /// </summary>
    public class PasswordHasher
    {
        private static readonly int SaltSize = 16;
        private static readonly int HashSize = 20;
        private static readonly int Iterations = 10000;

        /// <summary>
        /// Hashes a password using PBKDF2 (HMAC-SHA512) with a random cryptographic salt.
        /// The resulting hash is encoded as a base64 string for storage.
        /// </summary>
        /// <param name="password">The plaintext password to hash.</param>
        /// <returns>A base64-encoded string containing both the salt and hashed password.</returns>
        public static string HashPassword(string password)
        {
            byte[] salt = new byte[SaltSize];

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            using var key = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA512);
            byte[] hash = key.GetBytes(HashSize);

            byte[] hashBytes = new byte[SaltSize + HashSize];
            Array.Copy(salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

            return Convert.ToBase64String(hashBytes);
        }

        /// <summary>
        /// Verifies a password against a stored base64-encoded hash.
        /// Uses PBKDF2 (HMAC-SHA512) with the original salt for comparison.
        /// </summary>
        /// <param name="password">The plaintext password to verify.</param>
        /// <param name="base64Hash">The stored base64-encoded hash including salt.</param>
        /// <returns>True if the password matches the stored hash, otherwise false.</returns>
        public static bool VerifyPassword(string password, string base64Hash)
        {
            byte[] hashBytes = Convert.FromBase64String(base64Hash);

            byte[] salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);

            using var key = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA512);
            byte[] hash = key.GetBytes(HashSize);

            for (var i = 0; i < HashSize; i++)
            {
                if (hashBytes[i + SaltSize] != hash[i])
                    return false;
            }
            return true;
        }
    }
}