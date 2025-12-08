using Godot;
using System;
using GodotSimpleTools;

public partial class HoldingsComp : Component
{
    [Notify,Export] public int ItemId { get => GetItemId(); set => SetItemId(value); }

    public void Use()
    {
        Game.Instance.Pocket.Use(ItemId);
    }

    public void Drop()
    {
        Game.Instance.Pocket.Drop(ItemId);
    }
}
