using Godot;

public partial class Game : Node
{
    #region props
    public static Game Instance { get; private set; }
    
    public GameMapWorld Map { get; private set; }
    public GameMiscWorld Misc { get; private set; }
    public GameDialogueWorld Dialogue { get; private set; }
    public GamePocketWorld Pocket { get; private set; }
    public GameGlobalWorld Global { get; private set; }
    public TriggerHandler TriggerHandler { get; private set; }
    #endregion

    #region nodes
    [ExportGroup("Nodes")]
    [Export] public Node2D N_MapRoot { get; private set; }
    [Export] public DialogBox N_DialogBox { get; private set; }
    [Export] public TextBox N_TextBox { get; private set; }
    [Export] public TimerTriggerManager N_TimerTriggerManager { get; private set; }
    
    #endregion
    
    #region life
    public override void _EnterTree()
    {
        Instance ??= this;
    }
    
    public override void _Ready()
    {
        Map ??= new GameMapWorld(N_MapRoot);
        Dialogue ??= new GameDialogueWorld(N_DialogBox,N_TextBox);
    }
    #endregion
}