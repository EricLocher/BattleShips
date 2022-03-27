using UnityEngine;
using SaveData;
using Firebase.Database;
using TMPro;
using UnityEngine.SceneManagement;


public class GameController : MonoBehaviour
{
    private static GameController _instance;
    public static GameController Instance { get { return _instance; } }

    public static TurnStates turnState;

    public BoardController boardController;
    public static int userIndex;
    public static int opponentIndex;

    public delegate void OnStateChange(TurnStates state);
    public static event OnStateChange OnStateChangeEvent;

    [SerializeField] TMP_Text t_user, t_opponent;

    void Start()
    {

        if(User.activeGame.players[0].userID == User.user.UserId) {
            userIndex = 0;
            opponentIndex = 1;
        }
        else {
            userIndex = 1;
            opponentIndex = 0;
        }

        t_user.text = User.data.displayName;
        t_opponent.text = User.activeGame.players[opponentIndex].displayName;
        
        turnState = (TurnStates)userIndex;
        SaveManager.db.GetReference($"games/{User.activeGame.gameID}/players/{opponentIndex}/attack").ValueChanged += OnOpponentMove;
        SaveManager.db.GetReference($"games/{User.activeGame.gameID}/players/{opponentIndex}/userID").ValueChanged += OnOpponentLeft;
    }

    public static void NextTurn()
    {
        if (turnState == TurnStates.OpponentTurn) { turnState = TurnStates.MyTurn; }
        else { turnState = TurnStates.OpponentTurn; }

        OnStateChangeEvent?.Invoke(turnState);
    }

    async void OnOpponentMove(object sender, ValueChangedEventArgs args)
    {
        if (turnState == TurnStates.MyTurn) { return; }

        await User.LoadGameData();
        if (User.activeGame.players[opponentIndex].attack != new Vector2Int(-1, -1)) {
            boardController.OpponentAttack(User.activeGame.players[opponentIndex].attack);
            NextTurn();
        }
    }

    async void OnOpponentLeft(object sender, ValueChangedEventArgs args)
    {
        await User.LoadGameData();
        if(User.activeGame.players[opponentIndex].userID != "left") { return; }
        await SaveManager.RemoveObject($"games/{User.activeGame.gameID}");

        User.activeGame = null;
        SceneManager.LoadScene("MainMenu");
    }

    async void OnApplicationQuit()
    {
        User.activeGame.players[userIndex].userID = "left"; 
        await User.SaveGameData();
    }

}

public enum TurnStates
{
    MyTurn,
    OpponentTurn
}