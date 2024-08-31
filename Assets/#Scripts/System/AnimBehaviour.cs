using UnityEngine;

public class AnimBehaviour : StateMachineBehaviour
{
    public int thisState;

    private Camera Cam => GameManager._instance.cam;
    private CharManager Character => GameManager._instance.character;

    private Vector3 MoveDir => new(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
    private Quaternion Look => Quaternion.LookRotation(new Vector3(Cam.transform.forward.x, 0, Cam.transform.forward.z));

    public override void OnStateEnter(Animator _animator, AnimatorStateInfo _stateInfo, int _layerIndex)
    {
        switch (thisState)
        {
            case 1:
                Character.SetGuid();
                break;

            case 5:
                Character.roll = true;
                break;
        }
    }

    public override void OnStateUpdate(Animator _animator, AnimatorStateInfo _stateInfo, int _layerIndex)
    {
        switch (thisState)
        {
            case 1:
                Character.transform.rotation = Quaternion.Slerp(Character.transform.rotation, Look, 0.3f * Time.deltaTime);
                break;

            case 2:
            case 4:
                Character.transform.position += Quaternion.Euler(0, Cam.transform.eulerAngles.y, 0) * MoveDir * Character.commonInfo.moveSpeed * thisState / 2 * Time.deltaTime;

                if (MoveDir != Vector3.zero) Character.transform.rotation = Quaternion.Slerp(Character.transform.rotation, Quaternion.LookRotation(MoveDir) * Look, 15 * Time.deltaTime);
                break;

            case 5:
                Character.Rigidbody.AddRelativeForce((1 - _stateInfo.normalizedTime) * 600 * Vector3.forward);
                break;
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        switch (thisState)
        {
            case 5:
                Character.roll = false;
                break;
        }
    }
}
