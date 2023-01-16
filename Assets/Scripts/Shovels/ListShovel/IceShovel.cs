using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceShovel : Shovel
{
    public override void NewItemDiscovered()
    {
        _isOpen = true;
    }
}
