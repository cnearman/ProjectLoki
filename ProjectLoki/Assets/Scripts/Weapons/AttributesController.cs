using System.Collections.Generic;
using UnityEngine;

public class AttributesController : BaseClass
{
    private Player Owner;

    private Dictionary<AttributeType, BaseAttribute> _attributes;

    void Awake()
    { 
        _attributes = new Dictionary<AttributeType, BaseAttribute>();
        _attributes[AttributeType.Health] = new HealthAttribute(20.0f);
        Owner = GetComponent<Player>();
    }

    void Update()
    {
        if (((HealthAttribute) _attributes[AttributeType.Health]).IsDead)
        {
            Owner.Die();
        }

        _attributes[AttributeType.Health].Tick(Time.deltaTime);
    }

    public void ApplyEffect(Effect effect, AttributeType type)
    {
        if (_attributes.ContainsKey(type))
        {
            _attributes[type].AddEffect(effect);
        }
    }

    public void ApplyEffects(IEnumerable<Effect> effects)
    {
        foreach(var effect in effects)
        {
            ApplyEffect(effect, effect.AttributeTarget);
            //RPC call that says to update UI with effects stuff
        }
    }
}
