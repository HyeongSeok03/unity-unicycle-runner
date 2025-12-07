using UnityEngine;
using UnityEngine.SceneManagement;

public static class StageManager
{
    
    public static void GameOver()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
