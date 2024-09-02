using UnityEngine;

public class EnemyManager : HitBase
{
    public bool isBoss = false;

    private EnemyStateBase enemyStateBase;

    public int HP
    {
        get
        {
            return commonInfo.hp;
        }
        set
        {
            commonInfo.hp = value;
            Animator.SetBool("Hit", true);

            if (isBoss)
            {
                GameManager._instance.bossPanel.Target = this;
                GameManager._instance.bossPanel.SetHp((float)value / commonInfo.maxHp);
            }

            if (commonInfo.hp <= 0)
            {
                commonInfo.hp = 0;

                Die();
            }
        }
    }

    private void Start()
    {
        enemyStateBase = new(this);

        Init("Character");

        AnimBehaviour[] _animBehaviours = Animator.GetBehaviours<AnimBehaviour>();

        foreach (AnimBehaviour _animBehaviour in _animBehaviours) _animBehaviour.Init(transform, this);
    }

    private void Update()
    {
        enemyStateBase.UpdateState();
    }

    protected override bool HitAction(HitBase _hitBase) // 피격
    {
        if (AnimState.roll) return false; // 구르는 중인 경우 회피

        if (LookTarget == null) LookTarget = _hitBase.gameObject;

        HP -= _hitBase.commonInfo.atk;

        return true;
    }

    protected override void AttackCallback(HitBase _hitBase)
    {
        if (LookTarget == _hitBase.gameObject) // 추적중인 경우
        {
            if (_hitBase.commonInfo.hp == 0) // 대상이 사망한 경우
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
