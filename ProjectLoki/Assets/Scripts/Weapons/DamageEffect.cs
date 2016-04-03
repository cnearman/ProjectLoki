using System;

public class DamageEffect : Effect
{
    private float _damageValue;

    public DamageEffect(float value) : base()
    {
        _damageValue = value;
        AttributeTarget = AttributeType.Health;
    }

    public override float ModifyAttribute(float attributeValue)
    {
        if (Destroyed)
        {
            return attributeValue;
        }

        Destroyed = true;
        return attributeValue - _damageValue;
    }

    public override void Tick(float delta) { }
}
