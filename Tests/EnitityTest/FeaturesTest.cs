using Godot;
using System;

public partial class FeaturesTest : Node
{
    [Export] private TimerTriggerManager _timerTriggerManager;
    
    public override void _Ready()
    {
        var t1 = Component.Create<TriggerComp>();
        t1.Method = TriggerMethod.Timer;
        t1.WaitingTime = 1000;
        t1.RawEvents = ["print{Hello World}","print{WWWWWWWW}"];
        var t2 = Component.Create<TriggerComp>();
        t2.Method = TriggerMethod.Timer;
        t2.WaitingTime = 2000;
        t2.RawEvents = ["print{1,2}"];
        _timerTriggerManager.AddTrigger(t1);
        _timerTriggerManager.AddTrigger(t2);
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton { ButtonIndex: MouseButton.Left })
        {
            _timerTriggerManager.Start();
        }
    }
}
