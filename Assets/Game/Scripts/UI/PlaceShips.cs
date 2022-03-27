using SaveData;
using UnityEngine;
using TMPro;

public class PlaceShips : MonoBehaviour
{
    [SerializeField] Board board;

    Vector2Int oldShipPos = Vector2Int.zero;

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
            _ship.direction = ship.direction;
            if (!board.PlaceShip(ship.pos, _ship)) { Debug.LogError($"Ship couldn't be placed on the board! {ship.type}, {ship.pos}"); }
            board.shipList.Add(_ship);
        }
    }

    void Update()
    {
        CheckMousePos();

        if (Input.GetKeyDown(KeyCode.R)) {
            RotateShip();
        }
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
        if (Mouse.selectedShip != null) { PlaceShip(pos); return; }

        Vector2Int _pos = new Vector2Int((int)(pos.x), (int)(pos.y));
        if (board.cells[_pos.x, _pos.y].shipPart == null) { return; }

        Ship _ship = board.cells[_pos.x, _pos.y].shipPart.GetComponentInParent<Ship>();

        for (int i = 0; i < _ship.partList.Count; i++) {
            _ship.partList[i].occupyingCell.shipPart = null;
            _ship.partList[i].occupyingCell = null;
        }

        oldShipPos = _pos;

        _ship.transform.parent = null;
        Mouse.selectedShip = _ship;
    }

    void PlaceShip(Vector2 pos)
    {
        if (board.PlaceShip(pos, Mouse.selectedShip)) {
            Mouse.selectedShip = null;
            oldShipPos = Vector2Int.zero;
        }
    }

    public void RotateShip()
    {
        if(Mouse.selectedShip == null) { return; }
        if(Mouse.selectedShip.direction == Direction.Vertical) { Mouse.selectedShip.direction = Direction.Horizontal; }
        else { Mouse.selectedShip.direction = Direction.Vertical; }

        Mouse.selectedShip.UpdateRotation();
    }

    public async void SaveShips()
    {
        if (Mouse.selectedShip != null) { PlaceShip(oldShipPos); }
        oldShipPos = Vector2Int.zero;

        for (int i = 0; i < board.shipList.Count; i++) {
            User.data.ships[i] = (ShipData)board.shipList[i];
        }

        await User.SaveUserData();
    }

    void OnDestroy()
    {
        Mouse.OnMouseClickEvent -= MouseClick;
    }
}
