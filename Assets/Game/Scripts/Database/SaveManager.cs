using Firebase;
using Firebase.Auth;
using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private static SaveManager _instance;
    public static SaveManager Instance { get { return _instance; } }

    public static FirebaseAuth auth;
    public static FirebaseDatabase db;
    public static FirebaseUser user = null;

    void Awake()
    {
        if (_instance == null) {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(this.gameObject);
        }

        db = FirebaseDatabase.DefaultInstance;
    }

    private void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            if (task.Exception != null) { Debug.LogError(task.Exception.ToString()); return; }
            auth = FirebaseAuth.DefaultInstance;
        });
    }

    #region Load/Save Data

    /// <summary>
    /// Load an object from the database.
    /// </summary>
    /// <typeparam name="T">Object Type</typeparam>
    /// <param name="path">Database Path</param>
    /// <returns>Object of type (T)</returns>

    public async static Task<T> LoadObject<T>(string path)
    {
        string jsonData = await db.RootReference.Child(path).GetValueAsync().ContinueWith(task => {

            if (task.Exception != null) {
                Debug.LogWarning(task.Exception);
                return null;
            }

            return task.Result.GetRawJsonValue();
        });

        try {
            return JsonUtility.FromJson<T>(jsonData);
        }
        catch (Exception e) {
            Debug.LogError(e.ToString());
            return default(T);
        }

    }

    /// <summary>
    /// Load a list of objects from the database.
    /// </summary>
    /// <typeparam name="T">Object Type</typeparam>
    /// <param name="path">Database Path</param>
    /// <returns>List of objects of the type (T)</returns>
    public async static Task<List<T>> LoadMultipleObjects<T>(string path)
    {

        List<string> jsonData = await db.RootReference.Child(path).GetValueAsync().ContinueWith(task => {

            if (task.Exception != null) {
                Debug.LogWarning(task.Exception);
                return null;
            }

            List<string> jsonData = new List<string>();

            foreach (var item in task.Result.Children) {
                jsonData.Add(item.GetRawJsonValue());
            }

            return jsonData;

        });

        if (jsonData == null) { return null; }

        List<T> objectList = new List<T>();

        foreach (var item in jsonData) {
            objectList.Add(JsonUtility.FromJson<T>(item));
        }

        return objectList;
    }

    /// <summary>
    /// Save an object to the database.
    /// </summary>
    /// <param name="path">Database Path</param>
    /// <param name="data">Object to save</param>
    /// <returns>Bool representing the result of the operation</returns>
    public async static Task<bool> SaveObject<T>(string path, T data)
    {
        Debug.Log("Saving Data...");

        string jsonData = JsonUtility.ToJson(data);

        return await db.RootReference.Child(path).SetRawJsonValueAsync(jsonData).ContinueWith(task => {
            if (task.Exception != null) {
                Debug.LogWarning(task.Exception);
                return false;
            }

            return true;
        });
    }

    #endregion
}


