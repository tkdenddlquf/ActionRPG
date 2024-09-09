using UnityEngine;
using System.Collections.Generic;

public class HitBox : MonoBehaviour
{
    public List<Vector3> pos = new() { new(0, 0, 0) };
    public List<Vector3> size = new() { new(1, 1, 1) };

#if UNITY_EDITOR
    private int select = 0;

    public int Select
    {
        get
        {
            return select;
        }

        set
        {
            if (value < 0) return;
            if (value > pos.Count) return;

            select = value;
        }
    }

    public Vector3 this[GUIState _state]
    {
        get
        {
            return _state switch
            {
                GUIState.Move => pos[Select],
                GUIState.Size => size[Select],
                _ => Vector3.zero
            };
        }
        set
        {
            switch (_state)
            {
                case GUIState.Move:
                    pos[Select] = value;
                    break;

                case GUIState.Size:
                    size[Select] = value;
                    break;
            }
        }
    }
#endif
}