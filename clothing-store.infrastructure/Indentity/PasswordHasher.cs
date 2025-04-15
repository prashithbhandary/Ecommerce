using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace clothing_store.infrastructure.Indentity
{
  public class PasswordHasher
  {
    private const int SaltSize = 16; // 128-bit salt
    private const int KeySize = 32;  // 256-bit key
    private const int Iterations = 10000;

    public string HashPassword(string password)
    {
      byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);

      byte[] hash = KeyDerivation.Pbkdf2(
          password: password,
          salt: salt,
          prf: KeyDerivationPrf.HMACSHA256,
          iterationCount: Iterations,
          numBytesRequested: KeySize
      );

      return $"{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
    }

    public bool VerifyPassword(string password, string hashedPassword)
    {
      var parts = hashedPassword.Split('.');
      if (parts.Length != 2) return false;

      byte[] salt = Convert.FromBase64String(parts[0]);
      byte[] hash = Convert.FromBase64String(parts[1]);

      byte[] newHash = KeyDerivation.Pbkdf2(
          password: password,
          salt: salt,
          prf: KeyDerivationPrf.HMACSHA256,
          iterationCount: Iterations,
          numBytesRequested: KeySize
      );

      return CryptographicOperations.FixedTimeEquals(newHash, hash);
    }
  }
}
