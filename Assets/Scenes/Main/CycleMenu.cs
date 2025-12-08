using DG.Tweening;
using UnityEngine;

public class CycleMenu : MonoBehaviour
{
    private Quaternion _startRotation;
    private void Start()
    {
        _startRotation = transform.rotation;
        var targetRotation = _startRotation.eulerAngles + new Vector3(0, 360, 0);
        transform.DORotate(targetRotation, 20f, RotateMode.FastBeyond360)
            .SetLoops(-1)
            .SetEase(Ease.Linear);
    }
}
