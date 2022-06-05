using System;
using UnityEngine;
using UnityEditor;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AudioSource))]
public class Monster : Popit, IBarElement
{
    protected Animator anim;
    protected new AudioSource audio;

    [SerializeField]
    private SpriteRenderer modelRenderer;
    [SerializeField]
    private SpecialMonsterBehaviour specialBehaviour;
    [SerializeField]
    private InfMonsterGuider monsterGuider;

    [SerializeField]
    private Rarity rarity;
    [SerializeField]
    [Range(1, 3)]
    private int health;

    [SerializeField]
    private Sprite barIcon;
    [SerializeField]
    private Sprite whitePartOfBarIcon;
    [SerializeField]
    private Sprite bookIcon;

    [SerializeField]
    private ColorSet suitableColores;

    protected MonsterSounds sounds;

    public GameObject SpawnPrefab => transform.parent.gameObject;

    public Sprite ColoredBarIcon => barIcon;
    public Sprite WhiteBarIcon => whitePartOfBarIcon;
    public Sprite BookIcon => bookIcon;
    public Color BarIconColor => Color;

    public Rarity Rarity => rarity;
    public virtual int Health { get => health; set => health = value; }

    private void Start()
    {
        anim = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
        sounds = GetComponent<MonsterSounds>();
        sounds.Init(audio);
        sounds.PlayAwake();

        if (specialBehaviour != null)
            specialBehaviour.Init(this);
    }

    protected override void OnFull()
    {
        Health--;
        HealthReduced?.Invoke(Health);
        if (Health == 0) {
            anim.SetTrigger("die");
            sounds.PlayDeath();
        }
        else UpdatePimples();
    }

    public void OnSpawn()
    {
        Spawned?.Invoke();
        sounds.PlaySpawn();
    }

    protected virtual void OnDead()
    {
        Dead?.Invoke();
        Destroy(transform.parent.gameObject);
    }

    public override void Paint(Color color)
    {
        base.Paint(color);
    }

    // for testing or debug
    public void Kill()
    {
        Health = 1;
        OnFull();
    }

    public Color GetAutoColor(int seed, ColorType colorType)
    {
        Color[] colors = suitableColores.GetColorsByType(colorType);
        return colors[Random.Range(0, colors.Length)];
    }

    public void Damage()
    {
        PlayerInfo.Self.GetDamage();
        anim.SetTrigger("hit");
        sounds.PlayHit();
    }

    public void Damage(AudioClip sound)
    {
        PlayerInfo.Self.GetDamage();
        anim.SetTrigger("hit");
        sounds.PlayHit(sound);
    }

    public void RenameLinkedFiles()
    {
#if UNITY_EDITOR
        string oldName = AssetDatabase.GetAssetPath(bookIcon);
        AssetDatabase.RenameAsset(oldName, SpawnPrefab.name);
#endif
    }

    public void UpdateData()
    {
#if UNITY_EDITOR

#endif
    }

    public event Action<int> HealthReduced;
    public event Action Dead;
    public event Action Spawned;
}
