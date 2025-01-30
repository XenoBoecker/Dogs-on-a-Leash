using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] public AudioClip[] music;
    [SerializeField] AudioSource musicAudioSource;
    [SerializeField] AudioSource sfxAudioSource;
    bool musicOn = true;
    bool sfxOn = true;
    int currentTrack = 0;

    [Range(0, 1)]
    [SerializeField] float musicVolume = 0.5f;
    [Range(0, 1)]
    [SerializeField] float sfxVolume = 0.5f;
    public float SFXVolume => sfxVolume;

    public event Action OnSoundReload;

    [Header("SFX")]
    public UISFX uiSFX;
    public DogSFX dogSFX;
    public ScoreSFX scoreSFX;
    public AmbientSFX ambientSFX;
    public EventSFX eventSFX;
    public HumanSFX humanSFX;

    private void Awake()
    {
        if(Instance == null) Instance = this;
        else
        {
            if (Instance.music[0] != music[0])
            {
                Instance.SetMusic(music[0]);
            }
            Destroy(gameObject);
        }
        

        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        musicAudioSource.loop = true;
        musicAudioSource.clip = music[currentTrack];
        if(musicOn && music.Length > 0) musicAudioSource.Play();

        Reload();
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        musicAudioSource.volume = musicVolume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = volume;
        sfxAudioSource.volume = sfxVolume;
    }

    public void SetMusic(AudioClip musicClip)
    {
        musicAudioSource.clip = musicClip;
        musicAudioSource.Play();
    }

    public void PlaySound(AudioClip clip, AudioSource source = null, float volumeMultiplier = 1, bool resetPitch = true)
    {
        if (!sfxOn) return;

        if (source == null) source = sfxAudioSource;

        if (resetPitch) source.pitch = 1;

        source.PlayOneShot(clip, volumeMultiplier);
    }
    public AudioClip PlaySound(AudioClip[] clips, AudioSource source = null, float volumeMultiplier = 1)
    {
        if (clips.Length == 0) return null;

        int rand = UnityEngine.Random.Range(0, clips.Length);

        AudioClip clip = clips[rand];

        PlaySound(clip, source, volumeMultiplier);

        return clip;
    }

    public void PlaySoundWithRandomPitch(AudioClip clip, AudioSource source = null, float min = 0.1f, float max = 2f)
    {
        if (source == null) source = sfxAudioSource;

        source.pitch = UnityEngine.Random.Range(min, max);
        
        PlaySound(clip, source, 1, false);
    }

    internal void Reload()
    {
        SetMusicVolume(PlayerPrefs.GetFloat("MusicVolume", 0.5f));
        SetSFXVolume(PlayerPrefs.GetFloat("SFXVolume", 0.5f));
        bool wasMusicOn = musicOn;
        musicOn = PlayerPrefs.GetInt("MusicOn", 1) == 1;
        if (musicOn && !wasMusicOn) musicAudioSource.Play();
        else if(!musicOn) musicAudioSource.Stop();

        sfxOn = PlayerPrefs.GetInt("SFXOn", 1) == 1;

        OnSoundReload?.Invoke();
    }

    [System.Serializable]
    public struct DogSFX
    {
        public AudioClip walkBernard, walkPoodle, walkPug, walkRetreiver;
        public AudioClip[] walk_street;
        public AudioClip[] barkBernard, barkPoodle, barkPug, barkRetreiver;
        public AudioClip[] noise;
    }

    [System.Serializable]
    public struct HumanSFX
    {
        public AudioClip[] walking;
        public AudioClip[] walking_street;
        public AudioClip mumbling; ////TODO: mumbling is now the humanNoise Array, therefore here not needed anymore
        public AudioClip[] humanNoise;
        public AudioClip hit_obstacle;
    }

    [System.Serializable]
    public struct ScoreSFX
    {

        public AudioClip smallGain, bigGain;
        public AudioClip smallLoss, bigLoss; //TODO: bigLoss likely not needed as there is no point deduction in the score screen
    }

    [System.Serializable]
    public struct EventSFX
    {
        public AudioClip pickUp;
        public AudioClip sparkle;
        public AudioClip dig;
    }

    [System.Serializable]
    public struct AmbientSFX
    {
        public AudioClip carNoise, carHonk; //TODO: carHonk is now an Array, therefore here not needed anymore
        public AudioClip[] carHonk1;
        public AudioClip birdNoise, fountainNoise, riverNoise, wind, leavesRustling, constructionSite, childrenPlaying; //TODO: birdNoise, fountainNoise & wind are now Arrays, therefore here not needed anymore
        public AudioClip[] birdNoise1, fountainNoise1, wind1; 
    }

    [System.Serializable]
    public struct UISFX
    {
        public AudioClip buttonClickSound;
        public AudioClip buttonHoverSound;
        public AudioClip countDown, popUp; //TODO: countDown not needed anymore, instead countDownTick & countDownWhistle
        public AudioClip countDownTick, countDownWhistle;
        public AudioClip endOfGameTick, endOfGameWhistle;
    }
}

