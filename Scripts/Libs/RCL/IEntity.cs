using Godot.Collections;

public interface IEntity
{
    Array<Component> Components { get; }
    ComponentQuery E { get; }
}