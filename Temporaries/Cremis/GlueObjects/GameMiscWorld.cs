using System.Collections.Generic;
using Godot;

public class GameMiscWorld
{
    #region props
    public Player Player { get; set; }
    public List<int> ItemWhitelist { get; set; }
    #endregion

    #region op
    public void TpPlayer(Vector2I pos)
    {
        
    }
    #endregion
}