using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsWindow : OldWindow
{
    [SerializeField]
    private Slider volumeSlider;
    [SerializeField]
    private GameObject muteCross;

    private Animator animator;
    protected override Animator anim => animator;

    private void Start()
    {
        animator = GetComponent<Animator>();

        if (BackMusic.self == null)
            volumeSlider.gameObject.SetActive(false);

        muteCross.SetActive(BackMusic.self.MusicSourse.mute);
    }

    public void ChangeVolume(Slider slider)
    {
        BackMusic.self.MusicSourse.volume = slider.value;
        MetaSceneData.OptionsData.MusicVolume = slider.value;
    }

    public void MuteButton()
    {
        BackMusic.self.MusicSourse.mute = !BackMusic.self.MusicSourse.mute;
        muteCross.SetActive(BackMusic.self.MusicSourse.mute);
        MetaSceneData.OptionsData.Mute = BackMusic.self.MusicSourse.mute;
    }

    public override void OnClose()
    {
        Interactable.CurrentMode = Interactable.Mode.game;
    }

    public override void OnOpen()
    {

    }
}
