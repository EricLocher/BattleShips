using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField] TMP_Text userText, information;

    private void Start()
    {
        if (User.Instance == null) { SceneManager.LoadScene("Login"); }
        userText.text = $"{User.data.displayName} \nWins: {User.data.wins}";
    }

    public async void QuickPlay()
    {
        information.text = "Looking for a Game...";

        if (!await User.Matchmake()) { Debug.Log("Something went wrong whilst looking for a game..."); return; }
        information.text = "Found Game, joining...";
        return;
    }
}
