using SaveData;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public static class GameFinder
{
    public static async Task<GameData> FindGame()
    {
        List<GameData> games = await SaveManager.LoadMultipleObjects<GameData>("games/");

        if (games == null) { return null; }

        foreach (GameData game in games) {
            if (game.privateGame || game.activeGame) { continue; }

            //TODO: Check if game has an 'empty' player slot.

            //Return the first 'open' game found.
            return game;
        }

        return null;

    }

    public static void FindGame(string gameID)
    {
        //TODO: Join the game with the given gameID.
        throw new System.NotImplementedException();
    }

    public static async Task<GameData> CreateGame()
    {
        Debug.Log("Creating Game...");

        //Generate a new unique gameID
        string key = SaveManager.db.RootReference.Child("games/").Push().Key;
        GameData game = new GameData(key);
        game.players[0] = new PlayerGameData(User.data.displayName, User.user.UserId, User.data.ships);

        //Save the created game to db.
        if (!await SaveManager.SaveObject($"games/{game.gameID}", game)) { return null; }

        return game;
    }
}