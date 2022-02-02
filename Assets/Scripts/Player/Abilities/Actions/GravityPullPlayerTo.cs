using System.Collections;
using UnityEngine;

public class GravityPullPlayerTo : AbilityAction
{

    protected override bool AbilityActivate()
    {
        RaycastHit hit;
        bool foundTarget = Physics.Raycast(ability.mainCam.transform.position, ability.mainCam.transform.forward, out hit, ability.PushPullDist, ability.PushPullableMask) ||
                           Physics.BoxCast(transform.position, Vector3.one * ability.AimAssist, ability.mainCam.transform.forward, out hit, Quaternion.identity, ability.PushPullDist, ability.PushPullableMask);

        if (foundTarget)
        {
            StartCoroutine(PullToRoutine());
            return true;
        }

        return false;
    }

    private IEnumerator PullToRoutine()
    {
        yield return null;
    }
}
