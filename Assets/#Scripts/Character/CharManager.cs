using UnityEngine;

public class CharManager : HitBase
{
    public SpecificInfo specificInfo;

    private InputBase inputBase;

    private Rigidbody rb;
    private Animator animator;

    public int HP
    {
        get
        {
            return commonInfo.hp;
        }
        set
        {
            commonInfo.hp = value;

            GameManager._instance.charPanel.hp.@value = (float)value / commonInfoMax.hp;

            if (commonInfo.hp < 0)
            {
                commonInfo.hp = 0;

                Die();
            }
        }
    }

    public Rigidbody Rigidbody
    {
        get
        {
            return rb;
        }
    }

    public Animator Animator
    {
        get
        {
            return animator;
        }
    }

    private void Start()
    {
        inputBase = new InputCharPC(this);

        TryGetComponent(out rb);
        TryGetComponent(out animator);

        Init("Enemy");
    }

    private void Update()
    {
        inputBase.CheckInput();
    }

    protected override bool HitAction(GameObject _target, CommonInfo _info) // 피격
    {
        if (roll) return false; // 구르는 중인 경우 회피

        HP -= _info.atk;

        return true;
    }

    protected override void AttackAction() // 공격 성공
    {

    }

    protected override void Die()
    {

    }

    protected override void LookTargetCallback()
    {
        if (LookTarget == null) GameManager._instance.cineCam.Priority = -1;
        else
        {
            GameManager._instance.cineCam.LookAt = LookTarget.transform;
            GameManager._instance.cineCam.Priority = 1;
        }
    }
}
