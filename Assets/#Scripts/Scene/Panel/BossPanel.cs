using TMPro;
using UnityEngine;

public class BossPanel : MonoBehaviour
{
    public TMP_Text thisName;
    public LerpSlider hp;

    public EnemyManager Target { get; set; }

    public void Start()
    {
        hp.callback = Callback;
    }

    public void SetHp(float _value)
    {
        hp.SetData(Target.LerpAction, _value);

        if (_value == 0) Target = null;
        else gameObject.SetActive(true);
    }

    public void Callback(float _value)
    {
        if (_value == 0) gameObject.SetActive(false);
    }
}
