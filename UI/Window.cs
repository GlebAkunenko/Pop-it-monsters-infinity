using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : MonoBehaviour
{
    protected virtual Animator anim => GetComponent<Animator>();

    public virtual void OnOpen()
    {
        Interactable.CurrentMode = Interactable.Mode.canvas;
    }

    public virtual void OnClose()
    {
        Interactable.CurrentMode = Interactable.Mode.game;
    }

    public virtual void Open()
    {
        Interactable.CurrentMode = Interactable.Mode.none;
        anim.SetTrigger("open");
    }

    public virtual void Close()
    {
        Interactable.CurrentMode = Interactable.Mode.none;
        anim.SetTrigger("close");
    }
}
