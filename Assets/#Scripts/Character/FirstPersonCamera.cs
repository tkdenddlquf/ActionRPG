using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    public float MouseSensitivity = 3f;

    private float horiRot;
    private Vector3 offset = new(0, 1.8f, -3.5f);

    private Quaternion Angle => Quaternion.Euler(20, horiRot, 0);
    private CharManager Target => GameManager._instance.character;

    void LateUpdate()
    {
        if (Target == null) return;

        if (Target.LookTarget == null)
        {
            horiRot += Input.GetAxis("Mouse X") * MouseSensitivity;

            transform.SetPositionAndRotation(Target.transform.position + Angle * offset, Angle);
        }
        else
        {
            horiRot = Quaternion.LookRotation(Target.LookTarget.transform.position - transform.position).eulerAngles.y;

            transform.position = Target.transform.position + Angle * offset;

            horiRot = Quaternion.LookRotation(Target.LookTarget.transform.position - transform.position).eulerAngles.y;

            transform.rotation = Angle;
        }
    }
}