using Godot;

public class GameDialogWorld
{
    #region life
    public GameDialogWorld(DialogBox dialogBox)
    {
        _dialogBox = dialogBox;
        _dialogBox.Confirmed += NextDialog;
    }
    #endregion
    
    #region members
    private DialogBox _dialogBox;
    #endregion
    
    #region fields
    private int _currentIndex;
    private string[] _dialogs;
    private bool _isInDialog;
    #endregion
    
    #region op
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