using System.Collections.Generic;

/// <summary>
/// 
/// </summary>
public class BaseAttribute
{
    protected float _currentValue;
    private float _baseValue;

    public List<Effect> Effects;

    public bool IsModified { get; protected set; }

    /// <summary>
    /// 
    /// </summary>
    public BaseAttribute() : this(0) { }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="baseValue"></param>
    public BaseAttribute(float baseValue)
    {
        Effects = new List<Effect>();
        _baseValue = baseValue;
        _currentValue = _baseValue;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public virtual float GetCurrentValue()
    {
        if (IsModified)
        {
            foreach(Effect effect in Effects)
            {
                _currentValue = effect.ModifyAttribute(_currentValue);
            }
            IsModified = false;
        }

        return _currentValue;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="delta"></param>
    public virtual void Tick(float delta)
    {
        foreach(Effect effect in Effects)
        {
            effect.Tick(delta);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="effect"></param>
    public void AddEffect(Effect effect)
    {
        Effects.Add(effect);
        IsModified = true;
    }
}

