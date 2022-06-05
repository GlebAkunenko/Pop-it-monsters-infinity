using System.Collections;
using System.Collections.Generic;

public class InfinityLevelStack
{
    private int barSize;
    private int offset;

    private MonstersStack monstersStack;
    private MonsterColorBuilder colorBuilder;
    private IStack<InfChestLoot> chestStack;
    private ChestStep chestStep;

    public List<IBarElement> CurrentStack { get; set; } = new List<IBarElement>();

    public InfinityLevelStack(MonstersStack monstersStack, MonsterColorBuilder colorBuilder, IStack<InfChestLoot> chestStack, int offset, ChestStep chestStep, int barSize)
    {
        this.monstersStack = monstersStack;
        this.colorBuilder = colorBuilder;
        this.chestStack = chestStack;
        this.chestStep = chestStep;
        this.barSize = barSize;
        this.offset = offset;

        chestStep.Init();
    }

    private IBarElement GetNewStackElement(int index)
    {
        if (chestStep.CheckPosition(index))
            return chestStack.PopElementAndPushStack();

        Monster monster = monstersStack.PopElementAndPushStack();
        return new MonsterBarElement(monster, colorBuilder.GetMonsterColor(monster));
    }

    public void MakeStack()
    {
        if (CurrentStack.Count > 0) 
            CurrentStack.Clear();
        for (int i = 0; i < barSize; i++)
            CurrentStack.Add(GetNewStackElement(++offset));
    }

    public IBarElement PopElementAndPushStack()
    {
        IBarElement ret = CurrentStack[0];
        CurrentStack.RemoveAt(0);
        CurrentStack.Add(GetNewStackElement(++offset));
        return ret;
    }


}

public abstract class IStack<T>
{
    public List<T> CurrentStack { get; protected set; }

    public abstract int offset { get; }

    public abstract T PopElementAndPushStack();

    public virtual bool IsEmpty => CurrentStack.Count == 0;
}
