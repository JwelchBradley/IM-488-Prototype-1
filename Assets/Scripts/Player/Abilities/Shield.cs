using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : AbilityAction
{
    protected override void AbilityActivate()
    {
        Debug.Log(ability.name);
    }
}
