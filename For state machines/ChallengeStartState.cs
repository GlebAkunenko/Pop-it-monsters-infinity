using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallengeStartState : StateMachineBehaviour
{
    [SerializeField]
    private string monstersCountParamName = "monstersCount";

    private Animator animator;

    private int MonstersCount
    {
        get => animator.GetInteger(monstersCountParamName);
        set => animator.SetInteger(monstersCountParamName, value);
    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        this.animator = animator;
        Challange challange = MetaSceneData.Challange;

        MonstersCount = challange.Stack.Count;
    }
}
