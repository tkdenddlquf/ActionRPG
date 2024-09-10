using System;

[Serializable]
public class CommonInfo
{
    public BindData<int>[] hp = new BindData<int>[2];
    public BindData<int>[] mp = new BindData<int>[2];
    public BindData<int>[] energy = new BindData<int>[2];
    public BindData<int> atk = new();
    public BindData<float> attackSpeed = new();
    public BindData<float> moveSpeed = new();
    public BindData<int> money = new();
    public BindData<int> exp = new();
}
