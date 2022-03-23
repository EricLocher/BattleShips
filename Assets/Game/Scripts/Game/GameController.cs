using UnityEngine;
using SaveData;


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

        //ShipList.list[(int)ship.type];
    }

    public static void ChangeState(TurnStates state)
    {
        turnState = state;
        OnStateChangeEvent?.Invoke(state);
    }

    public static void NextTurn()
    {
        if (turnState == TurnStates.OpponentTurn) { turnState = TurnStates.MyTurn; }
        else { turnState = TurnStates.OpponentTurn; }
    }

}

public enum TurnStates
{
    MyTurn,
    OpponentTurn
}