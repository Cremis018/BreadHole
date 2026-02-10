using Godot;

[GlobalClass]
public abstract partial class Item : Resource
{
    [Export] public int Id { get; set; }
    public virtual void UsePrimarily(GameWorld world){}
    public virtual void UseSecondary(GameWorld world){}
}