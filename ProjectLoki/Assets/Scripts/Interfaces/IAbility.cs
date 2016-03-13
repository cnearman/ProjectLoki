using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface IAbility
{
    void Activate();

    void Tick(float delta);
}
