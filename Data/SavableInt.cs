public class SavableInt : Savable<int>
{
    public SavableInt(int defaultValue, string keyName)
        : base(defaultValue, keyName)
    {

    }

    public static SavableInt operator+(SavableInt obj, int number)
    {
        obj.Set(obj.Get() + number);
        return obj;
    }

    public static SavableInt operator -(SavableInt obj, int number)
    {
        obj.Set(obj.Get() - number);
        return obj;
    }

    public static SavableInt operator++(SavableInt obj)
    {
        return (SavableInt)obj.Set(obj.Get() + 1);
    }

}