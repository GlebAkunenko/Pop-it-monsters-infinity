using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class Pimple : Interactable
{
    private bool dimple = false;
    public bool Dimple
    {
        get { return dimple; }
        set
        {
            if (dimple == value)
                return;

            if (value == true && pimpleAnimGo)
                return;

            dimple = value;
            if (dimple)
            {
                StartCoroutine(DimpleAnim());
                if (popSounds.Length > 0)
                    audio.PlayOneShot(popSounds[Random.Range(0, popSounds.Length)]);
                popit.DimpleCount++;
            }
            else
            {
                StartCoroutine(PimpleAnim());
                popit.DimpleCount--;
            }
        }
        
    }

    private SpriteRenderer spriteRenderer;
    private Image image;
    private new AudioSource audio;

    public static Camera Camera { get; set; }

    [SerializeField] private bool paintOnStart = true; 
    [SerializeField] private bool popOnTouchUpOnly = false; 
    [SerializeField] private float time;
    [SerializeField] private Sprite[] dimpleSprites;
    [SerializeField] private Sprite[] pimpleSprites;

    public AudioClip[] popSounds;

    private bool pimpleAnimGo = false;

    private IEnumerator DimpleAnim()
    {
        foreach(Sprite sprite in dimpleSprites)
        {
            if (pimpleAnimGo)
                yield break;
            if (spriteRenderer != null)
                spriteRenderer.sprite = sprite;
            else
                image.sprite = sprite;
            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator PimpleAnim()
    {
        pimpleAnimGo = true;
        foreach (Sprite sprite in pimpleSprites)
        {
            if (spriteRenderer != null)
                spriteRenderer.sprite = sprite;
            else
                image.sprite = sprite;
            yield return new WaitForSeconds(time);
        }
        pimpleAnimGo = false;
    }

    [SerializeField]
    private Popit popit;

    private new void Start()
    {
        base.Start();

        if (spriteRenderer == null) {
            spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer == null)
                image = GetComponent<Image>();
        }

        audio = GetComponent<AudioSource>();
        popit.AddPimple(this);

        if (Camera == null)
            Camera = Camera.main;

        if (paintOnStart)
            StartCoroutine(LatePaint());
    }

    private IEnumerator LatePaint()
    {
        yield return new WaitForEndOfFrame();
        if (spriteRenderer != null)
            spriteRenderer.color = popit.Color;
        else
            image.color = popit.Color;
    }

    public void Paint(Color color)
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
            spriteRenderer.color = color;
        else if (image != null)
            image.color = color;
    }

    protected override void OnClick()
    {
        if (!popOnTouchUpOnly)
            Dimple = true;
    }

    protected override void OnTouchUp()
    {
        if (popOnTouchUpOnly)
            Dimple = true;
    }
}
