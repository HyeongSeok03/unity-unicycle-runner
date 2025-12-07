using UnityEngine;
using UnityEngine.InputSystem;


public class GameManager : MonoBehaviour
{
    [SerializeField] private GameOverUI gameOverUI;
    [SerializeField] private ESCMenuUI escMenuUI;
    [SerializeField] private InputActionReference escMenuInput;
    
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
