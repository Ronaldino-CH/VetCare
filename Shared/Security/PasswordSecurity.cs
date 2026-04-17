using System.Security.Cryptography;

namespace Shared.Security
{
    public static class PasswordSecurity
    {
        private const int SaltSize = 16;
        private const int HashSize = 32;
        private const int Iterations = 100_000;
        private const string Prefix = "pbkdf2";

        public static string HashPassword(string rawPassword)
        {
            if (string.IsNullOrWhiteSpace(rawPassword))
                throw new ArgumentException("La contraseña no puede estar vacía.", nameof(rawPassword));

            var saltBytes = RandomNumberGenerator.GetBytes(SaltSize);
            var hashBytes = Rfc2898DeriveBytes.Pbkdf2(
                rawPassword,
                saltBytes,
                Iterations,
                HashAlgorithmName.SHA256,
                HashSize);

            return $"{Prefix}${Iterations}${Convert.ToBase64String(saltBytes)}${Convert.ToBase64String(hashBytes)}";
        }

        public static bool VerifyPassword(string rawPassword, string storedPasswordHash)
        {
            if (string.IsNullOrWhiteSpace(rawPassword) || string.IsNullOrWhiteSpace(storedPasswordHash))
                return false;

            if (IsPbkdf2Hash(storedPasswordHash) && TryParseHash(storedPasswordHash, out var iterations, out var salt, out var hash))
            {
                var testHash = Rfc2898DeriveBytes.Pbkdf2(
                    rawPassword,
                    salt,
                    iterations,
                    HashAlgorithmName.SHA256,
                    hash.Length);

                return CryptographicOperations.FixedTimeEquals(testHash, hash);
            }

            return string.Equals(rawPassword, storedPasswordHash, StringComparison.Ordinal);
        }

        private static bool IsPbkdf2Hash(string value) => value.StartsWith($"{Prefix}$", StringComparison.Ordinal);

        private static bool TryParseHash(string value, out int iterations, out byte[] salt, out byte[] hash)
        {
            iterations = 0;
            salt = Array.Empty<byte>();
            hash = Array.Empty<byte>();

            var parts = value.Split('$', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 4) return false;
            if (!int.TryParse(parts[1], out iterations)) return false;

            try
            {
                salt = Convert.FromBase64String(parts[2]);
                hash = Convert.FromBase64String(parts[3]);
                return salt.Length > 0 && hash.Length > 0;
            }
            catch
            {
                return false;
            }
        }
    }
}
