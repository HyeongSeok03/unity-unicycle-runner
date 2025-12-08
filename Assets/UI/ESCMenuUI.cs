using UnityEngine;

public class ESCMenuUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;

    public bool isShown = false;
    
    private float _originalTimeScale;
    
    public void Show()
    {
        if (GameManager.instance.isGameOver) return;
        
        _originalTimeScale = Time.timeScale;
        Time.timeScale = 0;
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        isShown = true;
    }

    public void Resume()
    {
        Time.timeScale = _originalTimeScale;
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        isShown = false;
    }
    
    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        MusicPlayer.PlayMenuMusic();
    }
}
