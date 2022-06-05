using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfMonsterGuider : MonoBehaviour
{
    public void OnEndHandAnimation()
    {
        if (Interactable.CurrentMode != Interactable.Mode.pause)
            Interactable.CurrentMode = Interactable.Mode.game;
        Destroy(gameObject);
    }
}
