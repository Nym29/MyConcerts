using System;
using System.Threading.Tasks;
using Firebase.Auth;
using MyConcerts.Data;

namespace MyConcerts.iOS
{
    public class FirebaseAuthenticator : IFirebaseAuthenticator
    {
        public async Task<string> LoginWithEmailPassword(string email, string password)
        {
            var user = await Auth.DefaultInstance.SignInWithPasswordAsync(email, password);
            return await user.User.GetIdTokenAsync();
        }
    }
}
