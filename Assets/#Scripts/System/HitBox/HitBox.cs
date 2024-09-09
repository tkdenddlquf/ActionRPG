using UnityEngine;
using System.Collections.Generic;

public class HitBox : MonoBehaviour
{
    public List<int> maxHitCount = new() { 1 };

    public List<Vector3> pos = new() { new(0, 0, 0) };
    public List<Vector3> scale = new() { new(1, 1, 1) };

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
            if (value > pos.Count - 1) return;

            select = value;
        }
    }

    public Vector3 this[GUIMode _state]
    {
        get
        {
            return _state switch
            {
                GUIMode.Pos => pos[Select],
                GUIMode.Scale => scale[Select],
                _ => Vector3.zero
            };
        }
        set
        {
            switch (_state)
            {
                case GUIMode.Pos:
                    pos[Select] = value;
                    break;

                case GUIMode.Scale:
                    scale[Select] = value;
                    break;
            }
        }
    }
#endif
}