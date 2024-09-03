using UnityEngine;

public class CharManager : HitBase
{
    public SpecificInfo specificInfo;

    private InputBase inputBase;

    private void Start()
    {
        inputBase = new InputCharPC(this);

        Init("Enemy");

        AnimBehaviour[] _animBehaviours = Animator.GetBehaviours<AnimBehaviour>();

        foreach (AnimBehaviour _animBehaviour in _animBehaviours) _animBehaviour.Init(GameManager._instance.cam.transform, this);

        commonInfo.Hp.SetBind(HpBind);
        commonInfo.Mp.SetBind(MpBind);
        commonInfo.Energy.SetBind(EnergyBind);
    }

    private void Update()
    {
        inputBase.CheckInput();
    }

    // 데이터 바인드
    private void HpBind(ref int _current, int _change)
    {
        if (_current > _change) Animator.SetBool("Hit", true);

        _current = _change;

        GameManager._instance.charPanel.hp.value = (float)_current / commonInfo.MaxHp.Data;

        if (_current <= 0)
        {
            _current = 0;

            Die();
        }
    }

    private void MpBind(ref int _current, int _change)
    {
        _current = _change;

        GameManager._instance.charPanel.mp.value = (float)_current / commonInfo.MaxMp.Data;
    }

    private void EnergyBind(ref int _current, int _change)
    {
        _current = _change;

        GameManager._instance.charPanel.energy.value = (float)_current / commonInfo.MaxEnergy.Data;
    }

    // 상속
    protected override bool HitAction(HitBase _hitBase) // 피격
    {
        if (AnimState.roll) return false; // 구르는 중인 경우 회피

        if (AnimState.guard) commonInfo.Hp.Data -= _hitBase.commonInfo.Atk.Data / 2;
        else commonInfo.Hp.Data -= _hitBase.commonInfo.Atk.Data;

        return true;
    }

    protected override void AttackCallback(HitBase _hitBase) // 공격 성공
    {
        if (LookTarget == _hitBase.gameObject) // 추적중인 경우
        {
            if (_hitBase.commonInfo.Hp.Data == 0) // 대상이 사망한 경우
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
