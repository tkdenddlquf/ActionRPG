using TMPro;
using UnityEngine;

public class BossPanel : MonoBehaviour
{
    public TMP_Text thisName;
    public LerpSlider hp;

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
        hp.SetData(target.sliderAction, _value);

        if (_value == 0) Target = null;
    }
}
