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
    private void Awake()
    {
        

    }

    protected override bool AbilityActivate()
    {
        
            Transform pivot = transform.Find("Pivot").Find("Shield Spawn Pos");
            GameObject shield = Instantiate(ability.shield, pivot.position, Quaternion.identity);
            shield.GetComponent<ShieldBehaviour>().Initialization(ability.Duration, ability.health, pivot);
            shield.transform.localScale *= ability.size;
            return true;
        
       
    }
}
