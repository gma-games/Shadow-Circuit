using System.Collections.Generic;
using UnityEngine;

// Név adása a hangoknak a unityben
[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource backgroundSource;
    public AudioSource sfxSource;

    [Header("Audio Clips")]
    public AudioClip music;
    public AudioClip background;
    public Sound[] sfxList;

  
    private Dictionary<string, AudioClip> soundEffects = new Dictionary<string, AudioClip>();

    private void Awake()
    {

        // Singleton beállítása
        if (Instance == null)
        {
            Instance = this;

            if (sfxList == null) return;
            // Zene a pályál váltása között is
            // DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Dictionar feltötlése az Inspectorban megadott listából
        foreach (Sound s in sfxList)
        {
            if (!soundEffects.ContainsKey(s.name))
            {
                soundEffects.Add(s.name, s.clip);
            }
        }
    }

    private void Start()
    {
        if (music != null)
        {
            PlayMusic();
           
        }

        if (background != null)
        {
            PlayBackgroundNoise();
        }
    }

    public void PlaySound(string soundName, float volume = 1f)
    {
        if (soundEffects.ContainsKey(soundName))
        {
            sfxSource.PlayOneShot(soundEffects[soundName], volume);
        }
        else
        {
            Debug.LogWarning("Nem található ilyen nevű hang: " + soundName);
        }
    }

    public void StopSound(string soundName)
    {
      
        sfxSource.Stop();
    }

    public void PlayMusic()
    {
        musicSource.clip = music;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlayBackgroundNoise()
    {
        backgroundSource.clip = background;
        backgroundSource.loop = true;
        backgroundSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void StopBackgroundNoise()
    {
        backgroundSource.Stop();
    }
}