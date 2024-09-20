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
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (character.LookTarget == null)
            {
                if (Physics.Raycast(GameManager._instance.Cam.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 200, character.Mask)) character.LookTarget = hit.transform.GetComponent<IndividualBase>();
            }
            else character.LookTarget = null;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (character.specificInfo.potion[1].Data >= 100) character.specificInfo.potion[0].Data -= 100;
        }

        CheckState();
    }

    private void CheckState()
    {
        if (!character.UseEnergy(character.AnimStateBase.State, true))
        {
            character.AnimStateBase.StateRecord = AnimState.Idle;
        }
        else
        {
            switch (character.AnimStateBase.State)
            {
                case AnimState.Idle:
                case AnimState.Walk:
                    if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0) // 이동
                    {
                        if (Input.GetKey(KeyCode.LeftAlt)) character.Animator.SetFloat("MoveSpeed", 0.5f, 1f, 0.1f);
                        else character.Animator.SetFloat("MoveSpeed", character.commonInfo.moveSpeed.Data, 1f, 0.1f);

                        character.AnimStateBase.StateRecord = AnimState.Walk;
                    }
                    else character.AnimStateBase.StateRecord = AnimState.Idle;
                    break;

                case AnimState.Attack:
                    if (!Input.GetMouseButton(0)) character.AnimStateBase.StateRecord = AnimState.Idle;
                    break;

                case AnimState.Guard:
                    if (!Input.GetMouseButton(1)) character.AnimStateBase.StateRecord = AnimState.Idle;
                    break;

                case AnimState.Roll:
                    if (character.AnimStateBase.roll) character.AnimStateBase.StateRecord = AnimState.Idle;
                    break;
            }
        }

        if (Input.GetMouseButtonDown(0)) // 공격
        {
            if (character.UseEnergy(AnimState.Attack, true)) character.AnimStateBase.StateRecord = AnimState.Attack;
        }

        if (Input.GetMouseButtonDown(1)) // 방어
        {
            if (character.UseEnergy(AnimState.Guard, true)) character.AnimStateBase.StateRecord = AnimState.Guard;
        }

        if (Input.GetKeyDown(KeyCode.Space)) // 구르기
        {
            if (character.UseEnergy(AnimState.Roll, true)) character.AnimStateBase.StateRecord = AnimState.Roll;
        }

        character.AnimStateBase.State = character.AnimStateBase.StateRecord;
    }
}
