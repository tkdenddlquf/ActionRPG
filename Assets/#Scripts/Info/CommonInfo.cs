using System;

[Serializable]
public class CommonInfo
{
    public BindData<int> Hp = new();
    public BindData<int> MaxHp = new();
    public BindData<int> Mp = new();
    public BindData<int> MaxMp = new();
    public BindData<int> Energy = new();
    public BindData<int> MaxEnergy = new();
    public BindData<int> Atk = new();
    public BindData<float> AttackSpeed = new();
    public BindData<float> MoveSpeed = new();
    public BindData<int> Money = new();
    public BindData<int> Exp = new();
}
