using UnityEngine;

public class CheckHit : MonoBehaviour
{
    public HitAction hitAction;
    public AttackAction callback;

    private System.Guid targetGuid = new();

    public delegate bool HitAction(GameObject _target, CommonInfo _info); // 공격 당한경우 실행
    public delegate void AttackAction(); // 공격에 성공한 경우 실행

    public void Hit(System.Guid _guid, GameObject _target, CommonInfo _info)
    {
        if (targetGuid == _guid) return;

        targetGuid = _guid;

        if (hitAction(_target, _info)) callback();
    }
}
