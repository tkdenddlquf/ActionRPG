using UnityEngine;

public class EnemyManager : HitBase
{
    public bool isBoss = false;

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

    void Start()
    {
        Init("Character");
    }

    protected override bool HitAction(GameObject _target, CommonInfo _info) // 피격
    {
        if (AnimState.roll) return false; // 구르는 중인 경우 회피

        if (LookTarget == null) LookTarget = _target;

        HP -= _info.atk;

        return true;
    }

    protected override void AttackAction()
    {

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
