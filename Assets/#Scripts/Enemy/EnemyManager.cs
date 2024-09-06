using UnityEngine;

public class EnemyManager : HitBase
{
    public bool isBoss = false;

    private EnemyStateBase enemyStateBase;

    private void Start()
    {
        enemyStateBase = new(this);

        Init("Character");

        AnimStateMachine[] _animBehaviours = Animator.GetBehaviours<AnimStateMachine>();

        foreach (AnimStateMachine _animBehaviour in _animBehaviours) _animBehaviour.Init(transform, this);

        commonInfo.hp[0].SetBind(HpBind);
    }

    private void Update()
    {
        enemyStateBase.UpdateState();
    }

    // 데이터 바인드
    private void HpBind(ref int _current, int _change)
    {
        if (_current > _change) Animator.SetBool("Hit", true);

        if (_current == _change) return;

        _current = _change;

        if (isBoss)
        {
            GameManager._instance.bossPanel.Target = this;
            GameManager._instance.bossPanel.SetHp((float)_current / commonInfo.hp[1].Data);
        }

        if (_current <= 0)
        {
            _current = 0;

            Die();
        }
    }

    // 상속
    protected override bool HitAction(HitBase _hitBase) // 피격
    {
        if (AnimState.roll) return false; // 구르는 중인 경우 회피

        if (LookTarget == null) LookTarget = _hitBase.gameObject;

        if (AnimState.guard) commonInfo.hp[0].Data -= _hitBase.commonInfo.atk.Data / 2;
        else commonInfo.hp[0].Data -= _hitBase.commonInfo.atk.Data;

        return true;
    }

    protected override void AttackCallback(HitBase _hitBase)
    {
        if (LookTarget == _hitBase.gameObject) // 추적중인 경우
        {
            if (_hitBase.commonInfo.hp[0].Data == 0) // 대상이 사망한 경우
            {
                LookTarget = null;
            }
        }
    }

    protected override void Die()
    {
        Animator.SetBool("Die", true);

        LookTarget = null;
    }

    protected override void LookTargetCallback()
    {
        if (LookTarget == null) // 추적이 해제된 경우
        {
            if (GameManager._instance.bossPanel.Target == this) GameManager._instance.bossPanel.Target = null;
        }
    }
}
