using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    private const string MENU = "MainMenu";
    private const string GAME = "GameScene";
    private const string LEVEL_CHOOSE = "Levels";

    [SerializeField]
    private Transform sceneTransition;

    private void Start()
    {
        PlayTransition();
    }

    public void PlayTransition()
    {
        sceneTransition.GetComponent<Animator>().Play("SceneTransition");
    }

    public void ChangeToMenu()
    {
        StopAllCoroutines();
        StartCoroutine(ChangeScene(MENU));
    }

    public void ChangeToGameScene()
    {
        StopAllCoroutines();
        StartCoroutine(ChangeScene(GAME));
    }

    public void ChangeToNextLevel()
    {
        StopAllCoroutines();
        StartCoroutine(ChangeScene(GAME));

        if (LevelManager.instance.currentLevelIndex < LevelManager.instance.levelData.GetLevels().Count - 1)
        {
            LevelManager.instance.currentLevelIndex++;
            StartCoroutine(ChangeScene(GAME));
        }
        else
        {
            LevelManager.instance.currentLevelIndex = Random.Range(1, LevelManager.instance.levelData.GetLevels().Count);
            StartCoroutine(ChangeScene(GAME));
        }
    }


    private IEnumerator ChangeScene(string sceneName)
    {

        //Optional: Add animation here
        sceneTransition.GetComponent<Animator>().Play("SceneTransitionReverse");
        yield return new WaitForSecondsRealtime(1f);

        SceneManager.LoadSceneAsync(sceneName);

    }
}
