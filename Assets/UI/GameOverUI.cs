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
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }
    
    private void OnShowComplete()
    {
        Time.timeScale = 0;
    }
    
    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
    }
}
