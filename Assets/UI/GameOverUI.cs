using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float duration = 1f;
    
    public void Show()
    {
        canvasGroup.DOFade(1f, duration).OnComplete(OnShowComplete);
    }
    
    private void OnShowComplete()
    {
        Time.timeScale = 0;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }
    
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
    }
}
