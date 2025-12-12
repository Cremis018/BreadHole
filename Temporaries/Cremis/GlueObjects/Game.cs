using Godot;

public partial class Game : Node
{
    #region props
    public static Game Instance { get; private set; }
    
    public GameMapWorld Map { get; set; }
    public GameMiscWorld Misc { get; set; }
    public GameDialogueWorld Dialogue { get; set; }
    public GamePocketWorld Pocket { get; set; }
    public GameGlobalWorld Global { get; set; }
    public TriggerHandler TriggerHandler { get; set; }
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