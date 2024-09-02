using UnityEngine;

public class AnimStateBase
{
    public bool roll;

    private AnimState state;
    private AnimState stateRecord;

    private readonly Animator animator;

    public AnimStateBase(Animator _anim)
    {
        animator = _anim;
    }

    public AnimState State
    {
        get
        {
            return state;
        }
        set
        {
            if (state == value) return;

            state = value;

            animator.SetInteger("State", (int)state);
        }
    }

    public AnimState StateRecord
    {
        get
        {
            return stateRecord;
        }
        set
        {
            stateRecord = value;
        }
    }
}

public enum AnimState
{
    Idle,
    Attack,
    Walk,
    Guard,
    Run,
    Roll
}
