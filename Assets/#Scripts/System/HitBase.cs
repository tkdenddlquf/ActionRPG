using UnityEngine;

public abstract class HitBase : MonoBehaviour
{
    // STATUS
    public CommonInfo commonInfo;

    public GameObject attack;
    public bool roll;

    private CheckHit hit;
    private Rigidbody rb;

    private System.Guid guid;
    private CheckHit target;
    private GameObject lookTarget;
    private LayerMask mask;

    // 공격 범위 확인
    private int hitCount;
    private Vector3 hitBoxCenter = new(0, 1, 0.7f);
    private Vector3 hitBoxSize = new(0.5f, 0.75f, 0.7f);
    private readonly RaycastHit[] hits = new RaycastHit[5];

    public Rigidbody Rigidbody
    {
        get
        {
            return rb;
        }
    }

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
        TryGetComponent(out rb);
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
        if (LookTarget != null) LookTarget = null;
        else if (Physics.Raycast(GameManager._instance.cam.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 100, mask)) LookTarget = hit.transform.gameObject;
    }

    public void CheckHit()
    {
        if (!attack.activeSelf) return;

        hitCount = Physics.BoxCastNonAlloc(transform.position + hitBoxCenter, hitBoxSize, transform.forward, hits, Quaternion.identity, 0.5f, mask);

        for (int i = 0; i < hitCount; i++)
        {
            if (hits[i].collider.gameObject.TryGetComponent(out target)) target.Hit(guid, gameObject, commonInfo);
        }
    }
}
