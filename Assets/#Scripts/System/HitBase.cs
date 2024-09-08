using UnityEngine;
using System.Collections.Generic;

public abstract class HitBase : MonoBehaviour
{
    // STATUS
    public CommonInfo commonInfo;
    public LerpSliderAction sliderAction = new();

    public GameObject attack;

    private CheckHit hit;
    private Rigidbody rb;

    // 대상
    private LayerMask mask;
    private CheckHit target;
    private GameObject lookTarget;
    private readonly Dictionary<GameObject, float> attackDelays = new();

    // 공격확인 및 범위
    private int hitCount;
    private Vector3 hitBoxSize = new(0.5f, 0.75f, 0.7f);
    private readonly RaycastHit[] hits = new RaycastHit[5];

    // 애니메이션 및 상태
    private Animator animator;
    private AnimStateBase animStateBase;

    public Animator Animator => animator;
    public AnimStateBase AnimStateBase => animStateBase;
    public Rigidbody Rigidbody => rb;

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

        TryGetComponent(out animator);
        animStateBase = new(animator);

        mask = LayerMask.GetMask(_layers);
    }

    private void FixedUpdate()
    {
        sliderAction.actions?.Invoke();
    }

    // 상속
    public virtual bool UseEnergy(AnimState _state, bool _check = false)
    {
        return true;
    }

    protected virtual bool HitAction(HitBase _hitBase) // 피격
    {
        if (AnimStateBase.roll) return false; // 구르는 중인 경우 회피

        if (LookTarget == null) LookTarget = _hitBase.gameObject;

        if (AnimStateBase.guard) commonInfo.hp[0].Data -= _hitBase.commonInfo.atk.Data / 2;
        else commonInfo.hp[0].Data -= _hitBase.commonInfo.atk.Data;

        return true;
    }

    protected virtual void AttackCallback(HitBase _hitBase) // 공격 성공
    {
        attackDelays[_hitBase.gameObject] = Time.time;

        if (_hitBase.commonInfo.hp[0].Data == 0) // 대상이 사망한 경우
        {
            attackDelays.Remove(_hitBase.gameObject);

            if (LookTarget == _hitBase.gameObject) LookTarget = null; // 추적중인 경우
        }
        else if (LookTarget == null) LookTarget = _hitBase.gameObject;
    }

    protected virtual void Die() // 사망
    {
        Animator.SetBool("Die", true);

        LookTarget = null;
    }

    protected virtual void LookTargetCallback()
    {

    }

    // 기본
    public void SetTarget() // 대상 변경
    {
        if (LookTarget != null) LookTarget = null;
        else if (Physics.Raycast(GameManager._instance.cam.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, 100, mask)) LookTarget = hit.transform.gameObject;
    }

    public void CheckHit(float _delay) // 공격 확인
    {
        if (!attack.activeSelf) return;

        hitCount = Physics.BoxCastNonAlloc(attack.transform.position, hitBoxSize, transform.forward, hits, Quaternion.identity, 0, mask);

        for (int i = 0; i < hitCount; i++)
        {
            if (attackDelays.ContainsKey(hits[i].collider.gameObject))
            {
                if (attackDelays[hits[i].collider.gameObject] > Time.time - _delay) continue;
            }

            if (hits[i].collider.gameObject.TryGetComponent(out target)) target.Hit(this, AttackCallback);
        }
    }

    protected bool UseEnergy(int _value, bool _check)
    {
        if (_check) return commonInfo.energy[0].Data - _value >= 0;
        else commonInfo.energy[0].Data -= _value;

        return true;
    }
}
