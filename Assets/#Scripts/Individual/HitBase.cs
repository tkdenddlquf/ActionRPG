using UnityEngine;
using System.Collections.Generic;

public abstract class HitBase : MonoBehaviour
{
    // STATUS
    public CommonInfo commonInfo;
    public GameObject attack;

    private CheckHit hit;
    private GameObject lookTarget;

    // 공격
    private int hitsLength;
    private CheckHit hitTarget;
    private readonly Collider[] hitColliders = new Collider[5];
    private readonly Dictionary<GameObject, Dictionary<int, AttackInfoValue>> attackable = new();

    // 프로퍼티
    public HitBox HitBox { get; private set; }
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
        HitBox = GetComponent<HitBox>();
        Rigidbody = GetComponent<Rigidbody>();

        hit = GetComponent<CheckHit>();
        hit.hitAction = HitAction;

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

    protected virtual bool HitAction(HitBase _hitBase) // 피격
    {
        if (AnimStateBase.roll) return false; // 구르는 중인 경우 회피

        if (LookTarget == null) LookTarget = _hitBase.gameObject;

        if (AnimStateBase.guard) commonInfo.hp[0].Data -= _hitBase.commonInfo.atk.Data / 2;
        else commonInfo.hp[0].Data -= _hitBase.commonInfo.atk.Data;

        return true;
    }

    protected virtual void AttackCallback(HitBase _hitBase, int _type) // 공격 성공
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

    // 기본
    public void CheckHitBox(int _attackType, float _delay) // 공격 확인
    {
        hitsLength = Physics.OverlapBoxNonAlloc(transform.position + transform.TransformDirection(HitBox.pos[_attackType]), HitBox.scale[_attackType], hitColliders, Quaternion.identity, Mask);

        for (int i = 0; i < hitsLength; i++)
        {
            if (!attackable.ContainsKey(hitColliders[i].gameObject)) attackable[hitColliders[i].gameObject] = new();
            if (!attackable[hitColliders[i].gameObject].ContainsKey(_attackType)) attackable[hitColliders[i].gameObject][_attackType] = new();

            if (!attackable[hitColliders[i].gameObject][_attackType].Attackable(_delay, HitBox.maxHitCount[_attackType])) continue;

            if (hitColliders[i].gameObject.TryGetComponent(out hitTarget)) hitTarget.Hit(this, _attackType, AttackCallback);
        }
    }

    protected bool UseEnergy(int _value, bool _check)
    {
        if (_check) return commonInfo.energy[0].Data - _value >= 0;
        else commonInfo.energy[0].Data -= _value;

        return true;
    }
}
