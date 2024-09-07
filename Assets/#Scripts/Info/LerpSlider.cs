using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class LerpSlider
{
    public Slider slider;

    private float data;
    private LerpSliderAction action;

    public void SetData(LerpSliderAction _action, float _value)
    {
        action = _action;
        data = _value;

        action.Add(Update);

        Update();
    }

    public void Update()
    {
        slider.value = Mathf.Lerp(slider.value, data, 0.1f);

        if (slider.value == data) action.Remove(Update);
    }
}

public class LerpSliderAction
{
    public Action actions;

    public void Add(Action _action)
    {
        actions -= _action;
        actions += _action;
    }

    public void Remove(Action _action)
    {
        actions -= _action;
    }
}