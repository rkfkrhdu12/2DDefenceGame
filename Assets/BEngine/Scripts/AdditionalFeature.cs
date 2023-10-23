using System;

[Serializable]
public class DataPointer<T>
{
    [UnityEngine.SerializeField]
    public T Data;

    public static implicit operator T(DataPointer<T> d)
    {
        return d.Data;
    }
}

[Serializable]
public class DataReferencer<T>
{
    public T Data { get { return _dataPtr.Data; } }

    [UnityEngine.SerializeField]
    protected DataPointer<T> _dataPtr;

    public static implicit operator T(DataReferencer<T> d)
    {
        return d.Data;
    }

    public void Set(ref DataPointer<T> data) => _dataPtr = data;
}

public class DataReferencers<T>
{
    public DataReferencers()
    {
        for (int i = 0; i < (int)EKeyState.Last; ++i)
        {
            Array[i] = new DataReferencer<T>();
        }
    }

    readonly DataReferencer<T>[] Array = new DataReferencer<T>[(int)EKeyState.Last];

    public DataReferencer<T> this[EKeyState keyState]
    {
        get { return Array[(int)keyState]; }
    }
    public DataReferencer<T> this[int key]
    {
        get { return Array[key]; }
    }
}
