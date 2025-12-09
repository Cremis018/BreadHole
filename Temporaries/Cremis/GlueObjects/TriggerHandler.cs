using Godot;
using System;
using System.Linq;

public class TriggerHandler
{
    #region op
    public void TriggerEvents(TriggerComp trigger)
    {
        EventApplier.Apply(trigger);
    }

    public void TriggerEntity(IEntity entity)
    {
        if (entity is not Node node) return;
        node.GetChildren().OfType<TriggerComp>().ToList().ForEach(TriggerEvents);
        
        //对于一些有类似触发功能的组件，比如InfoBoardComp，需要单独处理。
        //目前来说只有这个组件是特殊的，如果以后有更多类似的组件，后面就需要用静态字典来处理。
        if (entity.E.HasComponent<InfoBoardComp>())
            entity.E.GetComponent<InfoBoardComp>().ShowText();
    }
    #endregion
}
