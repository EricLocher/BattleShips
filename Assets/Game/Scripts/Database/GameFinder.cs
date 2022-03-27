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

            return game;
        }

        return null;
    }

    public static async Task<GameData> CreateGameWithID(string gameID)
    {
        GameData game = new GameData(gameID);
        game.players[0] = new PlayerGameData(User.data.displayName, User.user.UserId, User.data.ships);
        game.privateGame = true;

        if (!await SaveManager.SaveObject($"games/{game.gameID}", game)) { return null; }
        return game;
    }

    public static async Task<bool> JoinGameWithID(string gameID)
    {
        GameData foundGame = await SaveManager.LoadObject<GameData>($"games/{gameID}");

        if (foundGame != null && foundGame.players[0].userID != User.user.UserId) {
            User.activeGame = foundGame; Debug.Log("Game found...");

            foundGame.players[1] = new PlayerGameData(User.data.displayName, User.user.UserId, User.data.ships);
            foundGame.activeGame = true;
            await SaveManager.SaveObject($"games/{foundGame.gameID}", foundGame);

            return true;
        }

        return false;
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