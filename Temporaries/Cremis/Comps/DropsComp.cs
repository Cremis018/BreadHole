using Godot;
using System;
using GodotSimpleTools;

public partial class DropsComp : Node, IComponent
{
    [Notify,Export] public int ItemId { get => GetItemId(); set => SetItemId(value); }
    
    public void PickUp()
    {
        //TODO：玩家捡起后，先查阅背包是否满了，如果满了则提示背包已满，否则将物品加入背包
    }
}
