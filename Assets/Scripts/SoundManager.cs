using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private AudioSource[] audioSource;
    public AudioClip clipFly, clipScore, clipDie;

    public static SoundManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {

        audioSource = GetComponents<AudioSource>();
    }

    public void PlayClip(AUDIO_CLIP clip, int index)
    {
        AudioClip audioClip;
        switch (clip)
        {
            case AUDIO_CLIP.FLY:
                audioClip = clipFly;
                break;
            case AUDIO_CLIP.SCORE:
                audioClip = clipScore;
                break;
            case AUDIO_CLIP.DIE:
                audioClip = clipDie;
                break;
            default:
                audioClip = clipFly;
                break;
        }
        audioSource[index].clip = audioClip;
        audioSource[index].Play();
    }
}

public enum AUDIO_CLIP
{
    FLY = 0, SCORE, DIE
}