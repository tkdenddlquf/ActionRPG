using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class LerpSlider
{
    public Slider slider;
    public float speed = 0.2f;

    private float lerpValue;
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
        lerpValue = Mathf.Lerp(slider.value, Data, speed);

        if (slider.value < lerpValue) // 증가
        {
            if (slider.value > Data - 0.0001f)
            {
                action.Remove(FixedUpdate);
                callback?.Invoke(Data);
            }
        }
        else
        {
            if (slider.value < Data + 0.0001f)
            {
                action.Remove(FixedUpdate);
                callback?.Invoke(Data);
            }
        }

        slider.value = lerpValue;
    }
}