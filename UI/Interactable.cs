using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public static Mode CurrentMode { get; set; } = Mode.game;
    public Mode Type { get => type; set => type = value; }

    protected Camera main;

    [SerializeField]
    protected Collider2D touchCollider;
    protected new Transform transform;

    [SerializeField]
    private Mode type;

    protected void Start()
    {
        main = Camera.main;
        transform = GetComponent<Transform>();
    }

    private bool CheckInclude(Touch touch)
    {
        Vector3 pos = main.ScreenToWorldPoint(touch.position);
        return touchCollider.bounds.Contains(new Vector3(pos.x, pos.y, transform.position.z));
    }

    protected void Update()
    {
        if (CurrentMode == type) {
            foreach (Touch touch in Input.touches) {
                bool has_cache = false, cache = false;
                if (touch.phase == TouchPhase.Began) {
                    has_cache = true;
                    cache = CheckInclude(touch);
                    if (cache) {
                        OnClick();
                        break;
                    }
                    else continue;
                }
                else if (touch.phase == TouchPhase.Ended) {
                    if (!has_cache)
                        cache = CheckInclude(touch);
                    if (cache) {
                        OnTouchUp();
                        break;
                    }
                }
            }
        }
    }


    protected abstract void OnClick();

    protected virtual void OnTouchUp() { }

    public enum Mode
    {
        game,
        canvas,
        pause,
        none
    }
}
