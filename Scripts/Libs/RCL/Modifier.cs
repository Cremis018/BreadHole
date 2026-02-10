using System;

/* HACK : Modifier是为了临时解决Renderer中可能遇到的匿名函数订阅问题，
 为了能够让组件的修改一对一影响实体，而不是一个组件的修改让全体实体进入修改的逻辑。
 目前这种写法还是要手动声明Modifier数组，后续肯定要加入到Renderer中实现自动化
 */
public class Modifier<E,T>(E entity,Action<E,T> modifyMethod) where E : IEntity
{
    public void Modify(T param)
    {
        modifyMethod(entity,param);
    }
}