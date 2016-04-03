/// <summary>
/// 
/// </summary>
public class HealthAttribute : BaseAttribute
{
    private readonly float _minimumHealth = 0.0f;

    public float MaxHealth { get; private set; }

    public bool IsDead { get { return GetCurrentValue() <= _minimumHealth; } }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="maxValue"></param>
    public HealthAttribute(float maxValue) : base(maxValue)
    {
        MaxHealth = maxValue;
    }
}
