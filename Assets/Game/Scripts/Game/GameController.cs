using UnityEngine;
using SaveData;
using Firebase.Database;


public class GameController : MonoBehaviour
{
    private static GameController _instance;
    public static GameController Instance { get { return _instance; } }

    public static TurnStates turnState;

    public static int userIndex;
    public static int opponentIndex;

    public delegate void OnStateChange(TurnStates state);
    public static event OnStateChange OnStateChangeEvent;

    void Awake()
    {
        if (_instance == null) {
            _instance = this;
        }
        else {
            Destroy(this.gameObject);
        }
    }

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

        //ShipList.list[(int)ship.type];
    }

    public static void NextTurn()
    {
        if (turnState == TurnStates.OpponentTurn) { turnState = TurnStates.MyTurn; }
        else { turnState = TurnStates.OpponentTurn; }

        OnStateChangeEvent?.Invoke(turnState);
    }

    void OnOpponentMove(object sender, ValueChangedEventArgs args)
    {
        if(turnState == TurnStates.MyTurn) { return; }
        NextTurn();
    }

}

public enum TurnStates
{
    MyTurn,
    OpponentTurn
}