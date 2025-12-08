using UnityEngine;
using TMPro;   // TextMeshPro 쓰면 필요 (UI.Text 쓰면 이 줄과 타입을 바꿔)

public class ScoreItemUI : MonoBehaviour
{
    int rank;
    public TMP_Text rankText;   // 순위 표시
    public TMP_Text scoreText;  // 점수 표시
    private ScoreboardUI scoreboard;

    public void Setup(int rank, int score, ScoreboardUI scoreboard)
    {
        this.rank = rank;
        if (rankText != null)
            rankText.text = rank.ToString();

        if (scoreText != null)
            scoreText.text = score.ToString();

        this.scoreboard = scoreboard;
    }

    public void OnClickSelf()
    {
        scoreboard.GetItemIndex(rank);
    }
}
