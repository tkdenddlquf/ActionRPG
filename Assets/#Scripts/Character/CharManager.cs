using UnityEngine;

public class CharManager : HitBase
{
    public SpecificInfo specificInfo;

    private InputBase inputBase;

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

            GameManager._instance.charPanel.hp.@value = (float)value / commonInfo.maxHp;

            if (commonInfo.hp <= 0)
            {
                commonInfo.hp = 0;

                Die();
            }
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

        TryGetComponent(out animator);

        foreach (var _aim in animator.GetBehaviours<AnimBehaviour>()) _aim.Init(GameManager._instance.cam.transform, this);

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
        Animator.SetBool("Die", true);
    }

    protected override void LookTargetCallback()
    {
        if (LookTarget == null)
        {
            GameManager._instance.cineOrbit.HorizontalAxis.Value = GameManager._instance.cam.transform.eulerAngles.y;
            GameManager._instance.cineOrbit.VerticalAxis.Value = GameManager._instance.cam.transform.eulerAngles.x + 17.5f;

            GameManager._instance.cineCam.Priority = -1;
        }
        else
        {
            GameManager._instance.cineCam.LookAt = LookTarget.transform;
            GameManager._instance.cineCam.Priority = 1;
        }
    }
}
