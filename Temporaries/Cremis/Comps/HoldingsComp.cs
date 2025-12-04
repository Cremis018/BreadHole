using Godot;
using System;
using GodotSimpleTools;

public partial class HoldingsComp : Node,IComponent
{
    [Notify,Export] public int ItemId { get => GetItemId(); set => SetItemId(value); }

    public void Use()
    {
        //TODO：使用物品后执行物品对应的逻辑，然后销毁物品
    }

    public void Drop()
    {
        //TODO：丢弃物品后执行物品对应的逻辑，然后销毁物品
    }
}
