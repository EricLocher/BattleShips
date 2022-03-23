using System;
using UnityEngine;

namespace SaveData
{
    [Serializable]
    public struct UserData
    {
        public string displayName;
        public int wins;
        public ShipData[] ships;

        public UserData(string displayName)
        {
            this.displayName = displayName;
            wins = 0;
            //TODO: Make less ugly - perhaps move to a json file format.
            //Assign the 'default' ship placements
            ships = new ShipData[7];
            ships[0] = new ShipData(new Vector2Int(9, 4), Direction.Vertical, ShipType.Carrier);
            ships[1] = new ShipData(new Vector2Int(8, 3), Direction.Vertical, ShipType.Battleship);
            ships[2] = new ShipData(new Vector2Int(7, 2), Direction.Vertical, ShipType.Cruiser);
            ships[3] = new ShipData(new Vector2Int(6, 1), Direction.Vertical, ShipType.Destroyer);
            ships[4] = new ShipData(new Vector2Int(5, 1), Direction.Vertical, ShipType.Destroyer);
            ships[5] = new ShipData(new Vector2Int(4, 0), Direction.Vertical, ShipType.Submarine);
            ships[6] = new ShipData(new Vector2Int(3, 0), Direction.Vertical, ShipType.Submarine);
        }

        public override string ToString() => $"Player: (displayName: {displayName}, wins: {wins})";
    }

    [Serializable]
    public class GameData
    {
        public string gameID;
        public bool privateGame;
        public bool activeGame;
        public PlayerGameData[] players;

        public GameData(string gameID)
        {
            this.gameID = gameID;
            this.privateGame = false;
            this.activeGame = false;
            this.players = new PlayerGameData[2];
            //TODO: Rethink this, perhaps change "PlayerGameData" to class/object instead of a struct
            this.players[0] = new PlayerGameData("empty", "noID", new ShipData[0]);
            this.players[1] = new PlayerGameData("empty", "noID", new ShipData[0]);

        }

        public override string ToString() => $"Game: (gameID: {gameID}, activeGame {activeGame}, player1: {players[0].displayName}, player2 {players[1].displayName})";
    }

    [Serializable]
    public struct PlayerGameData
    {
        public string userID;
        public string displayName;
        public ShipData[] ships;
        public Vector2Int attack;

        public PlayerGameData(string displayName, string userID, ShipData[] ships)
        {
            this.userID = userID;
            this.displayName = displayName;
            this.ships = ships;
            attack = Vector2Int.zero;
        }

        public override string ToString() => $"PlayerGameData: (displayName: {displayName}, doneTurn: {attack})";
    }

    [Serializable]
    public struct ShipData
    {
        public Vector2Int pos;
        public Direction direction;
        public ShipType type;

        public ShipData(Vector2Int pos, Direction direction, ShipType type)
        {
            this.pos = pos;
            this.direction = direction;
            this.type = type;
        }
    }
}
