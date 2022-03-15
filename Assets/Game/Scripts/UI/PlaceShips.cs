using SaveData;
using UnityEngine;

public class PlaceShips : MonoBehaviour
{
    [SerializeField] Board board;

    void Start()
    {
        board.Init(new Vector2(-4.5f, -4.5f));
        LoadShips();
        Mouse.OnMouseClickEvent += MouseClick;
    }

    void LoadShips()
    {
        foreach (ShipData ship in User.data.ships) {
            Ship _ship = Instantiate(ShipList.list[(int)ship.type]);
            if (!board.PlaceShip(ship.pos, _ship)) { Debug.LogError($"Ship couldn't be placed on the board! {ship.type}, {ship.pos}"); }
        }
    }

    void Update()
    {
        CheckMousePos();
    }

    void MouseClick()
    {
        if (Mouse.hoveringOver != board) { return; }

        Vector2 relativePos = Mouse.mousePosition - (Vector2)Mouse.hoveringOver.transform.position + new Vector2(0.5f, 0.5f);
        MoveShip(relativePos);
    }

    void CheckMousePos()
    {
        if (board.bounds.Contains(Mouse.mousePosition)) {
            Mouse.hoveringOver = board;
            return;
        }
        Mouse.hoveringOver = null;
    }

    void MoveShip(Vector2 pos)
    {
        if (Mouse.selectedShip != null) { PlaceShip(); return; }

        Vector2Int _pos = new Vector2Int((int)pos.x, (int)pos.y);
        if (board.cells[_pos.x, _pos.y].occupyingGameObject == null) { return; }

        Ship _ship = board.cells[_pos.x, _pos.y].occupyingGameObject.GetComponentInParent<Ship>();

        _ship.transform.parent = null;
        Mouse.selectedShip = _ship;
    }

    void PlaceShip()
    {

    }

    void OnDestroy()
    {
        Mouse.OnMouseClickEvent -= MouseClick;
    }

}