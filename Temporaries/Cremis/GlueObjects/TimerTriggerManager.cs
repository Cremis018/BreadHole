using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class TimerTriggerManager : Node
{
    #region members
    [Export] private Timer _timer;
    #endregion

    #region fields
    private int _currentIndex;
    private bool _isStarted;
    private List<TriggerComp> _triggers = [];
    #endregion

    #region props
    public int TotalDuration => _triggers.Select(t => t.WaitingTime).Max();

    public float CurrentWaitingTime
    {
        get
        {
            var times = _triggers.Select(t => t.WaitingTime).ToArray();
            var lastTime = _currentIndex == 0 ? 0 : times[_currentIndex - 1];
            return (times[_currentIndex] - lastTime) / 1000f;
        }
    }
    #endregion

    #region life
    public override void _Ready()
    {
        _timer.Timeout += ExecuteTiggerEvent;
    }
    
    public override void _ExitTree()
    {
        _timer.Timeout -= ExecuteTiggerEvent;
    }
    #endregion

    #region op
    public void AddTrigger(TriggerComp trigger)
    {
        if (trigger is null ||
            trigger.Method != TriggerMethod.Timer) return;
        _triggers.Add(trigger);
        _triggers = _triggers.OrderBy(t => t.WaitingTime).ToList();
    }
    
    public void RemoveTrigger(TriggerComp trigger)
    {
        if (trigger is null ||
            trigger.Method != TriggerMethod.Timer) return;
        _triggers.Remove(trigger);
        _triggers = _triggers.OrderBy(t => t.WaitingTime).ToList();
    }

    public void RemoveTriggerAt(int index)
    {
        var trigger = _triggers[index];
        RemoveTrigger(trigger);
    }

    public void Start()
    {
        if (_isStarted) return;
        _isStarted = true;
        Next();
    }

    public void Next()
    {
        _timer.WaitTime = CurrentWaitingTime;
        GD.Print($"计时器：当前要等待{CurrentWaitingTime}");
        _timer.Start();
    }
    
    public void Stop()
    {
        _timer.Stop();
        _currentIndex = 0;
        _isStarted = false;
    }
    #endregion

    #region handle
    private void ExecuteTiggerEvent()
    {
        var trigger = _triggers[_currentIndex];
        trigger.Execute();
        if (_currentIndex >= _triggers.Count - 1)
        {
            Stop();
            return;
        }
        _currentIndex++;
        Next();
    }
    #endregion
}
