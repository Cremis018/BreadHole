using Godot;
using System;
using GodotSimpleTools;

public partial class LockComp : Node,IComponent
{
    [Notify(true),Export] public bool IsLocked { get => GetIsLocked(); set => SetIsLocked(value); }
    [Notify,Export] public int NeedsItemId { get => GetNeedsItemId(); set => SetNeedsItemId(value); }

    public void AutoLock()
    {
        IsLocked = true;
    }
    
    public void AutoUnlock()
    {
        IsLocked = false;
    }

    public void Unlock()
    {
        //TODO:通过需求物品ID，删除玩家身上对应的物品，并解锁
    }
}
