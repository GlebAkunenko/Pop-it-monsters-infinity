using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonGuider : Guider
{
    [SerializeField]
    private PopButton button;

    public override void Init()
    {
        if (!MetaSceneData.IsGuideShown(guideName)) {
            button.OnClick.AddListener(EndGuide);
            base.Init();
        }
        else Destroy(gameObject);
    }
}
