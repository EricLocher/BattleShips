using Firebase.Auth;
using SaveData;
using System.Threading.Tasks;
using UnityEngine;

public class User : MonoBehaviour
{
    private static User _instance;
    public static User Instance { get { return _instance; } }

    public static UserData data;
    public static FirebaseUser user;
    public static GameData activeGame = null;
    public static string userPath;

    void Awake()
    {
        if (_instance == null) {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(this.gameObject);
        }
    }

    #region Login/Register user

    public async static Task RegisterUser(string email, string password, string username)
    {
        if (user != null) { print("A user is already logged in!"); return; }
        await FireBaseLogin.RegisterNewUser(email, password, username);

        return;
    }

    public async static Task SignIn(string email, string password)
    {
        if (user != null) { print("A user is already logged in!"); return; }
        user = await FireBaseLogin.SignIn(email, password);
        data = await LoadUserData();

        return;
    }

    public async static void AnonymousSignIn()
    {
        if (user != null) { print("A user is already logged in!"); return; }
        user = await FireBaseLogin.AnonymousSignIn();
    }

    #endregion

    #region Matchmaking
    public static async Task<bool> Matchmake()
    {
        GameData foundGame;

        foundGame = await GameFinder.FindGame();

        if (foundGame != null) { activeGame = foundGame; Debug.Log("Game found..."); Debug.Log(foundGame.ToString()); return true; }

        foundGame = await GameFinder.CreateGame();
        if (foundGame != null) { activeGame = foundGame; return true; }

        Debug.Log("Something went wrong whilst matchmaking...");
        return false;
    }

    #endregion

    static async Task<UserData> LoadUserData()
    {
        userPath = $"users/{FirebaseAuth.DefaultInstance.CurrentUser.UserId}";
        return await SaveManager.LoadObject<UserData>(userPath);
    }
}
