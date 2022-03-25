using System;
using System.Collections.Generic;
using UnityEngine;


public class Board : MonoBehaviour
{
    [SerializeField] Cell _cell;
    [SerializeField] GameObject inactive;

    public bool activeBoard
    {
        set { inactive.SetActive(value); }
    }

    public Bounds bounds;
    public bool deadBoard
    {
        get {
            foreach (Ship _ship in shipList) {
                foreach (ShipPart _part in _ship.partList) {
                    if (!_part.occupyingCell.isDead) { return false; }
                }
            }
            return true;
        }
    }

    public Cell[,] cells;
    public List<Ship> shipList = new List<Ship>();

    public void Init(Vector2 pos, int size = 10)
    {
        transform.position = pos;
        cells = new Cell[size, size];
        inactive.transform.localScale = new Vector2(size, size);

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

        bounds = new Bounds(new Vector3(pos.x - 0.5f + ((float)size / 2), pos.y - 0.5f + ((float)size / 2), 0), new Vector3(10, 10, 0));
    }

    public bool AttackCell(Vector2 pos, CursorScriptableObject cursor)
    {
        if (pos.x < 0 || pos.y < 0) { return false; }
        if (pos.x > cells.GetLength(0) || pos.y > (cells.GetLength(1))) { return false; }

        Vector2Int _pos = new Vector2Int(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y));
        if (cells[_pos.x, _pos.y].isDead) { return false; }

        foreach (Vector2Int part in cursor.cursorParts) {
            AttackCell(new Vector2(_pos.x + part.x, _pos.y + part.y));
        }

        return true;
    }

    public bool AttackCell(Vector2 pos)
    {
        if (pos.x < 0 || pos.y < 0) { return false; }
        if (pos.x > cells.GetLength(0) || pos.y > (cells.GetLength(1))) { return false; }

        Vector2Int _pos = new Vector2Int(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y));

        if (cells[_pos.x, _pos.y].isDead) { return false; }

        cells[_pos.x, _pos.y].Hit();
        return true;
    }

    public bool PlaceShip(Vector2 pos, Ship ship)
    {
        Vector2Int _pos = new Vector2Int(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y));

        try {
            for (int i = 0; i < ship.partList.Count; i++) {

                Vector2Int posToCheck = (ship.direction == Direction.Vertical) ? new Vector2Int(_pos.x, _pos.y - i) : new Vector2Int(_pos.x + i, _pos.y);

                if (cells[posToCheck.x, posToCheck.y].occupyingGameObject != null) { Debug.Log(posToCheck); return false; }
            }

            for (int i = 0; i < ship.partList.Count; i++) {
                Vector2Int posToCheck = (ship.direction == Direction.Vertical) ? new Vector2Int(_pos.x, _pos.y - i) : new Vector2Int(_pos.x + i, _pos.y);

                if (i == 0) { ship.transform.position = cells[posToCheck.x, posToCheck.y].transform.position; }

                cells[posToCheck.x, posToCheck.y].occupyingGameObject = ship.partList[i].gameObject;
                ship.partList[i].occupyingCell = cells[posToCheck.x, posToCheck.y];
            }
        }
        catch (Exception e) {
            Debug.LogWarning(e.ToString());
            return false;
        }

        ship.transform.parent = transform;
        ship.pos = _pos;
        return true;
    }
}
