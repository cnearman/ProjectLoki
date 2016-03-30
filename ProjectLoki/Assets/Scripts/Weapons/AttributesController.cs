using System.Collections.Generic;
using UnityEngine;

public class AttributeController : BaseClass
{
    private Dictionary<string, BaseAttribute> _attributes;

    public AttributeController()
    {
        _attributes = new Dictionary<string, BaseAttribute>();
        _attributes["health"] = new HealthAttribute(20.0f);
    }

    void Update()
    {
        _attributes["health"].Tick(Time.deltaTime);
    }

    public void ApplyEffect(Effect effect, string target)
    {
        if (_attributes.ContainsKey(target))
        {
            _attributes[target].AddEffect(effect);
        }
    }
}
