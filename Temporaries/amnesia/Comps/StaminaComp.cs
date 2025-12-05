using Godot;

public partial class StaminaComp : Component
{
    #region props
    [Export] public float MaxStamina { get; set; } = 10.0f;
    [Export] public float RegenerationRate { get; set; } = 10.0f;
    [Export] public float ConsumptionRate { get; set; } = 20.0f;
    [Export] public bool EnableRegeneration { get; set; } = false;
    
    public float CurrentStamina => _currentStamina;
    public float StaminaPercent => MaxStamina > 0 ? _currentStamina / MaxStamina : 0f;
    public bool IsExhausted => _currentStamina <= 0f;
    #endregion

    #region private
    private float _currentStamina;
    private bool _isConsuming = false;
    #endregion

    #region life
    public override void _Ready()
    {
        _currentStamina = MaxStamina;
    }

    public override void _Process(double delta)
    {
        if (_isConsuming) ConsumeStamina((float)delta * ConsumptionRate);
        // else if (EnableRegeneration && _currentStamina < MaxStamina)
        //     RegenerateStamina((float)delta * RegenerationRate);
    }
    #endregion

    #region op
    public bool ConsumeStamina(float amount)
    {
        if (_currentStamina <= 0f) return false;
        _currentStamina = Mathf.Max(0f, _currentStamina - amount);
        if (_currentStamina <= 0f) OnStaminaExhausted();
        return true;
    }

    public void RegenerateStamina(float amount) => _currentStamina = Mathf.Min(MaxStamina, _currentStamina + amount);
    public void SetStamina(float value) => _currentStamina = Mathf.Clamp(value, 0f, MaxStamina);
    public void StartConsuming() => _isConsuming = true;
    public void StopConsuming() => _isConsuming = false;
    public bool HasEnoughStamina(float requiredAmount) => _currentStamina >= requiredAmount;

    public bool TryConsumeStamina(float amount)
    {
        if (!HasEnoughStamina(amount)) return false;
        ConsumeStamina(amount);
        return true;
    }

    public void ResetStamina() => _currentStamina = MaxStamina;

    private void OnStaminaExhausted()
    {
        GD.Print("体力已耗尽！游戏结束！");
        EventSystem.Emit(new GameOverEvent("体力耗尽"));
    }
    #endregion
}

