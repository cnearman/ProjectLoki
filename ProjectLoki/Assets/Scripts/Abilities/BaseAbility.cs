using UnityEngine;

public class BaseAbility : IAbility
{
    public float CurrentCooldown { get; set; }
    public float CooldownTime { get; set; }

    private string _notification;

    public BaseAbility() { }

    public BaseAbility(string notification)
    {
        _notification = notification;
    }

    public void Activate()
    {
        if (CurrentCooldown == 0)
        {
            Debug.Log("Ability Activated: " + _notification);
            CurrentCooldown = CooldownTime;
        }
    }

    /// <summary>
    /// Method for adjusting things that require time changes.
    /// I.e. Cooldown, ammo accumulation, channels etc.
    /// </summary>
    /// <param name="delta"></param>
    public void Tick(float delta)
    {
        CurrentCooldown = Mathf.Clamp(this.CurrentCooldown - delta, 0, float.MaxValue);
    }
}