using Godot.Collections;

public interface IEntity
{
    Dictionary<string,Component> Components { get; }
    ComponentQuery E { get; }
}