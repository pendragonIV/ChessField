using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public LevelData levelData;
    public int currentLevelIndex;

    private void Awake()
    {
        if (instance != this && instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }

        levelData.LoadDataJSON();
        currentLevelIndex = levelData.GetCurrentLevelIndex();
    }
    private void OnApplicationQuit()
    {
        levelData.SaveDataJSON();
    }
}
