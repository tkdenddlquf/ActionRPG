using UnityEngine;

public class CharManager : HitBase
{
    public SpecificInfo specificInfo;

    private InputBase inputBase;

    private void Start()
    {
        inputBase = new InputCharPC(this);

        Init("Enemy");

        commonInfo.hp[0].SetBind(HpBind);
        commonInfo.mp[0].SetBind(MpBind);
        commonInfo.energy[0].SetBind(EnergyBind);
        commonInfo.attackSpeed.SetBind(AttackSpeedBind);

        specificInfo.potion[0].SetBind(PotionBind);
    }

    private void Update()
    {
        inputBase.CheckInput();
    }

    // 일반 데이터 바인드
    private void HpBind(ref int _current, int _change)
    {
        if (_change < 0) _change = 0;
        else if (_change > commonInfo.hp[1].Data) _change = commonInfo.hp[1].Data;

        if (_current > _change) Animator.SetBool("Hit", true);

        _current = _change;

        GameManager._instance.charPanel.hp.SetData(LerpAction, (float)_current / commonInfo.hp[1].Data);

        if (_current == 0) Die();
    }

    private void MpBind(ref int _current, int _change)
    {
        if (_change < 0) _change = 0;
        else if (_change > commonInfo.mp[1].Data) _change = commonInfo.mp[1].Data;

        _current = _change;

        GameManager._instance.charPanel.mp.SetData(LerpAction, (float)_current / commonInfo.mp[1].Data);
    }

    private void EnergyBind(ref int _current, int _change)
    {
        if (_change < 0) _change = 0;
        else if (_change > commonInfo.energy[1].Data) _change = commonInfo.energy[1].Data;

        _current = _change;

        GameManager._instance.charPanel.energy.SetData(LerpAction, (float)_current / commonInfo.energy[1].Data);
    }

    private void AttackSpeedBind(ref float _current, float _change)
    {
        _current = _change;

        Animator.SetFloat("AttackSpeed", _current);
    }

    // 특별 데이터 바인드
    private void PotionBind(ref int _current, int _change)
    {
        if (_change < 0) _change = 0;
        else if (_change > specificInfo.potion[1].Data) _change = specificInfo.potion[1].Data;

        _current = _change;

        GameManager._instance.charPanel.potion.SetData(LerpAction, (float)_current / specificInfo.potion[1].Data);
    }

    // 상속
    public override bool UseEnergy(AnimState _state, bool _check = false)
    {
        return _state switch
        {
            AnimState.Idle => UseEnergy(-1, _check),
            AnimState.Attack => UseEnergy(100, _check),
            AnimState.Guard => UseEnergy(1, _check),
            AnimState.Roll => UseEnergy(100, _check),
            _ => true,
        };
    }

    protected override void LookTargetCallback()
    {
        if (LookTarget == null)
        {
            GameManager._instance.cineOrbit.HorizontalAxis.Value = GameManager._instance.Cam.transform.eulerAngles.y;
            GameManager._instance.cineOrbit.VerticalAxis.Value = GameManager._instance.Cam.transform.eulerAngles.x + 17.5f;

            GameManager._instance.cineCam.Priority = -1;
        }
        else
        {
            GameManager._instance.cineCam.LookAt = LookTarget.transform;
            GameManager._instance.cineCam.Priority = 1;
        }
    }
}
