using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using WeCount.Application.Common.Interfaces;

namespace WeCount.Infrastructure.Services;

public class PasswordHasher : IPasswordHasher
{
    private const int SaltSize = 128 / 8;
    private const int KeySize = 256 / 8;
    private const int Iterations = 10000;
    private static readonly KeyDerivationPrf Prf = KeyDerivationPrf.HMACSHA256;

    public string HashPassword(string password)
    {
        // Generate a random salt
        byte[] salt = new byte[SaltSize];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }

        // Derive a key from the password
        byte[] key = KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: Prf,
            iterationCount: Iterations,
            numBytesRequested: KeySize
        );

        // Combine salt and key into a single string
        byte[] combined = new byte[SaltSize + KeySize];
        Buffer.BlockCopy(salt, 0, combined, 0, SaltSize);
        Buffer.BlockCopy(key, 0, combined, SaltSize, KeySize);

        return Convert.ToBase64String(combined);
    }

    public bool VerifyPassword(string password, string passwordHash)
    {
        // Extract salt and key from the hash
        byte[] combined = Convert.FromBase64String(passwordHash);
        if (combined.Length != SaltSize + KeySize)
        {
            return false;
        }

        byte[] salt = new byte[SaltSize];
        byte[] storedKey = new byte[KeySize];
        Buffer.BlockCopy(combined, 0, salt, 0, SaltSize);
        Buffer.BlockCopy(combined, SaltSize, storedKey, 0, KeySize);

        // Derive a key from the password using the extracted salt
        byte[] computedKey = KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: Prf,
            iterationCount: Iterations,
            numBytesRequested: KeySize
        );

        // Compare the computed key with the stored key
        return CryptographicOperations.FixedTimeEquals(storedKey, computedKey);
    }
}
