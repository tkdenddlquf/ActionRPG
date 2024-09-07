using UnityEngine;

public class AnimStateMachine : StateMachineBehaviour
{
    public AnimState thisState;

    private float time;
    private Transform look;
    private HitBase hitBase;

    private Vector3 MoveDir => look == hitBase.transform ? Vector3.forward : new(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
    private Quaternion Angle => Quaternion.LookRotation(new Vector3(look.forward.x, 0, look.forward.z));

    public void Init(Transform _look, HitBase _base)
    {
        look = _look;
        hitBase = _base;
    }

    public override void OnStateEnter(Animator _animator, AnimatorStateInfo _stateInfo, int _layerIndex)
    {
        switch (thisState)
        {
            case AnimState.Attack:
                hitBase.SetGuid();
                hitBase.UseEnergy(thisState);

                hitBase.transform.rotation = Quaternion.Slerp(hitBase.transform.rotation, Angle, 0.05f);
                break;

            case AnimState.Guard:
                hitBase.AnimStateBase.guard = true;
                break;

            case AnimState.Roll:
                hitBase.AnimStateBase.roll = true;
                time = 0;

                hitBase.UseEnergy(thisState);

                if (MoveDir != Vector3.zero) hitBase.transform.rotation = Quaternion.LookRotation(MoveDir) * Angle;
                break;

            case AnimState.Hit:
                _animator.SetBool("Hit", false);
                break;
        }
    }

    public override void OnStateUpdate(Animator _animator, AnimatorStateInfo _stateInfo, int _layerIndex)
    {
        switch (thisState)
        {
            case AnimState.Idle:
                if (MoveDir == Vector3.zero)
                {
                    if (_stateInfo.normalizedTime > 1f) hitBase.UseEnergy(thisState);
                }
                else
                {
                    hitBase.transform.rotation = Quaternion.Slerp(hitBase.transform.rotation, Quaternion.LookRotation(MoveDir) * Angle, 0.2f);
                    hitBase.Rigidbody.AddRelativeForce(100 * _animator.GetFloat("MoveSpeed") * hitBase.commonInfo.moveSpeed.Data * Vector3.forward);
                }
                break;

            case AnimState.Attack:
                hitBase.CheckHit();
                break;

            case AnimState.Guard:
                hitBase.UseEnergy(thisState);
                break;

            case AnimState.Roll:
                if (_stateInfo.normalizedTime < 0.5f) time += Time.deltaTime;
                else time -= Time.deltaTime;

                hitBase.Rigidbody.AddRelativeForce(1300f * time * Vector3.forward);
                break;
        }
    }

    public override void OnStateExit(Animator _animator, AnimatorStateInfo _stateInfo, int _layerIndex)
    {
        switch (thisState)
        {
            case AnimState.Guard:
                if (hitBase.AnimStateBase.State != thisState) hitBase.AnimStateBase.guard = false;
                break;

            case AnimState.Roll:
                hitBase.AnimStateBase.roll = false;
                break;
        }
    }
}
