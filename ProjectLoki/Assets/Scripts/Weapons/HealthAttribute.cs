/// <summary>
/// 
/// </summary>
public class HealthAttribute : BaseAttribute
{
    private readonly float _minimumHealth = 0.0f;

    public float MaxHealth { get; private set; }

    public bool IsDead { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="maxValue"></param>
    public HealthAttribute(float maxValue) : base(maxValue)
    {
        MaxHealth = maxValue;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="damageValue"></param>
    public void Damage(float damageValue)
    {
        _currentValue -= damageValue;
        IsDead = _currentValue <= _minimumHealth;
        IsModified = true;
    }
}
