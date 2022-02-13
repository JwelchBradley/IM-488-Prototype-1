/*****************************************************************************
// File Name :         FreezeEnemy.cs
// Author :            Jacob Welch & Jessica Barthelt
// Creation Date :     21 January 2022
//
// Brief Description : Freezes enemies in place and stops them from takign 
                       actions.
*****************************************************************************/
using UnityEngine;

public class FreezeEnemy : AbilityAction
{
    private Gun gun;
    GameObject selectControl;
    SelectionBehavior sb;

    private void OnEnable()
    {
        //selectControl = GameObject.Find("SelectControl");
        //sb = selectControl.GetComponent<SelectionBehavior>();
        gun = GetComponentInChildren<Gun>();
    }

    /// <summary>
    /// Handles the usage of this ability.
    /// </summary>
    protected override bool AbilityActivate()
    {
            RaycastHit hit;
            bool foundTarget = Physics.Raycast(ability.mainCam.transform.position, ability.mainCam.transform.forward, out hit, Mathf.Infinity, ability.EMPMask) ||
                               Physics.BoxCast(transform.position, Vector3.one * 4, ability.mainCam.transform.forward, out hit, Quaternion.identity, 1000, ability.EMPMask);

            if (foundTarget)
            {
                gun.Shoot(ability.EMPBullet, hit.transform.position, ability.EMPFireSound, ability.StunDuration, ability.EMPGunData);
                return true;
            }
        else
        {
            gun.Shoot(ability.EMPBullet, ability.mainCam.transform.position + ability.mainCam.transform.forward * 1000, ability.EMPFireSound, ability.StunDuration, ability.EMPGunData);
            return true;
        }        
    }
}
