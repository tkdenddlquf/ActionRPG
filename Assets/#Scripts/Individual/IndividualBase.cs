using UnityEngine;
using System.Collections.Generic;

public abstract class IndividualBase : MonoBehaviour
{
    // STATUS
    public CommonInfo commonInfo;
    public GameObject attack;

    private HitableObject hitThis;
    private GameObject lookTarget;

    // 공격
    private HitableObject hitTarget;
    private readonly Dictionary<GameObject, Dictionary<int, AttackInfoValue>> attackable = new();

    // 프로퍼티
    public BoxCollider_Custum Collider { get; private set; }
    public LayerMask Mask { get; private set; }
    public Rigidbody Rigidbody { get; private set; }
    public Animator Animator { get; private set; }
    public AnimStateBase AnimStateBase { get; private set; }
    public LerpUIAction LerpAction { get; private set; } = new();
    public GameObject LookTarget
    {
        get => lookTarget;
        set
        {
            lookTarget = value;

            LookTargetCallback();
        }
    }

    protected void Init(params string[] _layers)
    {
        Collider = GetComponent<BoxCollider_Custum>();
        Collider.callback = HitBoxCallback;

        Rigidbody = GetComponent<Rigidbody>();

        hitThis = GetComponent<HitableObject>();
        hitThis.check = HitAction;

        Animator = GetComponent<Animator>();
        AnimStateBase = new(Animator);

        Mask = LayerMask.GetMask(_layers);
    }

    private void FixedUpdate()
    {
        LerpAction.actions?.Invoke();
    }

    // 상속
    public virtual bool UseEnergy(AnimState _state, bool _check = false)
    {
        return true;
    }

    protected virtual bool HitAction(IndividualBase _hitBase) // 피격
    {
        if (AnimStateBase.roll) return false; // 구르는 중인 경우 회피

        if (LookTarget == null) LookTarget = _hitBase.gameObject;

        if (AnimStateBase.guard) commonInfo.hp[0].Data -= _hitBase.commonInfo.atk.Data / 2;
        else commonInfo.hp[0].Data -= _hitBase.commonInfo.atk.Data;

        return true;
    }

    protected virtual void AttackCallback(IndividualBase _hitBase, int _type) // 공격 성공
    {
        if (_hitBase.commonInfo.hp[0].Data == 0) // 대상이 사망한 경우
        {
            attackable.Remove(_hitBase.gameObject);

            if (LookTarget == _hitBase.gameObject) LookTarget = null; // 추적중인 경우
        }
        else
        {
            attackable[_hitBase.gameObject][_type].Attack();

            if (LookTarget == null) LookTarget = _hitBase.gameObject;
        }
    }

    protected virtual void Die() // 사망
    {
        Animator.SetBool("Die", true);

        LookTarget = null;
    }

    protected virtual void LookTargetCallback()
    {

    }

    protected virtual void HitBoxCallback(BoxCollider_CustomInfo _info)
    {
        if (!attackable.ContainsKey(_info.target)) attackable[_info.target] = new();
        if (!attackable[_info.target].ContainsKey(_info.index)) attackable[_info.target][_info.index] = new();

        if (!attackable[_info.target][_info.index].Attackable(GetDelay(_info.index), Collider.maxCount[_info.index])) return;

        if (_info.target.TryGetComponent(out hitTarget)) hitTarget.Hit(this, _info.index, AttackCallback);
    }

    protected virtual float GetDelay(int _type)
    {
        return 1f;
    }

    // 기본
    protected bool UseEnergy(int _value, bool _check)
    {
        if (_check) return commonInfo.energy[0].Data - _value >= 0;
        else commonInfo.energy[0].Data -= _value;

        return true;
    }
}
