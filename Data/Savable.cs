public class Savable<T>
{
    protected T value;
    protected string keyName;

    public Savable(T defaultValue, string keyName)
    {
        this.keyName = keyName;

        if (!DataSaver.HasKey(keyName)) {
            value = defaultValue;
            DataSaver.SetKey(keyName, value);
        }
        else
            value = (T)DataSaver.GetKey(keyName);
    }

    public T Get()
    {
        return value;
    }

    public Savable<T> Set(T value)
    {
        this.value = value;
        DataSaver.SetKey(keyName, value);
        return this;
    }

    public static implicit operator T(Savable<T> obj)
    {
        return obj.Get();
    }
}
