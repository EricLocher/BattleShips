using System;
using System.Collections.Generic;
using UnityEngine;

namespace SaveData
{
    [Serializable]
    public struct UserData
    {
        public string displayName;
        public int wins;

        public UserData(string displayName)
        {
            this.displayName = displayName;
            wins = 0;
        }
        public override string ToString() => $"Player: (displayName: {displayName}, wins: {wins})";
    }

    [Serializable]
    public class GameData
    {
        public string gameID;
        public PlayerGameData[] players;
        public bool activeGame;

        public GameData(string gameID)
        {
            this.gameID = gameID;
            this.activeGame = false;
            this.players = new PlayerGameData[2];
            this.players[0] = new PlayerGameData("empty");
            this.players[1] = new PlayerGameData("empty");
        }

        public override string ToString() => $"Game: (gameID: {gameID}, activeGame {activeGame}, player1: {players[0].displayName}, player2 {players[1].displayName})";
    }

    [Serializable]
    public struct PlayerGameData
    {
        public string displayName;
        public Vector2Int attack;

        public PlayerGameData(string displayName)
        {
            this.displayName = displayName;
            attack = Vector2Int.zero;
        }

        public override string ToString() => $"PlayerGameData: (displayName: {displayName}, doneTurn: {attack})";
    }

}
