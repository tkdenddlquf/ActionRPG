using UnityEngine;

public abstract class HitBase : MonoBehaviour
{
    // STATUS
    public CommonInfo commonInfo;
    public CommonInfoMax commonInfoMax;

    public GameObject attack;
    public bool roll;

    private CheckHit hit;

    private System.Guid guid;
    private CheckHit target;
    private GameObject lookTarget;
    private LayerMask mask;

    public GameObject LookTarget
    {
        get
        {
            return lookTarget;
        }
        protected set
        {
            lookTarget = value;

            LookTargetCallback();
        }
    }

    protected void Init(params string[] _layers)
    {
        TryGetComponent(out hit);
        hit.hitAction = HitAction;
        hit.callback = AttackAction;

        mask = LayerMask.GetMask(_layers);

        SetGuid();
    }

    protected abstract bool HitAction(GameObject _target, CommonInfo _info); // 피격

    protected abstract void AttackAction(); // 공격 성공

    protected abstract void Die(); // 사망

    protected virtual void LookTargetCallback()
    {

    }

    public void SetGuid()
    {
        guid = System.Guid.NewGuid();
    }

    public void SetTarget()
    {
        if (lookTarget != null) lookTarget = null;
        else if (Physics.Raycast(GameManager._instance.cam.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 100, mask)) lookTarget = hit.transform.gameObject;
    }

    private void OnTriggerEnter(Collider _col)
    {
        if (!attack.activeSelf) return;

        if ((mask & (1 << _col.gameObject.layer)) != 0)
        {
            if (_col.gameObject.TryGetComponent(out target)) target.Hit(guid, gameObject, commonInfo);
        }
    }
}
