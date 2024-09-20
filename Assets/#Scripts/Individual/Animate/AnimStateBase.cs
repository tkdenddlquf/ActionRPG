using UnityEngine;

public class AnimStateBase
{
    public bool roll;
    public bool guard;

    private AnimState state;

    private readonly Animator animator;

    public AnimStateBase(Animator _anim)
    {
        animator = _anim;
    }

    public AnimState State
    {
        get => state;
        set
        {
            if (state == value) return;

            state = value;

            animator.SetInteger("State", (int)state);
        }
    }

    public AnimState StateRecord { get; set; }
}

public enum AnimState
{
    Idle,
    Walk,
    Attack,
    Guard,
    Roll,
    Hit,
    UsePotion
}
