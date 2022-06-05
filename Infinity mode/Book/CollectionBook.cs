using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CollectionBook : Window
{
    [SerializeField]
    private BookPageModel dinamicPage;
    [SerializeField]
    private BookPageModel staticPage;
    [SerializeField]
    private PageCollection openedCollection;
    [SerializeField]
    private GameObject buttonStar;

    [SerializeField]
    private AudioClip openSound;
    [SerializeField]
    private AudioClip closeSound;

    private Animator anim;
    private AudioSource audioSource;

    private void Start()
    {
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        dinamicPage.SetPageCollection(openedCollection);
        staticPage.SetPageCollection(null);

        if (MetaSceneData.UnlockNewMonster)
            buttonStar.SetActive(true);
    }

    public void Open()
    {
        Interactable.CurrentMode = Interactable.Mode.none;
        anim.SetTrigger("open");

        MetaSceneData.UnlockNewMonster = false;
        buttonStar.SetActive(false);
    }

    public void Close()
    {
        Interactable.CurrentMode = Interactable.Mode.none;
        anim.SetTrigger("close");
    }

    public void PlayOpenSound()
    {
        audioSource.PlayOneShot(openSound);
    }

    public void PlayCloseSound()
    {
        audioSource.PlayOneShot(closeSound);
    }

    public override void OnOpen()
    {
        Interactable.CurrentMode = Interactable.Mode.canvas;
        dinamicPage.AnimateUnlockMonster();
    }

    public override void OnClose()
    {
        Interactable.CurrentMode = Interactable.Mode.game;
    }
}
