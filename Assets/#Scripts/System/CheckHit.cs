using UnityEngine;

public class CheckHit : MonoBehaviour
{
    public HitAction hitAction;

    private System.Guid targetGuid = new();
    private HitBase hitBase;

    public delegate bool HitAction(HitBase _hitBase); // 공격 당한경우 실행
    public delegate void AttackCallback(HitBase _hitBase); // 공격에 성공한 경우 실행

    private void Start()
    {
        TryGetComponent(out hitBase);
    }

    public void Hit(System.Guid _guid, HitBase _hitBase, AttackCallback _callback)
    {
        if (targetGuid == _guid) return;

        targetGuid = _guid;

        if (hitAction(_hitBase)) _callback(hitBase);
    }
}
