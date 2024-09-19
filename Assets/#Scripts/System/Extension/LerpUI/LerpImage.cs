using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class LerpImage
{
    public Image image;
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
        lerpValue = Mathf.Lerp(image.fillAmount, Data, speed);

        if (image.fillAmount < lerpValue) // 증가
        {
            if (image.fillAmount > Data - 0.0001f)
            {
                action.Remove(FixedUpdate);
                callback?.Invoke(Data);
            }
        }
        else
        {
            if (image.fillAmount < Data + 0.0001f)
            {
                action.Remove(FixedUpdate);
                callback?.Invoke(Data);
            }
        }

        image.fillAmount = lerpValue;
    }
}
