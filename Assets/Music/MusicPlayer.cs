using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [Header("Background Music")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioClip gameOST;
    [SerializeField] private AudioClip menuOST;
    
    [Header("Narrations")]
    [SerializeField] private AudioSource narrationSource;
    [SerializeField] private AudioClip[] narrationClips;
    
    private static MusicPlayer _instance;
    
    public static void StopNarration()
    {
        _instance.narrationSource.Stop();
    }
    
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

    public static void PlayNarration()
    {
        if (!_instance) return;
        var randomIndex = Random.Range(0, _instance.narrationClips.Length);
        _instance.narrationSource.PlayOneShot(_instance.narrationClips[randomIndex]);
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
