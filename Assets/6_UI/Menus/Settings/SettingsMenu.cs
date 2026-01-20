using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] Slider musicVolumeSlider;
    [SerializeField] Slider sfxVolumeSlider;

    [SerializeField] private Toggle tutorialToggle;


    private void Start()
    {
        musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume);

        musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
        sfxVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.5f);

        tutorialToggle.isOn = PlayerPrefs.GetInt("ShowTutorial", 1) == 1;
    }

    public void SetMusicVolume(float value)
    {
        PlayerPrefs.SetFloat("MusicVolume", value);

        ReloadSoundManager();
    }

    public void SetSFXVolume(float value)
    {
        PlayerPrefs.SetFloat("SFXVolume", value);

        ReloadSoundManager();
    }

    void ReloadSoundManager()
    {
        if (SoundManager.Instance == null) return;

        SoundManager.Instance.Reload();
    }

    public void SetShowTutorial(bool show)
    {
        PlayerPrefs.SetInt("ShowTutorial", show ? 1 : 0);
    }
}