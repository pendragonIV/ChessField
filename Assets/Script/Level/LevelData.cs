using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Level/LevelData")]
public class LevelData : ScriptableObject
{
    [SerializeField]
    private List<Level> levels = new List<Level>();
    [SerializeField]
    private int currentLevelIndex;

    public void SetCurrentLevelIndex(int index)
    {
        currentLevelIndex = index;
    }

    public int GetCurrentLevelIndex()
    {
        return currentLevelIndex;
    }

    public Level GetLevelAt(int index)
    {
        return levels[index];
    }

    public List<Level> GetLevels()
    {
        return levels;
    }

    public void SetLevelData(int levelIndex, float time)
    {
        levels[levelIndex].bestTime = time;
    }

    #region Save and Load
    public void SaveDataJSON()
    {
        string content = JsonUtility.ToJson(this, true);
        WriteFile(content);
    }

    public void LoadDataJSON()
    {
        string content = ReadFile();
        if (content != null)
        {
            JsonUtility.FromJsonOverwrite(content, this);
        }
    }

    private void WriteFile(string content)
    {
        FileStream file = new FileStream(Application.persistentDataPath + "/Levels.json", FileMode.Create);

        using (StreamWriter writer = new StreamWriter(file))
        {
            writer.Write(content);
        }
    }

    private string ReadFile()
    {
        if (File.Exists(Application.persistentDataPath + "/Levels.json"))
        {
            FileStream file = new FileStream(Application.persistentDataPath + "/Levels.json", FileMode.Open);

            using (StreamReader reader = new StreamReader(file))
            {
                return reader.ReadToEnd();
            }
        }
        else
        {
            return null;
        }
    }
    #endregion
}

[System.Serializable]
public class Level
{
    public Grid map;
    public List<Chess> chesses;
    public float time;
    public float bestTime;
}

[System.Serializable]
public class Chess
{
    public GameObject chessPrefab;
    public Vector3Int position;
}

public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}


