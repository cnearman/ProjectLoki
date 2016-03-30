using UnityEngine;
using System.Collections;

public abstract class Effect {
    public abstract void Tick(float delta);

    public abstract float ModifyAttribute(float attributeValue);
}
