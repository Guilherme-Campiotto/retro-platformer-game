using UnityEngine;

public class SoundController : MonoBehaviour
{
    public AudioSource audioSource;
    public static SoundController instance { get; private set; } = null;
    string sound;

    void Awake()
    {
        if(gameObject.CompareTag("SoundController") && instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        } else if(gameObject.CompareTag("SoundController"))
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

    }

    private void Start()
    {
        sound = PlayerPrefs.GetString("Sound");
        if (sound == "Off")
        {
            audioSource.mute = true;
        }
    }

    public void PlayAudioOnce(AudioClip clip, float volume = 1f)
    {
        audioSource.PlayOneShot(clip, volume);
    }

    public void MuteAudio()
    {
        audioSource.mute = true;
    }

    public void PlayAudio()
    {
        audioSource.mute = false;
    }

    public void PlayWithLoop(AudioClip music)
    {
        audioSource.Stop();
        audioSource.clip = music; 
        audioSource.Play();
    }
}
