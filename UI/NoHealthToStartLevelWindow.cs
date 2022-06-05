using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoHealthToStartLevelWindow : Window
{
    public static NoHealthToStartLevelWindow self { private set; get; }

    [SerializeField]
    private MenuAddHealth addHealth;

    private Animator animator;

    protected override Animator anim => animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        addHealth.Init();
        self = this;
    }

    public override void OnClose()
    {
        Interactable.CurrentMode = Interactable.Mode.game;
    }

    public override void OnOpen()
    {
        Interactable.CurrentMode = Interactable.Mode.canvas;
    }
}
