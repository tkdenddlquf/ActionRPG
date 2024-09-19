using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class LerpImage
{
    public Image image;

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
        image.fillAmount = Mathf.Lerp(image.fillAmount, Data, 0.2f);

        if (image.fillAmount == Data + 0.0001f)
        {
            action.Remove(FixedUpdate);
            callback?.Invoke(Data);
        }
    }
}
