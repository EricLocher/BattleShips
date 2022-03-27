using UnityEngine;
using TMPro;
using SaveData;
using Firebase.Database;
using UnityEngine.SceneManagement;

public class PrivateGame : MonoBehaviour
{
    [SerializeField] TMP_Text information;
    [SerializeField] TMP_InputField id;

    public async void CreateGame()
    {
        if(id.text == "") { information.text = "You need to enter a id."; return; }
        if(id.text.Length < 8) { information.text = "The id must be 8 characters or longer"; return; }
        information.text = $"Creating a private game with id: {id.text}";

        GameData foundGame = await GameFinder.CreateGameWithID(id.text);
        if(foundGame == null) { information.text = "Something went wrong..."; return; }
        User.activeGame = foundGame;
        information.text = $"A Game was created witht the id: {id.text} \n Waiting for a player to join...";

        SaveManager.db.GetReference($"games/{User.activeGame.gameID}/activeGame").ValueChanged += GameStarted;
    }

    public async void GameStarted(object sender, ValueChangedEventArgs args)
    {
        if (!(bool)args.Snapshot.Value) { return; }
        information.text = "Both players have joined! Starting game...";
        await User.LoadGameData();
        SceneManager.LoadScene("Game");

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
