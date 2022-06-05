using System;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class MonstersStack : IStack<Monster>
{
    private bool timeLevel;
    private int stackSize;

    private Monster[] allMonsters;
    private Monster last;
    private Random random;
    private List<Color> colorStack;

    public override int offset => throw new NotSupportedException();

    private Monster GetNextMonster()
    {
        Monster result = allMonsters[random.Next(0, allMonsters.Length)];
        if (last == null || allMonsters.Length < 2)
            return last = result;
        while (last == result)
            result = allMonsters[random.Next(0, allMonsters.Length)];
        return last = result;
    }

    public MonstersStack(Monster[] allMonsters, int seed, int offset, int stackSize)
    {
        this.stackSize = stackSize;
        this.allMonsters = allMonsters;

        random = new Random(seed);
        for (int i = 0; i < offset; i++)
            GetNextMonster();
    }

    public void MakeStack()
    {
        CurrentStack = new List<Monster>();
        for (int i = 0; i < stackSize; i++)
            CurrentStack.Add(GetNextMonster());
    }

    public override Monster PopElementAndPushStack()
    {
        Monster ret = CurrentStack[0];
        CurrentStack.RemoveAt(0);
        CurrentStack.Add(GetNextMonster());
        return ret;
    }
}


/*
 * P.S. (rus)
 * По данным теста на моём компьютере в editor mode цикл 
 * 
 * for (int i = 0; i < 100.000; i++) 
 *      random.Naxt()
 *      
 * давал задержку < 6 мс. Думаю этот приём не повредит оптимизации.
 */