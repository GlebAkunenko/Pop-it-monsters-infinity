using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfinityBar : MonoBehaviour
{
    [Header("Blocks settings")]
    [SerializeField]
    private float blockDistance = 120;
    [SerializeField]
    private int blocksCount;

    [Header("Animation")]
    [SerializeField]
    private int stepsCount;
    [SerializeField]
    private float time;

    [SerializeField]
    private Transform moveBlocksPrivot;
    [SerializeField]
    private GameObject blockPrefab;

    private bool inited = false;
    private bool pushing = false;

    private Vector3 baseMoveBlocksPrivotPostion;
    private List<InfinityBarBlock> blocks = new List<InfinityBarBlock>();

    public void Init(bool disableBlockPrivot = false)
    {
        baseMoveBlocksPrivotPostion = moveBlocksPrivot.position;
        for (int i = 0; i < blocksCount; i++)
            blocks.Add(CreateBlock(i));

        if (disableBlockPrivot)
            moveBlocksPrivot.gameObject.SetActive(false);

        inited = true;
    }

    private InfinityBarBlock CreateBlock(int position_offset)
    {
        GameObject newO = Instantiate(
            blockPrefab,
            baseMoveBlocksPrivotPostion,
            Quaternion.identity,
            moveBlocksPrivot);
        newO.transform.localPosition += new Vector3(position_offset * blockDistance, 0, 0);
        return newO.GetComponent<InfinityBarBlock>();
    }

    private IEnumerator BarPushing(IBarElement[] newStack)
    {
        pushing = true;

        float step = blockDistance / (float)stepsCount;
        float deltaTime = time / stepsCount;

        Vector3 move = new Vector3(-step, 0, 0);

        for(int i = 0; i < stepsCount; i++) {
            moveBlocksPrivot.localPosition += move;
            yield return new WaitForSeconds(deltaTime);
        }

        UpdateBlocksList();
        DrawBar(newStack);

        pushing = false;
    }

    private void UpdateBlocksList()
    {
        Destroy(blocks[0].gameObject);
        blocks.RemoveAt(0);
        blocks.Add(CreateBlock(blocksCount - 1));
    }

    /// <param name="stack">Monsters and chests stack</param>
    public void DrawBar(List<IBarElement> stack)
    {
        if (!inited)
            throw new System.Exception("Bar is not inited");

        for (int i = 0; i < Mathf.Min(stack.Count, blocks.Count); i++)
            blocks[i].Draw(stack[i]);
    }

    /// <param name="stack">Monsters and chests stack</param>
    public void DrawBar(IBarElement[] stack)
    {
        if (!inited)
            throw new System.Exception("Bar is not inited");

        for (int i = 0; i < Mathf.Min(stack.Length, blocks.Count); i++)
            blocks[i].Draw(stack[i]);

    }

    public void PushBar(List<IBarElement> newStack)
    {
        if (!inited)
            throw new System.Exception("Bar is not inited");

        if (pushing)
            throw new System.Exception("Bar is pushing already");

        StartCoroutine(BarPushing(newStack.ToArray()));
        // we use .ToArray() to copy newStack value to draw a this moment stack
    }

    public void AnimateSpawnBlocks()
    {
        moveBlocksPrivot.gameObject.SetActive(true);
        moveBlocksPrivot.GetComponent<Animation>().Play();
    }

}

public interface IBarElement
{
    public Sprite ColoredBarIcon { get; }

    public Sprite WhiteBarIcon { get; }

    public Color BarIconColor { get; }
}
