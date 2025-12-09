using System.Text;
using Godot;

public class GameDialogueWorld
{
    #region life
    public GameDialogueWorld(DialogBox dialogBox, TextBox textBox)
    {
        _dialogBox = dialogBox;
        _textBox = textBox;
        _dialogBox.Confirmed += NextDialog;
    }
    #endregion
    
    #region members
    private DialogBox _dialogBox;
    private TextBox _textBox;
    #endregion
    
    #region fields
    private int _currentIndex;
    private string[] _dialogs;
    private bool _isInDialog;
    #endregion
    
    #region op
    public void PopupText(string[] texts,Node2D spawn)
    {
        var sb = new StringBuilder();
        foreach (var text in texts) sb.AppendLine(text);
        _textBox.Popup(sb.ToString(),spawn.GlobalPosition);
    }
    
    public void Dialog(string[] dialogs)
    {
        _dialogs = dialogs;
        StartDialog();
    }

    private void DialogSingle(string dialog)
    {
        _dialogBox.Talk(dialog);
    }

    private void NextDialog()
    {
        if (_currentIndex == _dialogs.Length)
        {
            EndDialog();
            return;
        }
        DialogSingle(_dialogs[_currentIndex]);
        _currentIndex++;
    }
    #endregion

    #region handle
    private void StartDialog()
    {
        if (_isInDialog) return;
        _isInDialog = true;
        _dialogBox.GetTree().Paused = true;
        NextDialog();
    }

    private void EndDialog()
    {
        _isInDialog = false;
        _dialogBox.Close();
        _dialogBox.GetTree().Paused = false;
        _currentIndex = 0;
    }
    #endregion
}