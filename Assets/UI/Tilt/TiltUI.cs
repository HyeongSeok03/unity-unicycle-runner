using UnityEngine;
using UnityEngine.UI;

public class TiltUI : MonoBehaviour
{
    [SerializeField] private Slider tiltSlider;
    
    private Unicycle _player;
    
    private void Start()
    {
        _player = Unicycle.instance;
    }

    private void Update()
    {
        tiltSlider.value = -1 * _player.transform.rotation.z;
    }
}
