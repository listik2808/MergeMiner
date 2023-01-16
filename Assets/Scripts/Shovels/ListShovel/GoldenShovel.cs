using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldenShovel : Shovel
{
    public override void NewItemDiscovered()
    {
        _isOpen = true;
    }
}
