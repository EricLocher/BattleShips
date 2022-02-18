using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour
{
    [SerializeField] Board _board;
    [SerializeField] Camera _mainCamera;
    [SerializeField] int boardSize = 11;
    Board _myBoard, _opponentBoard;

    public void Start()
    {
        transform.position = Vector2.zero;
        Init();
    }

    public void Init()
    {
        _myBoard = Instantiate(_board, transform);
        _opponentBoard = Instantiate(_board, transform);

        _myBoard.Init(Vector2.zero, boardSize);
        _opponentBoard.Init(new Vector2(0, 50), boardSize);

        GameController.OnStateChangeEvent += ChangeBoard;
        Mouse.OnMouseClickEvent += MouseClick;
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

    void MouseClick()
    {
        CheckMousePos();
        if (Mouse.hoveringOver == null) { return; } 

        Vector2 relativePos = Mouse.mousePosition - (Vector2)Mouse.hoveringOver.transform.position;

        if (GameController.gameState == GameStates.Attack && Mouse.hoveringOver == _opponentBoard) {
            if (Mouse.hoveringOver.AttackCell(relativePos, Mouse._currentCursor)) {
                GameController.NextTurn();
            }
        }
        else if (GameController.gameState == GameStates.PlaceShips && Mouse.hoveringOver == _myBoard) {
            if (Mouse.selectedShip != null) {
                if (Mouse.hoveringOver.PlaceShip(relativePos, Mouse.selectedShip)) {
                    Mouse.selectedShip = null;
                }
            }
        }
    }

    void ChangeBoard(GameStates state)
    {
        Vector3 _cameraPos = new Vector3(boardSize / 2, boardSize / 2, -10);

        if (state == GameStates.PlaceShips) { _cameraPos += _myBoard.transform.position; }
        else if(state == GameStates.Attack) { _cameraPos += _opponentBoard.transform.position; }

        _mainCamera.transform.position = _cameraPos;
    }


    void OnDestroy()
    {
        GameController.OnStateChangeEvent -= ChangeBoard;
        Mouse.OnMouseClickEvent -= MouseClick;
    }
}
