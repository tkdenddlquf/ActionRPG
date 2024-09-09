using UnityEngine;

public class AttackInfoValue
{
    private int count;
    private float time;

    public void Attack()
    {
        time = Time.time;
        count++;
    }

    public bool Attackable(float _delay, int _max)
    {
        if (time < Time.time - _delay) return true;
        else if (count < _max) return true;

        return false;
    }
}
