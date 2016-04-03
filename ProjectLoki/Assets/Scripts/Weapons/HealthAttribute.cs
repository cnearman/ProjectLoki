/// <summary>
/// 
/// </summary>
public class HealthAttribute : BaseAttribute
{
    private readonly float _minimumHealth = 0.0f;

    public float MaxHealth;

    public bool IsDead { get { return GetCurrentValue() <= _minimumHealth; } }

    protected override void Awake()
    {
        base.Awake();
    }
}
