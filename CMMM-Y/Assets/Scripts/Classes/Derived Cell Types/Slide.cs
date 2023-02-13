using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slide : Cell
{
    public override (bool, bool) Push(Direction_e dir, int bias)
    {
        if(dir == this.GetDirection() || ((int)dir + 2) % 4 == (int)this.GetDirection())
            return base.Push(dir, bias);
        return (false, false);
    }
}
