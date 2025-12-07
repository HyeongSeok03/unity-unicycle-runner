using UnityEngine;


public class GameManager : MonoBehaviour
{
    [SerializeField] private GameOverUI gameOverUI;
    
    public static GameManager instance;
    
    private void Awake()
    {
        instance = this;
    }
    
    public void GameOver()
    {
        Time.timeScale = 0.5f;
        gameOverUI.Show();
    }
}
