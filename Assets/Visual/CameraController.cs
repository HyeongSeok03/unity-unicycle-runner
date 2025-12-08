using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Unicycle player;
    private Vector3 camerOffset;
    [SerializeField] private Camera cam;
    [SerializeField] private float maxFovIncrease = 30f;
    private LevelManager _levelManager;
    private float _initialFov;
    private void Start()
    {
        _levelManager = LevelManager.instance;
        _initialFov = cam.fieldOfView;
        camerOffset = new Vector3(0, 5, -10);
    }

    private void Update()
    {
        cam.transform.position = new Vector3(player.transform.position.x, 5, player.transform.position.z - 10);
        var ratio = (_levelManager.moveSpeed - _levelManager.initialSpeed) / _levelManager.maxObstacleSpeed;
        cam.fieldOfView = _initialFov + (maxFovIncrease * ratio);
    }
}
