using UnityEngine;

public class HitableObject : MonoBehaviour
{
    public Check check;

    private IndividualBase hitBase;

    public delegate bool Check(IndividualBase _hitBase); // 공격당한 경우 실행
    public delegate void Callback(IndividualBase _hitBase, int _type); // 공격에 성공한 경우 실행

    private void Start()
    {
        TryGetComponent(out hitBase);
    }

    public void Hit(IndividualBase _hitBase, int _type, Callback _callback)
    {
        if (check(_hitBase)) _callback(hitBase, _type);
    }
}
