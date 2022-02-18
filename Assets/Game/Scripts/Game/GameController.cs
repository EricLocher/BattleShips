using UnityEngine;

public class GameController : MonoBehaviour
{

    private static GameController _instance;
    public static GameController Instance { get { return _instance; } }

    public static GameStates gameState;
    public static TurnStates turnState;


    public delegate void OnStateChange(GameStates state);
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
        gameState = GameStates.Init;
    }

    //Temp Update Loop
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N)) {
            NextTurn();
        }

        if (Input.GetKeyDown(KeyCode.Y)) {
            ChangeState(GameStates.PlaceShips);
        }
        if (Input.GetKeyDown(KeyCode.U)) {
            ChangeState(GameStates.Attack);
        }
    }

    public static void ChangeState(GameStates state)
    {
        gameState = state;
        OnStateChangeEvent?.Invoke(state);
    }


    public static void NextTurn()
    {
        if (turnState == TurnStates.OpponentTurn) { turnState = TurnStates.MyTurn; }
        else { turnState = TurnStates.OpponentTurn; }
    }


}

public enum GameStates
{
    Init,
    PlaceShips,
    Attack,
    Result
}

public enum TurnStates
{
    OpponentTurn,
    MyTurn
}