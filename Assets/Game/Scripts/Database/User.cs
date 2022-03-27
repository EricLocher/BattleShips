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
            Destroy(gameObject);
        }
    }

    #region Login/Register user
    public async static Task RegisterUser(string email, string password, string username)
    {
        if (user != null) { print("A user is already logged in!"); return; }
        await FireBaseLogin.RegisterNewUser(email, password, username);
        return;
    }

    public async static Task<bool> SignIn(string email, string password)
    {
        if (user != null) { print("A user is already logged in!"); return false; }
        user = await FireBaseLogin.SignIn(email, password);

        if(user == null) { return false; }

        await LoadUserData();

        return true;
    }

    public async static void AnonymousSignIn()
    {
        if (user != null) { print("A user is already logged in!"); return; }
        user = await FireBaseLogin.AnonymousSignIn();
    }

    #endregion

    #region Matchmaking
    public static async Task<GameData> Matchmake()
    {
        GameData foundGame;

        //Check if there are any 'open' games.
        foundGame = await GameFinder.FindGame();
        
        if (foundGame != null && foundGame.players[0].userID != user.UserId) {
            activeGame = foundGame; Debug.Log("Game found..."); 

            foundGame.players[1] = new PlayerGameData(data.displayName, user.UserId, data.ships);
            foundGame.activeGame = true;
            await SaveManager.SaveObject($"games/{foundGame.gameID}", foundGame);

            return foundGame;
        }

        Debug.Log("No open game found...");

        foundGame = await GameFinder.CreateGame();
        if (foundGame != null) { activeGame = foundGame; return foundGame; }

        //No game could be found and something went wrong whilst creating a new game.
        Debug.Log("Something went wrong whilst matchmaking...");
        return null;
    }
    #endregion


    //Load the current user's data from database.
    static async Task LoadUserData()
    {
        userPath = $"users/{FirebaseAuth.DefaultInstance.CurrentUser.UserId}";
        data =  await SaveManager.LoadObject<UserData>(userPath);
    }

    public static async Task<bool> SaveUserData()
    {
        userPath = $"users/{FirebaseAuth.DefaultInstance.CurrentUser.UserId}";
        return (await SaveManager.SaveObject(userPath, data));
    }

    public static async Task LoadGameData()
    {
        string gamePath = $"games/{activeGame.gameID}";
        activeGame = await SaveManager.LoadObject<GameData>(gamePath);
    }

    public static async Task SaveGameData()
    {
        string gamePath = $"games/{activeGame.gameID}";
        await SaveManager.SaveObject(gamePath, activeGame);
    }
}
