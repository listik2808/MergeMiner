using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalShovel : Shovel
{
    public override void NewItemDiscovered()
    {
        _isOpen = true;
    }
}
