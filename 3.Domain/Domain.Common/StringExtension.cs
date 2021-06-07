using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common
{
  public static class StringExtension
  {
    private readonly static string _salt = "kBhVPEu8RjM9KdXB6W8eJTFzsAVlYpjy";

    public static string HashPassword(this string password)
    {
      // generate a 128-bit salt using a secure PRNG
      var salt = Convert.FromBase64String(_salt);

      // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
      string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
          password: password,
          salt: salt,
          prf: KeyDerivationPrf.HMACSHA1,
          iterationCount: 10000,
          numBytesRequested: 256 / 8));
      return hashed;
    }
  }
}