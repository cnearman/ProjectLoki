using System.Collections.Generic;
using System.Linq;
using ExitGames.Client.Photon;

/// <summary>
/// 
/// </summary>
public class BaseAttribute : BaseClass 
{
    protected float _currentValue;
    public float BaseValue;

    public List<Effect> Effects;

    public bool IsModified { get; protected set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="baseValue"></param>
    protected virtual void Awake()
    {
        Effects = new List<Effect>();
        _currentValue = BaseValue;
        /// PhotonPeer.RegisterType(typeof(BaseAttribute), (byte)'BAttr', SerializeVector2, DeserializeVector2);
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

    /*
    public static byte[] SerializeBaseAttribute(object customObject)
    {
        BaseAttribute attr = customObject as BaseAttribute;

        byte[] bytes = new byte[4];
        int index = 0;
        Protocol.Serialize(attr.GetCurrentValue(), bytes, ref index);
        return bytes;
    }

    public static byte[] SerializeBaseAttribute(object customObject)
    {
        BaseAttribute attr = customObject as BaseAttribute;

        byte[] bytes = new byte[4];
        int index = 0;
        Protocol.Deserialize(attr.GetCurrentValue(), bytes, ref index);
        return bytes;
    }
    */
}

