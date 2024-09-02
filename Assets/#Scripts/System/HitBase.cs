using UnityEngine;

public abstract class HitBase : MonoBehaviour
{
    // STATUS
    public CommonInfo commonInfo;

    public GameObject attack;

    private CheckHit hit;
    private Rigidbody rb;

    // ���
    private System.Guid guid;
    private CheckHit target;
    private GameObject lookTarget;
    private LayerMask mask;

    // ����Ȯ�� �� ����
    private int hitCount;
    private Vector3 hitBoxSize = new(0.5f, 0.75f, 0.7f);
    private readonly RaycastHit[] hits = new RaycastHit[5];

    // �ִϸ��̼� �� ����
    private Animator animator;
    private AnimStateBase animState;

    public Animator Animator
    {
        get
        {
            return animator;
        }
    }

    public AnimStateBase AnimState
    {
        get
        {
            return animState;
        }
    }

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

        TryGetComponent(out animator);
        animState = new(animator);

        foreach (var _aim in animator.GetBehaviours<AnimBehaviour>()) _aim.Init(transform, this);

        mask = LayerMask.GetMask(_layers);

        SetGuid();
    }

    protected abstract bool HitAction(GameObject _target, CommonInfo _info); // �ǰ�

    protected abstract void AttackAction(); // ���� ����

    protected abstract void Die(); // ���

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

        hitCount = Physics.BoxCastNonAlloc(attack.transform.position, hitBoxSize, transform.forward, hits, Quaternion.identity, 0, mask);

        for (int i = 0; i < hitCount; i++)
        {
            if (hits[i].collider.gameObject.TryGetComponent(out target)) target.Hit(guid, gameObject, commonInfo);
        }
    }
}
