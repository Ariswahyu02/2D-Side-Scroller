using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public struct NamedClip {
    public string name;    
    public AudioClip clip;
}

public class SoundManager : Singleton<SoundManager>
{
    [Header("SFX")]
    [SerializeField] private AudioSource sfxSourcePrefab;
    [SerializeField] private int sfxSourcePoolSize = 10;
    [SerializeField] private List<NamedClip> sfxLibrary = new List<NamedClip>();

    [Header("BGM")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private List<NamedClip> bgmLibrary = new List<NamedClip>();

    private readonly List<AudioSource> sfxSources = new List<AudioSource>();

    protected override void Awake()
    {
        base.Awake();

        // Pooling AudioSource untuk SFX
        for (int i = 0; i < sfxSourcePoolSize; i++)
        {
            AudioSource src = Instantiate(sfxSourcePrefab, transform);
            src.playOnAwake = false;
            sfxSources.Add(src);
        }
    }

    private AudioSource GetAvailableSFXSource()
    {
        foreach (var src in sfxSources)
            if (!src.isPlaying) return src;
        return sfxSources[0]; // fallback
    }

    private AudioClip FindClip(string name, List<NamedClip> library)
    {
        foreach (var nc in library)
        {
            if (nc.name.Equals(name, System.StringComparison.OrdinalIgnoreCase))
                return nc.clip;
        }
        return null;
    }

    public void PlaySFX(string name, float volume = 1f, float pitch = 1f)
    {
        var clip = FindClip(name, sfxLibrary);
        if (clip == null) { Debug.LogWarning($"SFX '{name}' tidak ditemukan"); return; }

        var src = GetAvailableSFXSource();
        src.pitch = Mathf.Clamp(pitch, 0.1f, 3f);
        src.PlayOneShot(clip, Mathf.Clamp01(volume));
    }

    public void PlayBGM(string name, bool loop = true, float volume = 1f)
    {
        var clip = FindClip(name, bgmLibrary);
        if (clip == null) { Debug.LogWarning($"BGM '{name}' tidak ditemukan"); return; }

        bgmSource.clip = clip;
        bgmSource.loop = loop;
        bgmSource.volume = Mathf.Clamp01(volume);
        bgmSource.Play();
    }

    public void StopBGM()
    {
        if (bgmSource != null) bgmSource.Stop();
    }
}
