using System.Collections.Generic;
using System.Linq;

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
        Effects.ForEach(x => Process(x, delta));
    }

    public virtual void Process(Effect e, float delta)
    {
        if (e.Destroyed)
        {
            RemoveEffect(e);
        }
        else
        {
            e.Tick(delta);
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

    public void RemoveEffect(Effect effect)
    {
        Effects.Remove(effect);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (info.sender.isMasterClient)
        { 
            stream.SendNext(GetCurrentValue());
        }
        else // IsReading
        {
            _currentValue = (float) stream.ReceiveNext();
        }
    }
}

