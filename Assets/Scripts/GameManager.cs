using UnityEngine;
using UnityEngine.InputSystem;


public class GameManager : MonoBehaviour
{
    [SerializeField] private GameOverUI gameOverUI;
    [SerializeField] private ESCMenuUI escMenuUI;
    [SerializeField] private InputActionReference escMenuInput;

    private bool isGameOver;
    
    public static GameManager instance;
    
    private void Awake()
    {
        instance = this;
        isGameOver = false;
    }
    
    public void GameOver()
    {
        if (isGameOver) return;

        isGameOver = true;
        Time.timeScale = 0.5f;
        ScoreManager.Instance.AddScore(LevelManager.instance.score);
        gameOverUI.Show();
    }
    
    public void ToggleESCMenu()
    {
        if (Time.timeScale == 1f)
        {
            escMenuUI.Show();
        }
        else
        {
            escMenuUI.Resume();
        }
    }

    private void Update()
    {
        if (escMenuInput.action.WasPressedThisFrame())
        {
            ToggleESCMenu();
        }
    }
}
