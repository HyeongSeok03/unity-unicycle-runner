using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaManager : MonoBehaviour
{
    [SerializeField] private List<Area> areas = new List<Area>();
    
    [SerializeField] private Vector3 startPosition = new Vector3(0f, 0f, 20f);
    [SerializeField] private Vector3 goalPosition = new Vector3(0f, 0f, 0f);
    [SerializeField] private Vector3 endPosition = new Vector3(0f, 0f, -20f);
    
    [SerializeField] private float moveDuration = 2f;
    
    private Area _currentArea;
    private Area _nextArea;

    private void Start()
    {
        _currentArea = Instantiate(GetNextArea(), goalPosition, Quaternion.identity);
        LevelManager.instance.OnLevelChanged += OnLevelChanged;
    }

    private Area GetNextArea()
    {
        var index = Random.Range(0, areas.Count);
        return areas[index];
    }
    
    private void OnLevelChanged(int level)
    {
        // 현재 구역을 뒤로 이동
        if (_currentArea)
        {
            StartCoroutine(MoveArea(_currentArea.transform, endPosition, moveDuration, true));
        }
        
        // 다음 구역을 생성하고 앞에서 중앙으로 이동
        _nextArea = Instantiate(GetNextArea(), startPosition, Quaternion.identity);
        StartCoroutine(MoveArea(_nextArea.transform, goalPosition, moveDuration, false));
    }

    private IEnumerator MoveArea(Transform areaTransform, Vector3 targetPosition, float duration, bool destroyOnComplete)
    {
        var startPos = areaTransform.position;
        var elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            var t = elapsedTime / duration;
            
            areaTransform.position = Vector3.Lerp(startPos, targetPosition, t);
            yield return null;
        }

        areaTransform.position = targetPosition;

        if (destroyOnComplete)
        {
            Destroy(areaTransform.gameObject);
        }
        else
        {
            _currentArea = _nextArea;
            _nextArea = null;
        }
    }

    private void OnDestroy()
    {
        LevelManager.instance.OnLevelChanged -= OnLevelChanged;
    }
}
