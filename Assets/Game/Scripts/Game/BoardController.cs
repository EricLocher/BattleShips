using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SaveData;

public class BoardController : MonoBehaviour
{
    [SerializeField] Board _board;
    [SerializeField] Camera _mainCamera;
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

        _myBoard.Init(Vector2.zero, boardSize);
        _opponentBoard.Init(new Vector2(0, 50), boardSize);

        LoadShips(_myBoard, GameController.userIndex);
        LoadShips(_opponentBoard, GameController.opponentIndex);

        GameController.OnStateChangeEvent += ChangeBoard;
        Mouse.OnMouseClickEvent += MouseClick;

        ChangeBoard(GameController.turnState);
    }

    void LoadShips(Board board, int playerIndex)
    {
        foreach (ShipData ship in User.activeGame.players[playerIndex].ships) {
            Ship _ship = Instantiate(ShipList.list[(int)ship.type]);
            _ship.direction = ship.direction;

            if (!board.PlaceShip(ship.pos, _ship)) { Debug.LogError($"Ship couldn't be placed on the board! {ship.type}, {ship.pos}"); }
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
                Vector2Int _pos = new Vector2Int((int)relativePos.x, (int)relativePos.y);
                User.activeGame.players[GameController.userIndex].attack = _pos;
                await User.SaveGameData();
                GameController.NextTurn();
            }
        }
    }

    void ChangeBoard(TurnStates state)
    {
        Vector3 _cameraPos = new Vector3(4.5f, (float)boardSize / 2, -10);

        if (state == TurnStates.OpponentTurn) { _cameraPos += _myBoard.transform.position; }
        else if(state == TurnStates.MyTurn) { _cameraPos += _opponentBoard.transform.position; }

        _mainCamera.transform.position = _cameraPos;
    }


    void OnDestroy()
    {
        GameController.OnStateChangeEvent -= ChangeBoard;
        Mouse.OnMouseClickEvent -= MouseClick;
    }
}
