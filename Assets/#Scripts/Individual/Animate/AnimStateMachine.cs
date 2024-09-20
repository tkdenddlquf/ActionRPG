using UnityEngine;

public class AnimStateMachine : StateMachineBehaviour
{
    public AnimState thisState;

    private float time;
    private Transform look;
    private IndividualBase individualBase;

    private Vector3 MoveDir => look == individualBase.transform ? Vector3.forward : new(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
    private Quaternion Angle => Quaternion.LookRotation(new Vector3(look.forward.x, 0, look.forward.z));

    public override void OnStateEnter(Animator _animator, AnimatorStateInfo _stateInfo, int _layerIndex)
    {
        if (individualBase == null)
        {
            _animator.gameObject.TryGetComponent(out individualBase);

            if ((1 << _animator.gameObject.layer) == LayerMask.GetMask("Character")) look = GameManager._instance.Cam.transform;
            else look = _animator.gameObject.transform;
        }

        switch (thisState)
        {
            case AnimState.Attack:
                individualBase.UseEnergy(thisState);

                individualBase.transform.rotation = Quaternion.Slerp(individualBase.transform.rotation, Angle, 0.05f);
                break;

            case AnimState.Guard:
                individualBase.AnimStateBase.guard = true;
                break;

            case AnimState.Roll:
                individualBase.AnimStateBase.roll = true;
                time = 0;

                individualBase.UseEnergy(thisState);

                if (MoveDir != Vector3.zero) individualBase.transform.rotation = Quaternion.LookRotation(MoveDir) * Angle;
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
                if (_stateInfo.normalizedTime > 1f) individualBase.UseEnergy(thisState);
                break;

            case AnimState.Walk:
                if (MoveDir != Vector3.zero)
                {
                    individualBase.transform.rotation = Quaternion.Slerp(individualBase.transform.rotation, Quaternion.LookRotation(MoveDir) * Angle, 0.5f);
                    individualBase.Rigidbody.AddRelativeForce(100 * _animator.GetFloat("MoveSpeed") * individualBase.GetSlopeVector());
                }
                break;

            case AnimState.Attack:
                if (individualBase.attack.activeSelf) individualBase.Collider.OnTriggerEnter_Callback(0, individualBase.Mask);

                _animator.SetBool("Hit", false);
                break;

            case AnimState.Guard:
                individualBase.UseEnergy(thisState);
                break;

            case AnimState.Roll:
                if (_stateInfo.normalizedTime < 0.3f) time += 0.04f;
                else if (time > 0.02f) time -= 0.02f;

                individualBase.Rigidbody.AddRelativeForce(1300f * time * Vector3.forward);

                _animator.SetBool("Hit", false);
                break;
        }
    }

    public override void OnStateExit(Animator _animator, AnimatorStateInfo _stateInfo, int _layerIndex)
    {
        switch (thisState)
        {
            case AnimState.Guard:
                if (individualBase.AnimStateBase.State != thisState) individualBase.AnimStateBase.guard = false;
                break;

            case AnimState.Roll:
                individualBase.AnimStateBase.roll = false;
                break;
        }
    }
}
