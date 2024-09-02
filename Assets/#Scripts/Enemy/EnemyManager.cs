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

    protected override bool HitAction(GameObject _target, CommonInfo _info) // �ǰ�
    {
        if (AnimState.roll) return false; // ������ ���� ��� ȸ��

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
        if (LookTarget == null) // ������ ������ ���
        {
            if (GameManager._instance.bossPanel.Target == this) GameManager._instance.bossPanel.Target = null;
        }
    }
}
