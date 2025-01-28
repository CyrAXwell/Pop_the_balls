using System.Runtime.InteropServices;
using DG.Tweening;
using UnityEngine;

public class HUD : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void ShowFullscreenAdv();
    
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject OnSoundButton;
    [SerializeField] private GameObject OffSoundButton;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private float fadeTime = 1f;
    [SerializeField] private CanvasGroup transition;
    [SerializeField] private GameField gameField;
    [SerializeField] private HighScoreView highScoreView;
    [SerializeField] private CanvasGroup losePanel;
    [SerializeField] private HighScoreView losePanelHighScore;
    [SerializeField] private ScoreView scoreView;


    private CanvasGroup _canvasGroupSettingsPanel => settingsPanel.GetComponent<CanvasGroup>();

    private void Awake()
    {
        settingsPanel.SetActive(false);
        losePanel.gameObject.SetActive(false);
    }

    public void OpenSettingsPanel()
    {
        settingsPanel.SetActive(true);
        highScoreView.DisplayScore();
        _canvasGroupSettingsPanel.alpha = 0;
        _canvasGroupSettingsPanel.DOFade(1, fadeTime);
    }

    public void CloseSettingsPanel()
    {
        _canvasGroupSettingsPanel.DOFade(0, fadeTime).OnComplete(() => settingsPanel.SetActive(false));
    }

    public void OnSound()
    {
        audioManager.UnMuteSFX();
        OnSoundButton.SetActive(false);
        OffSoundButton.SetActive(true);
    }

    public void OffSound()
    {
        audioManager.MuteSFX();
        OnSoundButton.SetActive(true);
        OffSoundButton.SetActive(false);
    }

    public void ResetTransition()
    {
        #if UNITY_WEBGL && !UNITY_EDITOR
        ShowFullscreenAdv();
        #endif
        transition.gameObject.SetActive(true);
        transition.alpha = 0;
        transition.DOFade(1, fadeTime).OnComplete(()=>
        {
            gameField.ResetField();
            transition.DOFade(0, fadeTime).OnComplete(()=>
            {
                transition.gameObject.SetActive(false);
                gameField.AppearBalls();
            });
        });
    }

    public void OpenLosePanel()
    {
        losePanel.gameObject.SetActive(true);
        losePanelHighScore.DisplayScore();
        scoreView.DisplayScoreOnLosePanel();
        losePanel.alpha = 0;
        losePanel.DOFade(1, fadeTime);
    }

    public void OnContinueButton()
    {   
        #if UNITY_WEBGL && !UNITY_EDITOR
        ShowFullscreenAdv();
        #endif
        losePanel.gameObject.SetActive(true);
        gameField.ResetField();
        losePanel.DOFade(0, fadeTime).OnComplete(()=>
        {
            losePanel.gameObject.SetActive(false);
            gameField.AppearBalls();
        });
    }
}
