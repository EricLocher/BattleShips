using Firebase.Database;
using SaveData;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField] TMP_Text userText, information;

    void Start()
    {
        if (User.Instance == null) { SceneManager.LoadScene("Login"); }
        userText.text = $"{User.data.displayName} \nWins: {User.data.wins}";
    }

    public async void QuickPlay()
    {
        information.text = "Looking for a Game...";

        GameData foundGame = await User.Matchmake();

        if (foundGame == null) { information.text = "Something went wrong, try again..."; return; }

        if (foundGame.activeGame == true) { information.text = "Found Game, joining..."; return; }

        information.text = "No actives games could be found. \nCreated a new Game, waiting for players...";

        SaveManager.db.GetReference($"games/{User.activeGame.gameID}/activeGame").ValueChanged += GameStarted;
    }

    public void GameStarted(object sender, ValueChangedEventArgs args)
    {
        if(!(bool)args.Snapshot.Value) { return; }
        information.text = "Both players have joined! Starting game...";
        SaveManager.db.GetReference($"games/{User.activeGame.gameID}/activeGame").ValueChanged -= GameStarted;
    }

    void OnDestroy()
    {
        SaveManager.db.GetReference($"games/{User.activeGame.gameID}/activeGame").ValueChanged -= GameStarted;
    }

    void OnApplicationQuit()
    {
        SaveManager.db.GetReference($"games/{User.activeGame.gameID}/activeGame").ValueChanged -= GameStarted;
    }
}
