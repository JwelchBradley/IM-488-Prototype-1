using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeEnemy : AbilityAction
{
    protected override void AbilityActivate()
    {
        Debug.Log(ability.name);
    }
}
