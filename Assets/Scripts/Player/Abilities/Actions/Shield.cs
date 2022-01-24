/*****************************************************************************
// File Name :         Shield.cs
// Author :            Jacob Welch
// Creation Date :     23 January 2022
//
// Brief Description : Spawns a shield in front of the player.
*****************************************************************************/
using UnityEngine;

public class Shield : AbilityAction
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
