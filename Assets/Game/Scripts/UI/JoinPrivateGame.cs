using UnityEngine;
using TMPro;
using SaveData;
using Firebase.Database;
using UnityEngine.SceneManagement;

public class JoinPrivateGame : MonoBehaviour
{
    [SerializeField] TMP_Text information;
    [SerializeField] TMP_InputField id;

    public async void JoinGame()
    {
        if (id.text == "") { information.text = "You need to enter a id."; return; }
        if (id.text.Length < 8) { information.text = "The id must be 8 characters or longer"; return; }
        information.text = $"Attempting to join a private game using the id: {id.text}";
        
        if(await GameFinder.JoinGameWithID(id.text)) {
            information.text = "Found game, joining...";
            SceneManager.LoadScene("Game");
            return;
        }

        information.text = "Something went wrong...";
    }

}
