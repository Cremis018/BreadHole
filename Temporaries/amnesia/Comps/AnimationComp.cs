using Godot;

public partial class AnimationComp : Component
{
    #region nodes
    [ExportGroup("Nodes")]
    [Export] public AnimationPlayer N_AnimationPlayer { get; private set; }
    #endregion

    #region life
    public override void _Ready()
    {
        InitNodes();
        EventSystem.Subscribe<PlayAnimationEvent>(OnPlayAnimation);
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        EventSystem.Unsubscribe<PlayAnimationEvent>(OnPlayAnimation);
    }

    private void InitNodes()
    {
        N_AnimationPlayer ??= GetParent().GetNodeOrNull<AnimationPlayer>("AnimationPlayer");
        if (N_AnimationPlayer == null)
            N_AnimationPlayer = GetParent().GetNodeOrNull<AnimationPlayer>("../AnimationPlayer");
    }
    #endregion

    #region op
    public void PlayAnimation(string animationName, bool loop = false)
    {
        if (N_AnimationPlayer == null)
        {
            GD.Print($"AnimationPlayer 未找到，无法播放动画: {animationName}");
            return;
        }
        if (!N_AnimationPlayer.HasAnimation(animationName))
        {
            GD.Print($"动画不存在: {animationName}");
            return;
        }
        var animation = N_AnimationPlayer.GetAnimation(animationName);
        if (animation != null)
            animation.LoopMode = loop ? Animation.LoopModeEnum.Linear : Animation.LoopModeEnum.None;
        N_AnimationPlayer.Play(animationName);
        GD.Print($"播放动画: {animationName}");
    }

    public void StopAnimation()
    {
        if (N_AnimationPlayer != null && N_AnimationPlayer.IsPlaying())
            N_AnimationPlayer.Stop();
    }

    private void OnPlayAnimation(PlayAnimationEvent e) => PlayAnimation(e.AnimationName, e.Loop);
    #endregion
}

