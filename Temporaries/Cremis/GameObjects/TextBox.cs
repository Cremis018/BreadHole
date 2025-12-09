using Godot;
using System;

public partial class TextBox : Control
{
    #region nodes
    [ExportGroup("Nodes")]
    [Export] public RichTextLabel N_Label { get; private set; }
    #endregion

    #region fileds
    private bool _isTyping;
    #endregion

    #region op
    public void Popup(string dialog,Vector2 pos)
    {
        if (_isTyping) return;
        if (dialog is null) return;
        _isTyping = true;
        if (!Visible) Show();
        GlobalPosition = pos;
        dialog = Tr(dialog);
        N_Label.Text = dialog;
        N_Label.VisibleRatio = 0;
        var tween = CreateTween();
        tween.TweenProperty(N_Label, "visible_ratio", 1, GetDuration(dialog));
        tween.TweenCallback(Callable.From(() => _isTyping = false));
    }

    public void Close()
    {
        GlobalPosition = Vector2.Inf;
        Hide();
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
