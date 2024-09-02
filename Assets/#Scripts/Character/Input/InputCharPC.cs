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
        character.AnimState.StateRecord = AnimState.Idle;

        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0) // �̵�
        {
            if (Input.GetKey(KeyCode.LeftAlt)) character.AnimState.StateRecord = AnimState.Walk;
            else character.AnimState.StateRecord = AnimState.Run;
        }

        if (Input.GetMouseButton(0)) // ����
        {
            character.AnimState.StateRecord = AnimState.Attack;
        }

        if (Input.GetMouseButton(1)) // ���
        {
            character.AnimState.StateRecord = AnimState.Guard;
        }

        if (Input.GetKeyDown(KeyCode.Space)) // ������
        {
            character.AnimState.StateRecord = AnimState.Roll;
        }

        character.AnimState.State = character.AnimState.StateRecord;
    }
}
