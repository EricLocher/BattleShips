using Firebase.Auth;
using SaveData;
using System.Threading.Tasks;
using UnityEngine;


public static class FireBaseLogin
{
    public static async Task<bool> RegisterNewUser(string email, string password, string username)
    {
        Debug.Log("Attempting to register a new user");

        return await SaveManager.auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task => {

            if (task.Exception != null) {
                Debug.LogWarning(task.Exception);
                return false;
            }
            UserData newUser = new UserData(username);
            SaveManager.SaveObject($"users/{task.Result.UserId}", newUser);

            return true;
        });
    }

    public static async Task<FirebaseUser> SignIn(string email, string password)
    {
        Debug.Log("Attempting to sign in user");

        return await SaveManager.auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if (task.Exception != null) {
                Debug.LogWarning(task.Exception);
                return null;
            }
            else {
                Debug.Log("Successfully signed in user!");
                return task.Result;
            }
        });
    }

    public static async Task<FirebaseUser> AnonymousSignIn()
    {
        Debug.Log("Attempting to sign in anonymously");

        return await SaveManager.auth.SignInAnonymouslyAsync().ContinueWith(task => {
            if (task.Exception != null) {
                Debug.LogWarning(task.Exception);
                return null;
            }
            else {
                return task.Result;
            }
        });
    }
}
