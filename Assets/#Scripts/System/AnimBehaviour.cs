using UnityEngine;

public class AnimBehaviour : StateMachineBehaviour
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
            case AnimState.Hit:
                _animator.SetBool("Hit", false);
                break;

            case AnimState.Attack:
                hitBase.SetGuid();
                hitBase.commonInfo.Energy.Data -= 10;
                break;

            case AnimState.Guard:
                hitBase.AnimState.guard = true;
                break;

            case AnimState.Roll:
                hitBase.AnimState.roll = true;
                time = 0;

                if (MoveDir != Vector3.zero) hitBase.transform.rotation = Quaternion.LookRotation(MoveDir) * Angle;
                break;
        }
    }

    public override void OnStateUpdate(Animator _animator, AnimatorStateInfo _stateInfo, int _layerIndex)
    {
        switch (thisState)
        {
            case AnimState.Attack:
                _animator.SetBool("Hit", false);

                hitBase.transform.rotation = Quaternion.Slerp(hitBase.transform.rotation, Angle, 0.3f * Time.deltaTime);
                hitBase.CheckHit();
                break;

            case AnimState.Walk:
                if (MoveDir != Vector3.zero)
                {
                    hitBase.transform.position += Quaternion.Euler(0, look.eulerAngles.y, 0) * MoveDir * hitBase.commonInfo.MoveSpeed.Data * Time.deltaTime;
                    hitBase.transform.rotation = Quaternion.Slerp(hitBase.transform.rotation, Quaternion.LookRotation(MoveDir) * Angle, 15 * Time.deltaTime);
                }
                break;

            case AnimState.Guard:
                _animator.SetBool("Hit", false);
                break;

            case AnimState.Run:
                if (MoveDir != Vector3.zero)
                {
                    hitBase.transform.position += Quaternion.Euler(0, look.eulerAngles.y, 0) * MoveDir * hitBase.commonInfo.MoveSpeed.Data * 2 * Time.deltaTime;
                    hitBase.transform.rotation = Quaternion.Slerp(hitBase.transform.rotation, Quaternion.LookRotation(MoveDir) * Angle, 15 * Time.deltaTime);
                }
                break;

            case AnimState.Roll:
                if (_stateInfo.normalizedTime < 0.5f) time += Time.deltaTime;
                else time -= Time.deltaTime;

                if (MoveDir != Vector3.zero) hitBase.transform.position += Quaternion.Euler(0, look.eulerAngles.y, 0) * MoveDir * time * 0.7f;
                else hitBase.transform.position += 0.7f * time * hitBase.transform.forward;
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
