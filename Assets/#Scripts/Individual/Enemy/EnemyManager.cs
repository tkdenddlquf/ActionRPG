using UnityEngine;

public class EnemyManager : IndividualBase
{
    public bool isBoss = false;

    private EnemyStateBase enemyStateBase;

    private void Start()
    {
        enemyStateBase = new(this);

        Init("Character");

        commonInfo.hp[0].SetBind(HpBind);
        commonInfo.attackSpeed.SetBind(AttackSpeedBind);
    }

    private void Update()
    {
        enemyStateBase.UpdateState();
    }

    // 데이터 바인드
    private void HpBind(ref int _current, int _change)
    {
        if (_change < 0) _change = 0;
        else if (_change > commonInfo.hp[1].Data) _change = commonInfo.hp[1].Data;

        if (_current > _change) Animator.SetBool("Hit", true);

        _current = _change;

        if (isBoss && LookTarget != null)
        {
            GameManager._instance.bossPanel.Target = this;
            GameManager._instance.bossPanel.SetHp((float)_current / commonInfo.hp[1].Data);
        }

        if (_current == 0) Die();
    }

    private void AttackSpeedBind(ref float _current, float _change)
    {
        _current = _change;

        Animator.SetFloat("AttackSpeed", _current);
    }

    // 상속
    protected override void LookTargetCallback()
    {
        if (LookTarget == null) // 추적이 해제된 경우
        {
            if (GameManager._instance.bossPanel.Target == this)
            {
                GameManager._instance.bossPanel.Target = null;
                GameManager._instance.bossPanel.gameObject.SetActive(false);
            }
        }
    }
}
