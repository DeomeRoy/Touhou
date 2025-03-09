using System.Collections;
using UnityEngine;

public class GlobalAudioManager : MonoBehaviour
{
    public static GlobalAudioManager Instance;

    public AudioClip damageSound;
    public AudioClip explosionSound;
    public AudioClip buttonSound;
    public AudioClip attackSound;
    public AudioClip mainMenuMusic;
    public AudioClip stage1Music;
    public AudioClip stage2Music;
    public AudioClip stage3Music;
    public AudioClip bossMusic;

    public float globalVolume = 1f;

    // 多音效使用
    private AudioSource sfxSource;

    public AudioSource musicSource1;
    public AudioSource musicSource2;
    private AudioSource activeMusicSource;

    public float defaultFadeDuration = 2.0f;

    // 個別音量控制
    public float mainMenuVolume = 1f;
    public float stage1Volume = 0.8f;
    public float stage2Volume = 0.8f;
    public float stage3Volume = 0.8f;
    public float bossVolume = 1f;
    public float damageVolume = 1f;
    public float explosionVolume = 1f;
    public float buttonVolume = 1f;
    public float attackVolume = 1f;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (PlayerPrefs.HasKey("GlobalVolume"))
            globalVolume = PlayerPrefs.GetFloat("GlobalVolume");
        AudioListener.volume = globalVolume;

        sfxSource = gameObject.AddComponent<AudioSource>();

        if (musicSource1 == null)
            musicSource1 = gameObject.AddComponent<AudioSource>();
        if (musicSource2 == null)
            musicSource2 = gameObject.AddComponent<AudioSource>();

        activeMusicSource = musicSource1;
    }

    public void SetVolume(float value)
    {
        globalVolume = value;
        AudioListener.volume = value;
        PlayerPrefs.SetFloat("GlobalVolume", value);
    }

    public void PlayDamageSound(float volumeScale = 1f)
    {
            sfxSource.PlayOneShot(damageSound, damageVolume * volumeScale);
    }

    public void PlayExplosionSound(float volumeScale = 1f)
    {
            sfxSource.PlayOneShot(explosionSound, explosionVolume * volumeScale);
    }

    public void PlayButtonSound(float volumeScale = 1f)
    {
        sfxSource.PlayOneShot(buttonSound, buttonVolume * volumeScale);
    }

    public void PlayAttackSound(float volumeScale = 1f)
    {
        sfxSource.PlayOneShot(attackSound, attackVolume * volumeScale);
    }

    public void PlayMainMenuMusic()
    {
        StopAllMusic();
        activeMusicSource = musicSource1;
        activeMusicSource.clip = mainMenuMusic;
        activeMusicSource.volume = mainMenuVolume;
        activeMusicSource.loop = true;
        activeMusicSource.Play();
    }

    public void PlayMusicDirectly(AudioClip newClip, float targetVolume, bool forceChange = false)
    {
        if (!forceChange && activeMusicSource.clip == newClip && activeMusicSource.isPlaying)
           return;
                
        activeMusicSource.Stop();
        activeMusicSource.clip = newClip;
        activeMusicSource.volume = targetVolume;
        activeMusicSource.loop = true;
        activeMusicSource.Play();
    }

    public void CrossfadeToMusicWithTarget(AudioClip newClip, float targetVolume, float fadeOutDuration, float fadeInDuration)
    {
        AudioSource newSource = (activeMusicSource == musicSource1) ? musicSource2 : musicSource1;
        newSource.clip = newClip;
        newSource.volume = 0f;
        newSource.loop = true;
        newSource.Play();

        StartCoroutine(Crossfade(activeMusicSource, newSource, fadeOutDuration, fadeInDuration, targetVolume));
        activeMusicSource = newSource;
    }

    public void CrossfadeToStage1Music(float fadeOutDuration, float fadeInDuration)
    {
        CrossfadeToMusicWithTarget(stage1Music, stage1Volume, fadeOutDuration, fadeInDuration);
    }

    public void CrossfadeToStage2Music(float fadeOutDuration, float fadeInDuration)
    {
        CrossfadeToMusicWithTarget(stage2Music, stage2Volume, fadeOutDuration, fadeInDuration);
    }

    public void CrossfadeToStage3Music(float fadeOutDuration, float fadeInDuration)
    {
        CrossfadeToMusicWithTarget(stage3Music, stage3Volume, fadeOutDuration, fadeInDuration);
    }

    public void PlayBossMusicWithFadeIn(float fadeInDuration)
    {
        AudioSource newSource = (activeMusicSource == musicSource1) ? musicSource2 : musicSource1;
        newSource.clip = bossMusic;
        newSource.volume = 0f;
        newSource.loop = true;
        newSource.Play();
        StartCoroutine(FadeIn(newSource, fadeInDuration, bossVolume));
        activeMusicSource = newSource;
    }

    IEnumerator FadeIn(AudioSource source, float duration, float targetVolume)
    {
        float time = 0f;
        while (time < duration)
        {
            source.volume = Mathf.Lerp(0f, targetVolume, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        source.volume = targetVolume;
    }

    public void FadeOutMusic(float fadeOutDuration)
    {
        StartCoroutine(FadeOut(activeMusicSource, fadeOutDuration));
    }

    IEnumerator FadeOut(AudioSource source, float duration)
    {
        float startVolume = source.volume;
        float time = 0;
        while (time < duration)
        {
            source.volume = Mathf.Lerp(startVolume, 0, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        source.volume = 0;
        source.Stop();
    }

    IEnumerator Crossfade(AudioSource fromSource, AudioSource toSource, float fadeOutDuration, float fadeInDuration, float targetVolume)
    {
        float time = 0;
        float maxDuration = Mathf.Max(fadeOutDuration, fadeInDuration);
        float fromStartVolume = fromSource.volume;
        while (time < maxDuration)
        {
            if (time < fadeOutDuration)
                fromSource.volume = Mathf.Lerp(fromStartVolume, 0, time / fadeOutDuration);
            else
                fromSource.volume = 0;

            if (time < fadeInDuration)
                toSource.volume = Mathf.Lerp(0, targetVolume, time / fadeInDuration);
            else
                toSource.volume = targetVolume;

            time += Time.deltaTime;
            yield return null;
        }
        fromSource.Stop();
    }

    public void StopAllMusic()
    {
        musicSource1.Stop();
        musicSource2.Stop();
    }
}

