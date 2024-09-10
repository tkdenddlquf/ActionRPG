using UnityEngine;

public abstract class InputBase
{
    protected MonoBehaviour mono;

    protected InputBase(MonoBehaviour _mono)
    {
        mono = _mono;
    }

    public abstract void CheckInput();
}
