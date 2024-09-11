using UnityEngine;

public class AnimStateMachine : StateMachineBehaviour
{
    public AnimState thisState;

    private float time;
    private Transform look;
    private IndividualBase hitBase;

    private Vector3 MoveDir => look == hitBase.transform ? Vector3.forward : new(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
    private Quaternion Angle => Quaternion.LookRotation(new Vector3(look.forward.x, 0, look.forward.z));

    public override void OnStateEnter(Animator _animator, AnimatorStateInfo _stateInfo, int _layerIndex)
    {
        if (hitBase == null)
        {
            _animator.gameObject.TryGetComponent(out hitBase);

            if ((1 << _animator.gameObject.layer) == LayerMask.GetMask("Character")) look = GameManager._instance.Cam.transform;
            else look = _animator.gameObject.transform;
        }

        switch (thisState)
        {
            case AnimState.Attack:
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
                if (_animator.GetInteger("State") == 0)
                {
                    if (_stateInfo.normalizedTime > 1f) hitBase.UseEnergy(thisState);
                }

                if (MoveDir != Vector3.zero)
                {
                    hitBase.transform.rotation = Quaternion.Slerp(hitBase.transform.rotation, Quaternion.LookRotation(MoveDir) * Angle, 0.5f);
                    hitBase.Rigidbody.AddRelativeForce(100 * (_animator.GetFloat("MoveSpeed") - 1) * Vector3.forward);
                }
                break;

            case AnimState.Attack:
                if (hitBase.attack.activeSelf) hitBase.Collider.CheckHit(0, hitBase.Mask);

                _animator.SetBool("Hit", false);
                break;

            case AnimState.Guard:
                hitBase.UseEnergy(thisState);
                break;

            case AnimState.Roll:
                if (_stateInfo.normalizedTime < 0.3f) time += 0.04f;
                else if (time > 0.02f) time -= 0.02f;

                hitBase.Rigidbody.AddRelativeForce(1300f * time * Vector3.forward);

                _animator.SetBool("Hit", false);
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
