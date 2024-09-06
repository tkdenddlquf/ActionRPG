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
                hitBase.commonInfo.energy[0].Data -= 10;

                hitBase.transform.rotation = Quaternion.Slerp(hitBase.transform.rotation, Angle, 0.05f);
                break;

            case AnimState.Guard:
                hitBase.AnimState.guard = true;
                break;

            case AnimState.Roll:
                hitBase.AnimState.roll = true;
                time = 0;

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
            case AnimState.Attack:
                hitBase.CheckHit();
                break;

            case AnimState.Walk:
                if (MoveDir != Vector3.zero)
                {
                    hitBase.transform.rotation = Quaternion.Slerp(hitBase.transform.rotation, Quaternion.LookRotation(MoveDir) * Angle, 0.1f);
                    hitBase.Rigidbody.AddRelativeForce(50 * hitBase.commonInfo.moveSpeed.Data * Vector3.forward);
                }
                break;

            case AnimState.Run:
                if (MoveDir != Vector3.zero)
                {
                    hitBase.transform.rotation = Quaternion.Slerp(hitBase.transform.rotation, Quaternion.LookRotation(MoveDir) * Angle, 0.1f);
                    hitBase.Rigidbody.AddRelativeForce(100 * hitBase.commonInfo.moveSpeed.Data * Vector3.forward);
                }
                break;

            case AnimState.Roll:
                if (_stateInfo.normalizedTime < 0.5f) time += Time.deltaTime;
                else time -= Time.deltaTime;

                hitBase.Rigidbody.AddRelativeForce(1200f * time * Vector3.forward);
                break;
        }
    }

    public override void OnStateExit(Animator _animator, AnimatorStateInfo _stateInfo, int _layerIndex)
    {
        switch (thisState)
        {
            case AnimState.Guard:
                if (hitBase.AnimState.State != thisState) hitBase.AnimState.guard = false;
                break;

            case AnimState.Roll:
                hitBase.AnimState.roll = false;
                break;
        }
    }
}
