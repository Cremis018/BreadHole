using Godot;
using System;

public partial class TestRoom : Node
{
    [Export] private TimerTriggerManager _timerTriggerManager;
    
    public override void _Ready()
    {
        var children = GetChildren();
        foreach (var child in children)
        {
            if (child is TriggerComp trigger)
            {
                _timerTriggerManager.AddTrigger(trigger);
            }
        }
    }

    public override void _Input(InputEvent @event)
    {
        // if (@event is InputEventMouseButton {ButtonMask:MouseButtonMask.Left})
        //     _timerTriggerManager.Start();
    }
}
