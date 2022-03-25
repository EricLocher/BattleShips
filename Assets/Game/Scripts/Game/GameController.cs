using UnityEngine;
using SaveData;
using Firebase.Database;


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

        turnState = (TurnStates)userIndex;
        SaveManager.db.GetReference($"games/{User.activeGame.gameID}/players/{opponentIndex}/attack").ValueChanged += OnOpponentMove;
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
}

public enum TurnStates
{
    MyTurn,
    OpponentTurn
}