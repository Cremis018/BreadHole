using Godot;
using System;

public partial class DialogBox : Control
{
    #region nodes
    [ExportGroup("Nodes")]
    [Export] public RichTextLabel N_Label { get; private set; }
    #endregion

    #region fileds
    private bool _isTalking;
    #endregion

    #region events
    public event Action Confirmed;
    #endregion

    #region op
    public void Talk(string dialog)
    {
        if (_isTalking) return;
        if (dialog is null) return;
        _isTalking = true;
        if (!Visible) Show();
        dialog = Tr(dialog);
        N_Label.Text = dialog;
        N_Label.VisibleRatio = 0;
        var tween = CreateTween();
        tween.TweenProperty(N_Label, "visible_ratio", 1, GetDuration(dialog));
        tween.TweenCallback(Callable.From(() => _isTalking = false));
    }

    public void Close()
    {
        Hide();
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("confirm") 
            && !_isTalking
            && Visible)
            Confirmed?.Invoke();
    }
    #endregion

    #region handle
    private float GetDuration(string dialog)
    {
        dialog = dialog.WithoutBracketContent('[', ']');
        return dialog.Length * 0.06f;
    }
    #endregion
}
