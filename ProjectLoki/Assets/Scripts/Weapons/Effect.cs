using UnityEngine;
using System.Collections;

public abstract class Effect {

    public bool Destroyed { get; internal set; }

    public AttributeType AttributeTarget;

    public abstract void Tick(float delta);

    public abstract float ModifyAttribute(float attributeValue);
}
