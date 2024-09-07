using UnityEngine;

public class CharManager : HitBase
{
    public SpecificInfo specificInfo;

    private InputBase inputBase;

    private void Start()
    {
        inputBase = new InputCharPC(this);

        Init("Enemy");

        AnimStateMachine[] _animBehaviours = Animator.GetBehaviours<AnimStateMachine>();

        foreach (AnimStateMachine _animBehaviour in _animBehaviours) _animBehaviour.Init(GameManager._instance.cam.transform, this);

        commonInfo.hp[0].SetBind(HpBind);
        commonInfo.mp[0].SetBind(MpBind);
        commonInfo.energy[0].SetBind(EnergyBind);
        commonInfo.attackSpeed.SetBind(AttackSpeedBind);
    }

    private void Update()
    {
        inputBase.CheckInput();
    }

    // 데이터 바인드
    private void HpBind(ref int _current, int _change)
    {
        if (_change < 0) _change = 0;
        else if (_change > commonInfo.hp[1].Data) _change = commonInfo.hp[1].Data;

        if (_current > _change) Animator.SetBool("Hit", true);

        _current = _change;

        GameManager._instance.charPanel.hp.value = (float)_current / commonInfo.hp[1].Data;

        if (_current == 0) Die();
    }

    private void MpBind(ref int _current, int _change)
    {
        if (_change < 0) _change = 0;
        else if (_change > commonInfo.mp[1].Data) _change = commonInfo.mp[1].Data;

        _current = _change;

        GameManager._instance.charPanel.mp.value = (float)_current / commonInfo.mp[1].Data;
    }

    private void EnergyBind(ref int _current, int _change)
    {
        if (_change < 0) _change = 0;
        else if (_change > commonInfo.energy[1].Data) _change = commonInfo.energy[1].Data;

        _current = _change;

        GameManager._instance.charPanel.energy.value = (float)_current / commonInfo.energy[1].Data;
    }

    private void AttackSpeedBind(ref float _current, float _change)
    {
        _current = _change;

        Animator.SetFloat("AttackSpeed", _current);
    }

    // 상속
    public override bool UseEnergy(AnimState _state, bool _check = false)
    {
        switch (_state)
        {
            case AnimState.Idle:
                if (_check) return true;
                else commonInfo.energy[0].Data += 1;
                break;

            case AnimState.Attack:
                if (_check) return commonInfo.energy[0].Data - 100 >= 0;
                else commonInfo.energy[0].Data -= 100;
                break;

            case AnimState.Guard:
                if (_check) return commonInfo.energy[0].Data - 1 >= 0;
                else commonInfo.energy[0].Data -= 1;
                break;

            case AnimState.Roll:
                if (_check) return commonInfo.energy[0].Data - 100 >= 0;
                else commonInfo.energy[0].Data -= 100;
                break;
        }

        return true;
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
