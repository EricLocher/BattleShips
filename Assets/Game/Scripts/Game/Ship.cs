using System.Collections.Generic;
using UnityEngine;
using SaveData;

public class Ship : MonoBehaviour
{
    public Vector2Int pos = Vector2Int.zero;
    public List<ShipPart> partList;
    public Direction direction = Direction.Vertical;
    public ShipType shipType;
    public Bounds bounds;

    public static explicit operator ShipData(Ship ship)
    {
        return new ShipData(ship.pos, ship.direction, ship.shipType);
    }
    void Start()
    {
        UpdateRotation();
        bounds = new Bounds(new Vector2(transform.position.x, transform.position.y - partList.Count/2), new Vector2(1, partList.Count));
    }

    void Update()
    {
        bounds.center = new Vector2(transform.position.x, transform.position.y - partList.Count / 2);
    }

    public void UpdateRotation()
    {
        if(direction == Direction.Vertical) { transform.eulerAngles = new Vector3(0, 0, 0); }
        else { transform.eulerAngles = new Vector3(0, 0, 90); }
    }

    public void Flip()
    {
        if(direction == Direction.Vertical) { direction = Direction.Horizontal; }
        else { direction = Direction.Vertical; }

        UpdateRotation();
    }
}

public enum Direction
{
    Vertical,
    Horizontal
}

public enum ShipType
{
    Carrier,
    Battleship,
    Cruiser,
    Destroyer,
    Submarine
}