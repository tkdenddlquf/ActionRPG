using UnityEngine;

public class CheckHit : MonoBehaviour
{
    public HitAction hitAction;

    private HitBase hitBase;

    public delegate bool HitAction(HitBase _hitBase); // 공격 당한경우 실행
    public delegate void AttackCallback(HitBase _hitBase, int _type); // 공격에 성공한 경우 실행

    private void Start()
    {
        TryGetComponent(out hitBase);
    }

    public void Hit(HitBase _hitBase, int _type, AttackCallback _callback)
    {
        if (hitAction(_hitBase)) _callback(hitBase, _type);
    }
}
