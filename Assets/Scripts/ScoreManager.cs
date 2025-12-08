using System;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    [Header("High Score List (높은 점수 순)")]
    [SerializeField]
    private List<int> scores = new List<int>();   // Inspector 확인용

    private const string PlayerPrefsKey = "HighScores";

    // 외부에서는 읽기만 가능
    public IReadOnlyList<int> Scores => scores;

    // ✅ 점수 리스트에 변화가 생겼을 때 호출되는 이벤트
    public event Action OnScoresChanged;

    private void Awake()
    {
        // Singleton 세팅
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadScores();

        // 시작할 때 한 번 UI가 초기 상태를 그릴 수 있도록 알림
        NotifyScoresChanged();
    }

    /// <summary>
    /// 새로운 점수 추가 후 자동 정렬 & 저장 & 이벤트 호출
    /// </summary>
    public void AddScore(int score)
    {
        if (score < 0)
        {
            Debug.LogWarning($"ScoreManager: 음수 점수는 추가하지 않음. score = {score}");
            return;
        }

        scores.Add(score);
        SortScoresDescending();
        SaveScores();
        NotifyScoresChanged();
    }

    /// <summary>
    /// 인덱스로 기존 기록 제거 (0 = 최고점)
    /// </summary>
    public void RemoveScoreAt(int index)
    {
        if (index < 0 || index >= scores.Count)
        {
            Debug.LogWarning($"ScoreManager: 잘못된 인덱스 {index}");
            return;
        }

        scores.RemoveAt(index);
        SaveScores();
        NotifyScoresChanged();
    }

    /// <summary>
    /// 높은 점수 순 정렬
    /// </summary>
    public void SortScoresDescending()
    {
        scores.Sort((a, b) => b.CompareTo(a));    // 내림차순
    }

    /// <summary>
    /// 최고 점수 리턴 (없으면 0)
    /// </summary>
    public int GetBestScore()
    {
        if (scores.Count == 0) return 0;
        return scores[0]; // 항상 높은 점수 순으로 관리
    }

    /// <summary>
    /// 모든 기록 삭제
    /// </summary>
    [ContextMenu("clear scores")]
    public void ClearScores()
    {
        scores.Clear();
        SaveScores();
        NotifyScoresChanged();
    }

    /// <summary>
    /// 점수 리스트 변화 알림 이벤트 호출
    /// </summary>
    private void NotifyScoresChanged()
    {
        OnScoresChanged?.Invoke();
    }

    #region Save / Load

    [System.Serializable]
    private class ScoreData
    {
        public int[] scores;
    }

    private void SaveScores()
    {
        ScoreData data = new ScoreData
        {
            scores = scores.ToArray()
        };

        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(PlayerPrefsKey, json);
        PlayerPrefs.Save();
    }

    private void LoadScores()
    {
        if (!PlayerPrefs.HasKey(PlayerPrefsKey))
            return;

        string json = PlayerPrefs.GetString(PlayerPrefsKey, "");
        if (string.IsNullOrEmpty(json))
            return;

        ScoreData data = JsonUtility.FromJson<ScoreData>(json);
        if (data != null && data.scores != null)
        {
            scores = new List<int>(data.scores);
            SortScoresDescending();
        }
    }

    #endregion
}
