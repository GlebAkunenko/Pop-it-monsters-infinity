using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingStack : IStack<IBarElement>
{
    private int _offset;

    public TrainingStack(List<IBarElement> barElements, int offset)
    {
        CurrentStack = barElements;

        for (int i = 0; i < offset; i++)
            CurrentStack.RemoveAt(0);
    }

    public override int offset => _offset;

    public override IBarElement PopElementAndPushStack()
    {
        IBarElement ret = CurrentStack[0];
        CurrentStack.RemoveAt(0);
        _offset++;
        return ret;
    }
}
