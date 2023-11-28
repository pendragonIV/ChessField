using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    public SceneChanger sceneChanger;
    public GameScene gameScene;
    public Transform chessesContainer;
    #region Game status
    [SerializeField]
    private bool isGameWin = false;
    [SerializeField]
    private bool isLose = false;
    private float timeLeft;
    private Level currentLevelData;
    #endregion

    private void OnEnable()
    {
        SetupLevel();
    }

    private void Start()
    {
        Time.timeScale = 1;
    }

    private void Update()
    {
        if (!isGameWin && !isLose)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft <= 0)
            {
                timeLeft = 0;
            }
            gameScene.SetTime(timeLeft);
            if (timeLeft <= 0)
            {
                Lose();
            }
        }
    }

    private void SetupLevel()
    {
        currentLevelData = LevelManager.instance.levelData.GetLevelAt(LevelManager.instance.currentLevelIndex);
        LevelManager.instance.levelData.SetCurrentLevelIndex(LevelManager.instance.currentLevelIndex);
        Grid map = Instantiate(currentLevelData.map);
        GridCellManager.instance.SetMap(map);
        timeLeft = currentLevelData.time;
        gameScene.SetLevel(LevelManager.instance.currentLevelIndex);
        gameScene.SetTime(timeLeft);
        InitChesses(currentLevelData.chesses);
    }

    private void InitChesses(List<Chess> chesses)
    {
        foreach (Chess chess in chesses)
        {
            GameObject chessObj = Instantiate(chess.chessPrefab, chessesContainer);
            chessObj.transform.position = GridCellManager.instance.PositonToMove(chess.position);
        }
    }

    public void Win()
    {
        isGameWin = true;
        if(timeLeft > currentLevelData.bestTime)
        {
            LevelManager.instance.levelData.SetLevelData(LevelManager.instance.currentLevelIndex, timeLeft);
        }
        StartCoroutine(WaitToWin());
        //LevelManager.instance.levelData.SaveDataJSON();
    }

    private IEnumerator WaitToWin()
    {
        yield return new WaitForSecondsRealtime(1f);
        sceneChanger.ChangeToNextLevel();
    }

    public void Lose()
    {
        isLose = true;
        Time.timeScale = 0;
        gameScene.ShowLosePanel();
    }

    public bool IsGameWin()
    {
        return isGameWin;
    }

    public bool IsGameLose()
    {
        return isLose;
    }
}

