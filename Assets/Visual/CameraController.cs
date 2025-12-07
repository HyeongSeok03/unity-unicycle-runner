using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private float maxFovIncrease = 30f;
    private LevelManager _levelManager;
    private float _initialFov;
    private void Start()
    {
        _levelManager = LevelManager.instance;
        _initialFov = cam.fieldOfView;
    }

    private void Update()
    {
        var ratio = (_levelManager.obstacleSpeed - _levelManager.initialSpeed) / _levelManager.maxObstacleSpeed;
        cam.fieldOfView = _initialFov + (maxFovIncrease * ratio);
    }
}
