using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;

    private void Start()
    {
        LevelManager.instance.OnScoreChanged += OnScoreChanged;
    }
    
    private void OnScoreChanged(int score)
    {
        scoreText.text = $"{score}";
    }
}
