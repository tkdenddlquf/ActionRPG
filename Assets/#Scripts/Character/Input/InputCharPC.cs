using UnityEngine;

public class InputCharPC : InputBase
{
    private readonly CharManager character;

    public InputCharPC(CharManager _mono) : base(_mono)
    {
        character = _mono;
    }

    public override void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.Q)) character.SetTarget();

        CheckState();
    }

    private void CheckState()
    {
        switch (character.AnimState.StateRecord)
        {
            case AnimState.Idle:
                if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0) // 이동
                {
                    if (Input.GetKey(KeyCode.LeftAlt)) character.AnimState.StateRecord = AnimState.Walk;
                    else character.AnimState.StateRecord = AnimState.Run;
                }
                break;

            case AnimState.Walk:
            case AnimState.Run:
                if (Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0) character.AnimState.StateRecord = AnimState.Idle;
                break;

            case AnimState.Attack:
                if (!Input.GetMouseButton(0)) character.AnimState.StateRecord = AnimState.Idle;
                break;

            case AnimState.Guard:
                if (!Input.GetMouseButton(1)) character.AnimState.StateRecord = AnimState.Idle;
                break;

            case AnimState.Roll:
                if (character.AnimState.roll) character.AnimState.StateRecord = AnimState.Idle;
                break;
        }

        if (Input.GetMouseButtonDown(0)) // 공격
        {
            character.AnimState.StateRecord = AnimState.Attack;
        }

        if (Input.GetMouseButtonDown(1)) // 방어
        {
            character.AnimState.StateRecord = AnimState.Guard;
        }

        if (Input.GetKeyDown(KeyCode.Space)) // 구르기
        {
            character.AnimState.StateRecord = AnimState.Roll;
        }

        character.AnimState.State = character.AnimState.StateRecord;
    }
}
