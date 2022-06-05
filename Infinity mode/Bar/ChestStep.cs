using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Chest steps", menuName = "Chest step", order = 51)]
public class ChestStep : ScriptableObject
{
    [Tooltip("First number: chest count\n" +
        "Second number: between monsters count")]
    [SerializeField]
    private Vector2Int[] chestsAndSteps;
    [SerializeField]
    private int finallyChestStep;

    private List<int> chestPositions = new List<int>();
    private int max = 0;
    private bool inited;

    private void SolveMax()
    {
        foreach (int pos in chestPositions)
            max = Mathf.Max(max, pos);
    }

    public void Init()
    {
        int last = -1;
        for(int v = 0; v < chestsAndSteps.Length; v++) {
            for (int i = 0; i < chestsAndSteps[v].x; i++)
                chestPositions.Add(last = last + chestsAndSteps[v].y + 1);
        }
        SolveMax();
        inited = true;
    }

    public bool CheckPosition(int positnionIndex)
    {
        if (!inited)
            throw new System.Exception("Chest step not inited");

        if (chestPositions.Contains(positnionIndex))
            return true;
        if (positnionIndex > max)
            return (positnionIndex - max) % finallyChestStep == 0;
        return false;
    }
}
