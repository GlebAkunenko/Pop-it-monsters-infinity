using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSounds : MonoBehaviour
{
    [SerializeField]
    protected AudioClip[] awakeSounds;
    [SerializeField]
    protected AudioClip[] spawnSounds;
    [SerializeField]
    public AudioClip[] hitSounds;
    [SerializeField]
    public AudioClip[] deathSounds;

    protected AudioSource audioSource;

    public void Init(AudioSource audioSource)
    {
        this.audioSource = audioSource;
    }

    protected void TryPlaySound(AudioClip[] clips)
    {
        if (clips.Length > 0) {
            AudioClip clip = clips[Random.Range(0, clips.Length)];
            if (clip != null)
                audioSource.PlayOneShot(clip);
        }
    }

    public void PlayAwake()
    {
        TryPlaySound(awakeSounds);
    }

    public void PlaySpawn()
    {
        TryPlaySound(spawnSounds);
    }

    public void PlayHit()
    {
        TryPlaySound(hitSounds);
    }

    public void PlayHit(AudioClip specialSound)
    {
        audioSource.PlayOneShot(specialSound);
    }

    public void PlayDeath()
    {
        TryPlaySound(deathSounds);
    }




}
