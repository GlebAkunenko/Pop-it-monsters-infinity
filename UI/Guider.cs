using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Guider : MonoBehaviour
{
    [SerializeField]
    protected Guide guideName;

    [SerializeField]
    protected bool showOnStart;

    protected Animator animator;

    public virtual void Init()
    {
        animator = GetComponent<Animator>();
        animator.speed = 0;

        if (showOnStart && !MetaSceneData.IsGuideShown(guideName))
            StartGuide();
    }

    public virtual void StartGuide()
    {
        switch (guideName) {
            case Guide.map:
                transform.position = CurrentPoint.Self.transform.position;
                CurrentPoint.Self.click += EndGuide;
                break;
        }

        animator.speed = 1;
    }

    public virtual void ContinueGuide()
    {

    }

    protected virtual void EndGuide()
    {
        MetaSceneData.ShowGuide(guideName);
        Destroy(gameObject);
    }
}

public enum Guide
{
    map,
    monster,
    lose,
    loseWithAds
}