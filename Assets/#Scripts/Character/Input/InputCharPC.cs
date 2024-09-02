using UnityEngine;

public class InputCharPC : InputBase
{
    private readonly CharManager character;

    private AnimState state;
    private AnimState stateRecoard;

    public InputCharPC(CharManager _mono) : base(_mono)
    {
        character = _mono;
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

            character.Animator.SetInteger("State", (int)state);
        }
    }

    public override void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.Q)) character.SetTarget();

        CheckState();
    }

    private void CheckState()
    {
        stateRecoard = AnimState.Idle;

        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0) // 이동
        {
            if (Input.GetKey(KeyCode.LeftAlt)) stateRecoard = AnimState.Walk;
            else stateRecoard = AnimState.Run;
        }

        if (Input.GetMouseButton(0)) // 공격
        {
            stateRecoard = AnimState.Attack;
        }

        if (Input.GetMouseButton(1)) // 방어
        {
            stateRecoard = AnimState.Guard;
        }

        if (Input.GetKeyDown(KeyCode.Space)) // 구르기
        {
            stateRecoard = AnimState.Roll;
        }

        State = stateRecoard;
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
