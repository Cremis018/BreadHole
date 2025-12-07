using Godot;
using System;

public partial class TriggerComp : Component
{
    public TriggerMethod Method { get; set; }
    /// <summary>
    /// 等待时间，只有触发方法为Timer时才有意义
    /// </summary>
    public int WaitingTime { get; set; }
    public string[] RawEvents { get; set; }
    
    public void Execute()
    {
        EventApplier.Apply(this);
    }
}
