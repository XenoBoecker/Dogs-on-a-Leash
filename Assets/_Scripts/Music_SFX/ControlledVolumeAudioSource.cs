using UnityEngine;

public class ControlledVolumeAudioSource : MonoBehaviour
{
    AudioSource source;


    [SerializeField]
    AudioClip[] audioClips;

    [SerializeField] float minDelayBetweenClips = 1, maxDelayBetweenClips = -1;
    float delayTimer;
    [SerializeField] float volumeMultiplier = 1;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();

        if(audioClips.Length > 0)
        {
            source.clip = audioClips[Random.Range(0, audioClips.Length)];
            source.Play();
        }

        SoundManager.Instance.OnSoundReload += SoundReload;

        SoundReload();
    }

    private void Update()
    {
        if (maxDelayBetweenClips < minDelayBetweenClips || maxDelayBetweenClips < 0) return;
        if (audioClips.Length == 0) return;

        delayTimer -= Time.deltaTime;

        if (delayTimer > 0) return;

        source.clip = audioClips[UnityEngine.Random.Range(0, audioClips.Length)];
        source.Play();

        delayTimer = Random.Range(minDelayBetweenClips, maxDelayBetweenClips) + source.clip.length;
    }

    private void SoundReload()
    {
        source.volume = SoundManager.Instance.SFXVolume * volumeMultiplier;
    }
}
