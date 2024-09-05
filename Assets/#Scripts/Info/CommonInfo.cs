using System;

[Serializable]
public class CommonInfo
{
    public BindData<int> hp = new();
    public BindData<int> maxHp = new();
    public BindData<int> mp = new();
    public BindData<int> maxMp = new();
    public BindData<int> energy = new();
    public BindData<int> maxEnergy = new();
    public BindData<int> atk = new();
    public BindData<float> attackSpeed = new();
    public BindData<float> moveSpeed = new();
    public BindData<int> money = new();
    public BindData<int> exp = new();
}
