using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [Header("Background Music")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioClip gameOST;
    [SerializeField] private AudioClip menuOST;
    
    private static MusicPlayer _instance;
    
    public static void PlayGameMusic()
    {
        if (!_instance) return;
        
        _instance.bgmSource.clip = _instance.gameOST;
        _instance.bgmSource.Play();
    }
    
    public static void PlayMenuMusic()
    {
        if (!_instance) return;
        
        _instance.bgmSource.clip = _instance.menuOST;
        _instance.bgmSource.Play();
    }

    private void Awake()
    {
        if (!_instance)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
