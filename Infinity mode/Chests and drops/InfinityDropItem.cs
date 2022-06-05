using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InfinityDropItem : MonoBehaviour
{
    protected Animator anim;
    protected int state = 0;

    public virtual void Init(LootInfo lootInfo)
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.touchCount > 0 && state == 1) {
            OnTouch();
        }
    }

    protected virtual void OnTouch()
    {
        state++;
        anim.SetTrigger("exit");
    }

    protected abstract void UpdatePlayerData();

    public void EndSpawning()
    {
        state++;
    }

    public void OnEndMoving()
    {
        UpdatePlayerData();
        EndMoving?.Invoke();
        Destroy(gameObject);
    }

    public event System.Action EndMoving;
}
