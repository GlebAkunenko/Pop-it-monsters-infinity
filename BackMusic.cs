using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BackMusic : MonoBehaviour, IAutoSaving
{
    public static BackMusic self { get; set; }
    public AudioSource MusicSourse { get; set; }
    public float SoundsVolume { get; private set; } = 1;
    public bool SoundMute { get; private set; } = false;

    public string KeyName => "audio_settings";

    [SerializeField] 
    private bool initOnStart;

    private AudioSource[] audios;
    private InfSettingsWindow settingsWindow;
    private bool mutedCourseAds;

    private void Awake()
    {
        if (initOnStart)
            Init();
    }

    private void Start()
    {
        if (self != null && self != this)
            Destroy(gameObject);
    }

    public void Init()
    {
        if (self != null && self != this) {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        MusicSourse = GetComponent<AudioSource>();

        if (!DataSaver.HasKey(KeyName)) {
            SoundsVolume = 1;
            SoundMute = false;
            DataSaver.SetKey(KeyName, GetSavingData());
        }
        else {
            AudioSettings data = (AudioSettings)DataSaver.GetKey(KeyName);
            SetAudioSettings(data);
        }
        DataSaver.AddAutoSaveObject(this);

        DontDestroyOnLoad(gameObject);
        self = this;
    }

    //private void LoadSoundsAudioSourses()
    //{
    //    AudioSource[] all = FindObjectsOfType<AudioSource>();
    //    AudioSource music = MusicSourse;
    //    audios = new AudioSource[all.Length - 1];
    //    for (int i = 0, j = 0; i < all.Length; i++, j++) {
    //        if (all[i] == music) {
    //            j -= 1;
    //            continue;
    //        }
    //        audios[j] = all[i];
    //    }
    //}

    //private void OnSceneLoad(Scene scene, LoadSceneMode mode)
    //{
    //    LoadSoundsAudioSourses();
    //    ApplyAudioSettings();

    //    settingsWindow = FindObjectOfType<InfSettingsWindow>();
    //    if (settingsWindow != null) {
    //        settingsWindow.InitMusicSettings(MusicSourse.volume, MusicSourse.mute);
    //        settingsWindow.InitSoundsSettings(SoundsVolume, SoundMute);
    //    }

    //}

    private void ApplyAudioSettings()
    {
        //float volume = SoundsVolume;
        //bool mute = SoundMute;
        //foreach (AudioSource audio in audios) {
        //    if (audio) {
        //        audio.volume = volume;
        //        audio.mute = mute;
        //    }
        //}
        AudioListener.volume = SoundMute ? 0 : SoundsVolume;
    }

    private void SetAudioSettings(AudioSettings settings)
    {
        SoundMute = settings.soundsMute;
        SoundsVolume = settings.soundsVolume;
        MusicSourse.mute = settings.musicMute;
        MusicSourse.volume = settings.musicVolume;
    }

    public void SetUp(AudioSettings settings)
    {
        SetAudioSettings(settings);
        ApplyAudioSettings();
    }

    public void MuteMusicForTime(float seconds)
    {
        if (MusicSourse.mute)
            return;

        MusicSourse.mute = true;
        StartCoroutine(UnmuteAfterTime(seconds));
    }

    public void MuteCourseAds()
    {
        if (MusicSourse.mute)
            return;
        MusicSourse.mute = true;
        mutedCourseAds = true;
    }

    public void UnmuteCourseAds()
    {
        if (mutedCourseAds)
            MusicSourse.mute = false;
    }

    private IEnumerator UnmuteAfterTime(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        MusicSourse.mute = false;
    }

    public object GetSavingData()
    {
        return new AudioSettings() {
            musicMute = MusicSourse.mute,
            musicVolume = MusicSourse.volume,
            soundsMute = SoundMute,
            soundsVolume = SoundsVolume
        };
    }

}

[System.Serializable]
public struct AudioSettings
{
    public bool musicMute;
    public float musicVolume;
    public bool soundsMute;
    public float soundsVolume;
}
