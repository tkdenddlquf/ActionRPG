using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class LerpSlider
{
    public Slider slider;

    private LerpUIAction action;

    public Action<float> callback;
    public float Data { get; private set; }

    public void SetData(LerpUIAction _action, float _value)
    {
        action = _action;
        Data = _value;

        action.Add(FixedUpdate);
    }

    public void FixedUpdate()
    {
        slider.value = Mathf.Lerp(slider.value, Data, 0.2f);

        if (slider.value < Data + 0.0001f)
        {
            action.Remove(FixedUpdate);
            callback?.Invoke(Data);
        }
    }
}