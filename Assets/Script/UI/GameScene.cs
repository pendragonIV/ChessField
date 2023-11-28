using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameScene : MonoBehaviour
{
    [SerializeField]
    private Transform overlayPanel;
    [SerializeField]
    private Transform losePanel;
    [SerializeField]
    private Button replayButton;
    [SerializeField]
    private Button homeButton;
    [SerializeField]
    private Text timeText;
    [SerializeField]
    private Text levelText;
    [SerializeField]
    private Text bestTime;

    public void SetTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60F);
        int seconds = Mathf.FloorToInt(time % 60f);
        timeText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
    }

    public void SetLevel(int levelIndex)
    {
        levelText.text = "Level " + (levelIndex + 1);
    }

    public void ShowLosePanel()
    {
        bestTime.text = LevelManager.instance.levelData.GetLevelAt(LevelManager.instance.currentLevelIndex).bestTime.ToString("00:00");
        overlayPanel.gameObject.SetActive(true);
        losePanel.gameObject.SetActive(true);
        FadeIn(overlayPanel.GetComponent<CanvasGroup>(), losePanel.GetComponent<RectTransform>());
        homeButton.interactable = false;
        replayButton.interactable = false;
    }

    private void FadeIn(CanvasGroup canvasGroup, RectTransform rectTransform)
    {
        canvasGroup.alpha = 0f;
        canvasGroup.DOFade(1, .3f).SetUpdate(true);

        rectTransform.anchoredPosition = new Vector3(0, 500, 0);
        rectTransform.DOAnchorPos(new Vector2(0, 0), .3f, false).SetEase(Ease.OutQuint).SetUpdate(true);
    }
}
