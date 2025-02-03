using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

        }
    }

    
    public void PlayClip(AudioClip clip, AudioSource source)
    {
        source.clip = clip;
        source.Play();

    }

    public void PlayRandomClip(AudioClip[] clips, AudioSource source)
    {
        int randIndex = Random.Range(0, clips.Length);
        source.clip = clips[randIndex];
        source.Play();

    }


}

