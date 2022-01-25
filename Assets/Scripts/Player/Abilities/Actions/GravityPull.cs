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
        Debug.Log(true);
        RaycastHit hit;
        bool foundTarget = Physics.Raycast(ability.mainCam.transform.position, ability.mainCam.transform.forward, out hit, ability.PushPullDist, ability.PushPullableMask) ||
                           Physics.BoxCast(transform.position, Vector3.one * ability.AimAssist, ability.mainCam.transform.forward, out hit, Quaternion.identity, ability.PushPullDist, ability.PushPullableMask);
        if (foundTarget)
        {
            Rigidbody rb = hit.transform.gameObject.GetComponent<Rigidbody>();

            Vector3 dir = transform.position - hit.transform.localPosition;

            Debug.Log(hit.transform.gameObject.name);
            Debug.Log(hit.transform.localPosition);

            rb.velocity = dir.normalized * ability.PushPullSpeed;

            return true;
        }

        return false;
    }
}
