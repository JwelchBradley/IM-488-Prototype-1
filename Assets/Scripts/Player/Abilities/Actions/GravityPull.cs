/*****************************************************************************
// File Name :         GravityPull.cs
// Author :            Jacob Welch
// Creation Date :     23 January 2022
//
// Brief Description : Pulls available objects towards the player.
*****************************************************************************/
using UnityEngine;

public class GravityPull : AbilityAction
{
    /// <summary>
    /// Handles the usage of this ability.
    /// </summary>
    protected override bool AbilityActivate()
    {
        //Debug.Log(ability.pullSpeed);
        return true;
    }
}
