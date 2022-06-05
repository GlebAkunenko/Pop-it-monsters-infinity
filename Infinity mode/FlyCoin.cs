using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animation))]
public class FlyCoin : MonoBehaviour
{
    private Animation anim;
    private SpriteRenderer spriteRenderer;

    public bool IsFlying { get; private set; } = false;

    private void Start()
    {
        anim = GetComponent<Animation>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
    }

    public void Fly()
    {
        spriteRenderer.enabled = true;
        IsFlying = true;
        anim.Play();
    }

    public void OnEndFlying()
    {
        PlayerInfo.Self.AddLife();
        spriteRenderer.enabled = false;
        IsFlying = false;
    }

}
