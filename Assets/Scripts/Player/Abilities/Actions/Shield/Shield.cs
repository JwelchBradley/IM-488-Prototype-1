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

    GameObject selectControl;
    SelectionBehavior sb;
    /// <summary>
    /// Handles the usage of this ability.
    /// </summary>
    protected override bool AbilityActivate()
    {
        selectControl = GameObject.Find("SelectControl");
        sb = selectControl.GetComponent<SelectionBehavior>();
        if (sb.ability1 == "shield" || sb.ability2 == "shield")
        {
            Transform pivot = transform.Find("Pivot").Find("Shield Spawn Pos");
            GameObject shield = Instantiate(ability.shield, pivot.position, Quaternion.identity);
            shield.GetComponent<ShieldBehaviour>().Initialization(ability.Duration, ability.health, pivot);
            shield.transform.localScale *= ability.size;
            return true;
        }
        else
        {
            return false;
        }
    }
}
