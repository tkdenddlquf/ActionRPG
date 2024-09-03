using System;
using UnityEngine;

[Serializable]
public class BindData<T>
{
    [SerializeField] private T data;

    private Bind bind;

    public T Data
    {
        get
        {
            return data;
        }
        set
        {
            if (bind == null) data = value;
            else bind(ref data, value);
        }
    }

    public void SetData(T _data)
    {
        data = _data;
    }

    public void SetBind(Bind _bind)
    {
        bind = _bind;
        bind(ref data, data);
    }

    public delegate void Bind(ref T _current, T _change);
}
