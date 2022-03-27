using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SaveData;

public class BoardController : MonoBehaviour
{
    [SerializeField] Board _board;
    [SerializeField] GameObject userUI, opponentUI;
    int boardSize = 10;
    Board _myBoard, _opponentBoard;

    public void Start()
    {
        transform.position = Vector2.zero;
        Init();
    }

    public void Init()
    {
        _myBoard = Instantiate(_board, transform);
        _myBoard.name = "MyBoard";
        _opponentBoard = Instantiate(_board, transform);
        _opponentBoard.name = "OpponentsBoard";

        _myBoard.Init(new Vector2(-6.7f, 0), boardSize, userUI);
        _opponentBoard.Init(new Vector2(6.7f, 0), boardSize, opponentUI);

        LoadShips(_myBoard, GameController.userIndex);
        LoadShips(_opponentBoard, GameController.opponentIndex, true);

        GameController.OnStateChangeEvent += ChangeBoard;
        Mouse.OnMouseClickEvent += MouseClick;

        ChangeBoard(GameController.turnState);
    }

    void LoadShips(Board board, int playerIndex, bool hideShips = false)
    {
        foreach (ShipData ship in User.activeGame.players[playerIndex].ships) {
            Ship _ship = Instantiate(ShipList.list[(int)ship.type]);
            _ship.direction = ship.direction;

            if (!board.PlaceShip(ship.pos, _ship, hideShips)) { Debug.LogError($"Ship couldn't be placed on the board! {ship.type}, {ship.pos}"); }
            board.shipList.Add(_ship);
        }
    }

    void CheckMousePos()
    {
        if (_myBoard.bounds.Contains(Mouse.mousePosition)) {
            Mouse.hoveringOver = _myBoard;
            return;
        }

        if (_opponentBoard.bounds.Contains(Mouse.mousePosition)) {
            Mouse.hoveringOver = _opponentBoard;
            return;
        }

        Mouse.hoveringOver = null;
    }

    async void MouseClick()
    {
        CheckMousePos();
        if (GameController.turnState != TurnStates.MyTurn) { return; }
        if (Mouse.hoveringOver == null) { return; } 

        Vector2 relativePos = Mouse.mousePosition - (Vector2)Mouse.hoveringOver.transform.position;

        if (Mouse.hoveringOver == _opponentBoard) {
            if (Mouse.hoveringOver.AttackCell(relativePos, Mouse._currentCursor)) {
                Vector2Int _pos = new Vector2Int(Mathf.RoundToInt(relativePos.x), Mathf.RoundToInt(relativePos.y));
                User.activeGame.players[GameController.userIndex].attack = _pos;
                await User.SaveGameData();
                GameController.NextTurn();
                CheckWinCondition();
            }
        }
    }
    public void OpponentAttack(Vector2Int pos)
    {
        _myBoard.AttackCell(pos);
        CheckWinCondition();
    }

    async void CheckWinCondition()
    {
        if (_myBoard.deadBoard) {
            SceneManager.LoadScene("Lose");
        } else if (_opponentBoard.deadBoard) {
            User.data.wins += 1;
            await User.SaveUserData();
            SceneManager.LoadScene("Win");
        }
    }

    void ChangeBoard(TurnStates state)
    {
       if (state == TurnStates.OpponentTurn) { _myBoard.activeBoard = false; _opponentBoard.activeBoard = true; }
       else if(state == TurnStates.MyTurn) { _opponentBoard.activeBoard = false; _myBoard.activeBoard = true; }
    }

    void OnDestroy()
    {
        GameController.OnStateChangeEvent -= ChangeBoard;
        Mouse.OnMouseClickEvent -= MouseClick;
    }
}
