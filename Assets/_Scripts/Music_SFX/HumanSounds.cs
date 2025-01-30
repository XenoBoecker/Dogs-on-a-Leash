using UnityEngine;

public class HumanSounds : MonoBehaviour
{
    [SerializeField] AudioSource stepAudioSource;

    [SerializeField] float stepVolumeMultiplier = 0.5f;


    [SerializeField] float minHumanNoiseDelay, maxHumanNoiseDelay;
    float humanNoiseTimer;

    void Start()
    {
        SoundManager.Instance.OnSoundReload += Reload;
        Reload();

        IKFootSolver[] ikFootSolvers = GetComponentsInChildren<IKFootSolver>();
        for (int i = 0; i < ikFootSolvers.Length; i++)
        {
            ikFootSolvers[i].OnTakeStep += PlayStepSound;
        }

        humanNoiseTimer = Random.Range(minHumanNoiseDelay, maxHumanNoiseDelay);
    }
    private void Update()
    {
        humanNoiseTimer -= Time.deltaTime;

        if(humanNoiseTimer < 0)
        {
            float clipLength = SoundManager.Instance.PlaySound(SoundManager.Instance.humanSFX.humanNoise, null, stepVolumeMultiplier).length;
            humanNoiseTimer = clipLength + Random.Range(minHumanNoiseDelay, maxHumanNoiseDelay);
        }
    }

    private void PlayStepSound()
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.humanSFX.walking, stepAudioSource, stepVolumeMultiplier);
    }

    private void Reload()
    {
        stepAudioSource.volume = SoundManager.Instance.SFXVolume;
    }
}