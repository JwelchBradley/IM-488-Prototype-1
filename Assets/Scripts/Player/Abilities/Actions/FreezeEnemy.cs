/*****************************************************************************
// File Name :         FreezeEnemy.cs
// Author :            Jacob Welch
// Creation Date :     21 January 2022
//
// Brief Description : Freezes enemies in place and stops them from takign 
                       actions.
*****************************************************************************/
using UnityEngine;

public class FreezeEnemy : AbilityAction
{
    /// <summary>
    /// Handles the usage of this ability.
    /// </summary>
    protected override bool AbilityActivate()
    {
        Debug.Log(ability.name);
        return true;
    }
}
