using SaveData;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public static class GameFinder
{
    public static async Task<GameData> FindGame()
    {
        List<GameData> games = await SaveManager.LoadMultipleObjects<GameData>("games/");

        if (games == null) { Debug.Log("No games found..."); return null; }

        foreach (GameData game in games) {

            //TODO: Add all intended functionality

            return game;
        }

        Debug.Log("No open games found...");
        return null;

    }

    public static void FindGame(string gameID)
    {
        throw new System.NotImplementedException();
    }

    public static async Task<GameData> CreateGame()
    {
        Debug.Log("Creating Game...");

        string key = SaveManager.db.RootReference.Child("games/").Push().Key;
        GameData game = new GameData(key);
        game.players[0] = new PlayerGameData(User.data.displayName);


        if (!await SaveManager.SaveObject($"games/{game.gameID}", game)) { return null; }

        return game;
    }
}