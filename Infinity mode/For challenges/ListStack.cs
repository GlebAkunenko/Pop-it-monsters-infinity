using System.Collections.Generic;

public class ListStack<T> : IStack<T>
{
    private int _offset = 0;

    public ListStack(ICollection<T> stack)
    {
        CurrentStack = new List<T>(stack);
    }

    public override int offset => _offset;

    public override T PopElementAndPushStack()
    {
        _offset++;
        T ret = CurrentStack[0];
        CurrentStack.RemoveAt(0);
        return ret;
    }
}
