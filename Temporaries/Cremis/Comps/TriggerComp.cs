using Godot;
using System;

public partial class TriggerComp : Component
{
    [Export] public TriggerMethod Method { get; set; }
    /// <summary>
    /// 等待时间，只有触发方法为Timer时才有意义
    /// </summary>
    [Export] public int WaitingTime { get; set; }
    [Export] public string[] RawEvents { get; set; }
    
    public void Execute()
    {
        EventApplier.Apply(this);
    }
}
