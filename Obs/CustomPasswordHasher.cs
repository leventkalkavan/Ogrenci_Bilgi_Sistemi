using Microsoft.AspNetCore.Identity;
using Obs.Models;

namespace Obs
{
    public class CustomPasswordHasher : IPasswordHasher<AppUser>
    {

        private readonly AdvancedEncryptionStandardProvider _provider;

        public CustomPasswordHasher()
        {
            _provider = new AdvancedEncryptionStandardProvider();
        }

        public string HashPassword(AppUser appUser, string password)
        {
            // Do no hashing
            return _provider.Encrypt(password);
        }

        public PasswordVerificationResult VerifyHashedPassword(AppUser appUser, string hashedPassword, string providedPassword)
        {


            // Just check if the two values are the same
            if (hashedPassword.Equals(this.HashPassword(appUser, providedPassword)))
                return PasswordVerificationResult.Success;

            // Fallback
            return PasswordVerificationResult.Failed;
        }

    }
}