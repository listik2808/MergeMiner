using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BitonShovel : Shovel
{
    public override void NewItemDiscovered()
    {
        _isOpen = true;
    }
}
