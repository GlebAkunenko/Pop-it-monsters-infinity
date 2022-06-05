using System;
using UnityEngine;
using UnityEngine.UI;

public class InfSettingsWindow : Window
{
    [SerializeField]
    private Slider musicSlider;
    [SerializeField]
    private GameObject musicMuteCross;
    [SerializeField]
    private Slider soundsSlider;
    [SerializeField]
    private GameObject soundsMuteCross;

    private bool musicMute;
    private bool soundsMute;
    private float musicVolume;
    private float soundsVolume;

    protected override Animator anim => throw new NotImplementedException();

    public void InitMusicSettings(float volume, bool mute)
    {
        musicMute = mute;
        musicVolume = volume;
        musicSlider.value = volume;
        musicMuteCross.SetActive(mute);
    }

    public void InitSoundsSettings(float volume, bool mute)
    {
        soundsMute = mute;
        soundsVolume = volume;
        soundsSlider.value = volume;
        soundsMuteCross.SetActive(mute);
    }

    public void OnMusicSliderChange(float value)
    {
        musicVolume = value;
    }

    public void OnMusicMuteClick()
    {
        musicMute = !musicMute;
        musicMuteCross.SetActive(musicMute);
    }

    public void OnSoundsSliderChange(float value)
    {
        soundsVolume = value;
    }

    public void OnSoundMuteClicked()
    {
        soundsMute = !soundsMute;
        soundsMuteCross.SetActive(soundsMute);
    }

    public void SaveChanges()
    {
        BackMusic.self.SetUp(new AudioSettings() {
            musicMute = musicMute,
            musicVolume = musicVolume,
            soundsMute = soundsMute,
            soundsVolume = soundsVolume
        });
    }

}
