using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsyncMouthBehaviour : SpecialMonsterBehaviour
{
    [SerializeField]
    protected Animator[] asyncMouthes;

    private IEnumerator AnimationPause(float time, Animator animator)
    {
        animator.speed = 0;
        yield return new WaitForSeconds(time);
        animator.speed = 1;
    }

    private float RandomClipTime(float clipLenght)
    {
        return clipLenght * (Random.value * Random.value);
    }

    protected void MakeMouthesAsync()
    {
        foreach (Animator mouth in asyncMouthes) {
            StartCoroutine(AnimationPause(
                time: RandomClipTime(mouth.GetCurrentAnimatorClipInfo(0)[0].clip.length),
                animator: mouth));
        }
    }

    protected void RestartMouthes()
    {
        foreach (Animator mouth in asyncMouthes) {
            mouth.SetTrigger("restart");
        }
    }

    protected override void FirstEvent()
    {
        MakeMouthesAsync();
    }

    protected override void SecondEvent()
    {
        RestartMouthes();
    }


}
