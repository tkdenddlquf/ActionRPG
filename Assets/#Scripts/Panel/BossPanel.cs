using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossPanel : MonoBehaviour
{
    public TMP_Text thisName;
    public Slider hp;

    private EnemyManager target;

    public EnemyManager Target
    {
        get
        {
            return target;
        }
        set
        {
            if (target == value) return;
            
            target = value;

            if (value == null) gameObject.SetActive(false);
            else gameObject.SetActive(true);
        }
    }

    public void SetHp(float _value)
    {
        hp.value = _value;

        if (hp.value == 0) Target = null;
    }
}
