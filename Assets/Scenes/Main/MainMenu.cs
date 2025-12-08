using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        MusicPlayer.PlayGameMusic();
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
}
