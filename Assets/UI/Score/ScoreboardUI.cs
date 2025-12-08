using UnityEngine;
using UnityEngine.UI;

public class ScoreboardUI : MonoBehaviour
{
    [Header("ScrollView Content")]
    public Transform content;           // ScrollView 안의 Content 오브젝트

    [Header("Score Item Prefab")]
    public GameObject scoreItemPrefab;  // 점수 한 줄 프리팹 (ScoreItemUI 붙어 있어야 함)

    [Header("Options")]
    public bool autoRefreshOnEnable = true; // 켜질 때 한 번 즉시 갱신할지

    public Button deleteButton;

    private void Start()
    {
        if (ScoreManager.Instance != null)
        {
            print("aasd");
            // ✅ 점수 변경 이벤트에 구독
            ScoreManager.Instance.OnScoresChanged += Refresh;

            // UI 켜질 때도 한 번 바로 그리게 하고 싶으면
            if (autoRefreshOnEnable)
            {
                Refresh();
            }
        }
    }

    private void OnDisable()
    {
        if (ScoreManager.Instance != null)
        {
            // ✅ 이벤트 구독 해제 (메모리/중복 호출 방지)
            ScoreManager.Instance.OnScoresChanged -= Refresh;
        }
    }

    /// <summary>
    /// ScoreManager의 Scores를 기반으로 UI 다시 그림
    /// </summary>
    public void Refresh()
    {
        if (content == null || scoreItemPrefab == null)
        {
            Debug.LogWarning("ScoreUI: content나 scoreItemPrefab이 설정되지 않음");
            return;
        }

        // 기존 항목 모두 제거
        for (int i = content.childCount - 1; i >= 0; i--)
        {
            Destroy(content.GetChild(i).gameObject);
        }

        // 점수 리스트 가져오기
        var scores = ScoreManager.Instance.Scores;

        for (int i = 0; i < scores.Count; i++)
        {
            GameObject itemGO = Object.Instantiate(scoreItemPrefab, content);
            var itemUI = itemGO.GetComponent<ScoreItemUI>();

            if (itemUI != null)
            {
                int rank = i + 1;          // 1위부터 시작
                int score = scores[i];
                itemUI.Setup(rank, score, this);
            }
            else
            {
                Debug.LogWarning("ScoreUI: scoreItemPrefab에 ScoreItemUI 컴포넌트가 없음");
            }
        }
    }

    public void GetItemIndex(int rank)
    {
        removeIndex = rank - 1;
        deleteButton.interactable = true;
    }

    private int removeIndex = -1;
    public void DeleteSelectedBtn()
    {
        if (removeIndex == -1)
        {
            Debug.Log("removeIndex is -1");
            return;
        }
        ScoreManager.Instance.RemoveScoreAt(removeIndex);
        Refresh();
        deleteButton.interactable=false;
    }

    public void ClearAll()
    {
        ScoreManager.Instance.ClearScores();
        Refresh();
    }
}
