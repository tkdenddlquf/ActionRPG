using UnityEngine;

public class CharManager : HitBase
{
    public SpecificInfo specificInfo;

    private InputBase inputBase;

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

            GameManager._instance.charPanel.hp.@value = (float)value / commonInfo.maxHp;

            if (commonInfo.hp <= 0)
            {
                commonInfo.hp = 0;

                Die();
            }
        }
    }

    private void Start()
    {
        inputBase = new InputCharPC(this);

        Init("Enemy");

        AnimBehaviour[] _animBehaviours = Animator.GetBehaviours<AnimBehaviour>();

        foreach (AnimBehaviour _animBehaviour in _animBehaviours) _animBehaviour.Init(GameManager._instance.cam.transform, this);
    }

    private void Update()
    {
        inputBase.CheckInput();
    }

    protected override bool HitAction(HitBase _hitBase) // 피격
    {
        if (AnimState.roll) return false; // 구르는 중인 경우 회피

        HP -= _hitBase.commonInfo.atk;

        return true;
    }

    protected override void AttackCallback(HitBase _hitBase) // 공격 성공
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
