using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialMonsterBehaviour : MonoBehaviour
{
    protected const string blend_tree_parameter = "difficulty";

    [SerializeField]
    protected Mode mode;
    [SerializeField]
    protected Animator animator;

    protected int state { get; set; } = 0;

    public void Init(Monster monster)
    {
        monster.HealthReduced += OnGetDamage;
    }

    protected virtual void FirstEvent()
    {
        animator.SetFloat(blend_tree_parameter, 1);   
    }

    protected virtual void SecondEvent()
    {
        animator.SetFloat(blend_tree_parameter, 0);
    }

    protected void OnGetDamage(int obj)
    {
        if (state == 0) {
            FirstEvent();
            state = 1;
            return;
        }
        if (state == 1 && mode == Mode.InMidle) {
            SecondEvent();
            state = 2;
            return;
        }
    }

    public enum Mode
    {
        AfterDamage,
        InMidle
    }
}
