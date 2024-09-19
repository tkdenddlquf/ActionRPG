using UnityEngine;
using System.Collections.Generic;
using System;

public class BoxCollider_Custum : MonoBehaviour
{
    public List<int> maxCount = new() { 1 };

    public List<Vector3> pos = new() { new(0, 0, 0) };
    public List<Vector3> scale = new() { new(1, 1, 1) };

    public Action<BoxCollider_CustomInfo> callback;

    private int length;
    private readonly Collider[] colliders = new Collider[5];

    public void CheckHit(int _index, LayerMask _mask) // 공격 확인
    {
        length = Physics.OverlapBoxNonAlloc(transform.position + transform.TransformDirection(pos[_index]), scale[_index], colliders, Quaternion.identity, _mask);

        for (int i = 0; i < length; i++) callback(new(colliders[i].gameObject, _index, maxCount[_index]));
    }

#if UNITY_EDITOR
    private int select = 0;

    public int Select
    {
        get
        {
            if (select < 0) select = 0;
            if (select > pos.Count - 1) select = pos.Count - 1;
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

public struct BoxCollider_CustomInfo
{
    public GameObject target;

    public int index;
    public int maxCount;

    public BoxCollider_CustomInfo(GameObject _target, int _index, int _maxCount)
    {
        target = _target;
        index = _index;
        maxCount = _maxCount;
    }
}