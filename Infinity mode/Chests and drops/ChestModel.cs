using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestModel : Popit
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private Transform itemSpawnTarget;
    [SerializeField]
    private AudioClip chestFall;
    [SerializeField]
    private AudioClip chestOpen;

    private new AudioSource audio;

    private void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    protected override void OnFull()
    {
        PopAllPimples();
    }

    public void OnOpen()
    {
        Open();
    }

    public void PlayChestFallSound()
    {
        audio.PlayOneShot(chestFall);
    }

    public void PlayChestOpenSound()
    {
        audio.PlayOneShot(chestOpen);
    }


    public Vector3 SpawnItemPosition => itemSpawnTarget.position;
    public Animator Animator => animator;

    public event System.Action PopAllPimples;
    public event System.Action Open;
}
