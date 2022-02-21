using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;


public class Board : MonoBehaviour
{
    [SerializeField] int size;
    [SerializeField] Cell _cell;

    public Bounds bounds;

    public Cell[,] cells;
    List<Ship> shipList = new List<Ship>();
    bool mouseOverBoard = false;

    public void Init(Vector2 pos, int size = 10)
    {
        this.size = size;
        transform.position = pos;
        cells = new Cell[size, size];

        var _cellHolder = new GameObject();
        _cellHolder.name = "CellHolder";
        _cellHolder.transform.parent = transform;

        for (int x = 0; x < size; x++) {
            for (int y = 0; y < size; y++) {
                cells[x, y] = Instantiate(_cell, new Vector3(pos.x + x, pos.y + y, 0), Quaternion.identity, _cellHolder.transform);
                cells[x, y].name = $"Cell ({x}, {y})";

                var isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
                cells[x, y].Init(isOffset);
            }
        }

        bounds = new Bounds(new Vector3(pos.x - 0.5f + ((float)size/2), pos.y - 0.5f + ((float)size/2), 0), new Vector3(10, 10, 0));
    }

    public bool AttackCell(Vector2 pos, CursorScriptableObject cursor)
    {
        if(pos.x < 0 || pos.y < 0) { return false; }
        if(pos.x > cells.GetLength(0) || pos.y > (cells.GetLength(1))) { return false; }

        Vector2Int _pos = new Vector2Int((int)pos.x, (int)pos.y);
        if(cells[_pos.x, _pos.y].isDead) { return false; }

        foreach (Vector2Int part in cursor.cursorParts) {
            AttackCell(new Vector2(_pos.x + part.x, _pos.y + part.y));
        }

        return true;
    }

    public bool AttackCell(Vector2 pos)
    {
        if(pos.x < 0 || pos.y < 0) { return false; }
        if(pos.x > cells.GetLength(0) || pos.y > (cells.GetLength(1))) { return false; }

        Vector2Int _pos = new Vector2Int((int)pos.x, (int)pos.y);

        if (cells[_pos.x, _pos.y].isDead) { return false; }

        cells[_pos.x, _pos.y].Hit();
        return true;
    }

    public bool PlaceShip(Vector2 pos, Ship ship)
    {
        Vector2Int _pos = new Vector2Int((int)pos.x, (int)pos.y);

        //if (Mouse.mousePosition.y - (ship.partList.Count - 1) < transform.position.y) { return false; }

        for (int i = 0; i < ship.partList.Count; i++) {
            if (cells[_pos.x, _pos.y - i].occupyingGameObject != null) { Debug.Log(_pos.x + "," + (_pos.y - i)); return false; }
        }

        for (int i = 0; i < ship.partList.Count; i++) {
            if(i == 0) { ship.transform.position = cells[_pos.x, _pos.y - i].transform.position; }


            cells[_pos.x, _pos.y - i].occupyingGameObject = ship.partList[i];
        }

        shipList.Add(ship);
        ship.transform.parent = transform;
        ship.pos = _pos;
        return true;
    }



}
