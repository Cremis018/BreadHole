using Godot;

public partial class Game : Node
{
    #region props
    public static Game Instance { get; private set; }
    
    public GameMapWorld Map { get; private set; }
    public GameDialogWorld Dialog { get; private set; }
    public GamePocketWorld Pocket { get; private set; }
    #endregion

    #region nodes
    [ExportGroup("Nodes")]
    [Export] public Node2D N_MapRoot { get; private set; }
    [Export] public DialogBox N_DialogBox { get; private set; }
    #endregion
    
    #region life
    public override void _EnterTree()
    {
        Instance ??= this;
    }
    
    public override void _Ready()
    {
        Map ??= new GameMapWorld(N_MapRoot);
        Dialog ??= new GameDialogWorld(N_DialogBox);
    }
    #endregion
}