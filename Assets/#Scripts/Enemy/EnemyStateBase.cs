using UnityEngine;

public class EnemyStateBase
{
    private float dist;

    private readonly Vector3 initPos;
    private readonly EnemyManager enemy;

    public EnemyStateBase(EnemyManager _enemy)
    {
        enemy = _enemy;
        initPos = enemy.transform.position;
    }

    public void UpdateState()
    {
        if (enemy.LookTarget == null) // 추적할 대상이 없는 경우 원래 위치로 이동
        {
            enemy.AnimState.State = AnimState.Idle;

            if (Vector3.Distance(enemy.transform.position, initPos) < 1) enemy.Animator.SetFloat("MoveSpeed", 0, 1f, 0.1f);
            else
            {
                enemy.transform.LookAt(initPos); // 기존 위치로 방향 설정

                enemy.Animator.SetFloat("MoveSpeed", 0.5f, 1f, 0.1f);
            }

            return;
        }

        dist = Vector3.Distance(enemy.transform.position, enemy.LookTarget.transform.position);

        if (dist > 10) // 너무 멀어진 경우
        {
            enemy.transform.LookAt(initPos); // 기존 위치로 방향 설정

            enemy.AnimState.State = AnimState.Idle;
            enemy.Animator.SetFloat("MoveSpeed", 0.5f, 1f, 0.1f);

            enemy.SetTarget(); // 추적 해제

            return;
        }

        CheckState();
    }

    private void CheckState()
    {
        enemy.transform.LookAt(enemy.LookTarget.transform);

        if (dist < 1.8f) // 대상이 가까운 경우
        {
            enemy.AnimState.StateRecord = AnimState.Attack;
        }
        else
        {
            enemy.AnimState.StateRecord = AnimState.Idle;
            enemy.Animator.SetFloat("MoveSpeed", 0.5f, 1f, 0.1f);
        }

        enemy.AnimState.State = enemy.AnimState.StateRecord;
    }
}
