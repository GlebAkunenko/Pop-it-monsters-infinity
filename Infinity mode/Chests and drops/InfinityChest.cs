using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class InfinityChest : MonoBehaviour
{
    [SerializeField]
    private Transform canvas;

    private LootInfo loot;

    private Animator animator;
    private AudioSource audio;
    private Vector3 spawnItemPosition;
    private int currentItemIndex = -1;

    public void Init(IChestInfo chestInfo, PageCollection collection)
    {
        audio = GetComponent<AudioSource>();

        ChestModel chestModel = Instantiate(chestInfo.ChestModel, transform.position, Quaternion.identity, transform)
            .GetComponent<ChestModel>();
        animator = chestModel.Animator;
        spawnItemPosition = chestModel.SpawnItemPosition;
        chestModel.PopAllPimples += Open;
        chestModel.Open += SpawnNextItem;

        chestInfo.Init(collection);
        loot = chestInfo.Unpack();
    }

    private void SpawnNextItem()
    {
        currentItemIndex++;
        GameObject o = Instantiate(loot.lootObjects[currentItemIndex], spawnItemPosition, Quaternion.identity, canvas);
        InfinityDropItem drop = o.GetComponent<InfinityDropItem>();
        drop = o.GetComponents<InfinityDropItem>()[0]; // because InfinityDropItem is abstract
        drop.Init(loot);
        if (currentItemIndex + 1 < loot.lootObjects.Count)
            drop.EndMoving += SpawnNextItem;
        else {
            drop.EndMoving += () => { Finished?.Invoke(); };
            if (loot.music != null) {
                BackMusic.self.MuteMusicForTime(loot.music.length);
                audio.PlayOneShot(loot.music);
                MetaSceneData.UnlockNewMonster = true;
            }
        }
        animator.SetTrigger("drop");
    }

    private void Open()
    {
        animator.SetTrigger("open");
    }

    public event System.Action Finished;
}
