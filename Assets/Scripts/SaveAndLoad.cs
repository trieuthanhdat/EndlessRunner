using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

/// <summary>
/// Saves, loads and deletes all data in the game
/// </summary>
/// <typeparam name="T"></typeparam>
public static class SaveAndLoad<T>
{
    public static void Save(T data, string folder, string file)
    {
        string dataPath = GetFilePath(folder, file);

        string jsonData = JsonUtility.ToJson(data, true);
        byte[] byteData = Encoding.UTF8.GetBytes(jsonData);

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(dataPath));
            File.WriteAllBytes(dataPath, byteData);
            Debug.Log("SAVEANDLOAD: Save data to: " + dataPath);
        }
        catch (Exception e)
        {
            Debug.LogError("SAVEANDLOAD: Failed to save data to: " + dataPath);
            Debug.LogError("SAVEANDLOAD: Error " + e.Message);
        }
    }

    public static T Load(string folder, string file)
    {
        string dataPath = GetFilePath(folder, file);

        if (!Directory.Exists(Path.GetDirectoryName(dataPath)))
        {
            Debug.LogWarning("SAVEANDLOAD: File or path does not exist! " + dataPath);
            return default(T);
        }

        try
        {
            byte[] jsonDataAsBytes = File.ReadAllBytes(dataPath);
            string jsonData = Encoding.UTF8.GetString(jsonDataAsBytes);
            T returnedData = JsonUtility.FromJson<T>(jsonData);
            Debug.Log("SAVEANDLOAD: Loaded all data from: " + dataPath);
            return returnedData;
        }
        catch (Exception e)
        {
            Debug.LogWarning("SAVEANDLOAD: Failed to load data from: " + dataPath);
            Debug.LogWarning("SAVEANDLOAD: Error: " + e.Message);
            return default(T);
        }
    }

    /// <summary>
    /// Create file path for where a file is stored on the specific platform given a folder name and file name
    /// </summary>
    /// <param name="FolderName"></param>
    /// <param name="FileName"></param>
    /// <returns></returns>
    private static string GetFilePath(string FolderName, string FileName = "")
    {
        string filePath = "";

#if UNITY_EDITOR
    filePath = Path.Combine(Application.dataPath, "StreamingAssets", "data", FolderName);

    if (FileName != "")
        filePath = Path.Combine(filePath, FileName + ".txt");

#else
        // Handle other platforms as before
#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
        // mac
        filePath = Path.Combine(Application.persistentDataPath, "data", FolderName);

        if (FileName != "")
            filePath = Path.Combine(filePath, FileName + ".txt");

#elif UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
        // windows
        filePath = Path.Combine(Application.persistentDataPath, "data", FolderName);
        if (FileName != "")
            filePath = Path.Combine(filePath, FileName + ".txt");

#elif UNITY_ANDROID
        // android
        filePath = Path.Combine(Application.persistentDataPath, "data", FolderName);

        if (FileName != "")
            filePath = Path.Combine(filePath, FileName + ".txt");

#elif UNITY_IOS
        // ios
        filePath = Path.Combine(Application.persistentDataPath, "data", FolderName);

        if (!string.IsNullOrEmpty(FileName))
            filePath = Path.Combine(filePath, FileName + ".txt");
#endif

#endif

        return filePath;
    }
}