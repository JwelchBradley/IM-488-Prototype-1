/*****************************************************************************
// File Name :         GravityPull.cs
// Author :            Jacob Welch & Jessica Barthelt
// Creation Date :     23 January 2022
//
// Brief Description : Pulls available objects towards the player.
*****************************************************************************/
using Cinemachine;
using System.Collections;
using UnityEngine;

public class GravityPull : AbilityAction
{
    private Transform gunTip;

    private Transform currentGrabbedTransform;
    private Rigidbody currentGrabbed;

    private Transform moveToTarget;

    private CinemachineVirtualCamera rockCam;

    private Transform pivot;

    GameObject indicator;

    private void Awake()
    {
        if(ability != null && !isManager)
        {
            indicator = Instantiate(ability.Indicator);
            indicator.SetActive(false);
        }

        pivot = transform.Find("Pivot");
        gunTip = GameObject.Find("Bullet Spawn Pos").transform;
        rockCam = GameObject.Find("RockCam").GetComponent<CinemachineVirtualCamera>();
        tpc = GetComponent<ThirdPersonController>();
    }

    private void FixedUpdate()
    {
        if (indicator != null)
        {
            if (canUse)
            {
                RaycastHit hit;
                bool foundTarget = Physics.Raycast(ability.mainCam.transform.position, ability.mainCam.transform.forward, out hit, ability.PushPullDist, ability.PushPullableMask) ||
                                   Physics.BoxCast(transform.position, Vector3.one * ability.AimAssist, ability.mainCam.transform.forward, out hit, Quaternion.identity, ability.PushPullDist, ability.PushPullableMask);

                if (foundTarget)
                {
                    indicator.transform.position = hit.transform.position - (hit.transform.position - ability.mainCam.transform.position).normalized * 5;
                    indicator.transform.localScale = Vector3.one * Vector3.Distance(hit.point, ability.mainCam.transform.position) * .2f;
                    indicator.SetActive(true);
                }
                else
                {
                    indicator.SetActive(false);
                }
            }
            else
            {
                indicator.SetActive(false);
            }
        }
        else if(ability != null && !isManager)
        {
            indicator = Instantiate(ability.Indicator);
            indicator.SetActive(false);
        }
    }

    /// <summary>
    /// Handles the usage of this ability.
    /// </summary>
    protected override bool AbilityActivate()
    {
        RaycastHit hit;
        bool foundTarget = Physics.Raycast(ability.mainCam.transform.position, ability.mainCam.transform.forward, out hit, ability.PushPullDist, ability.PushPullableMask) ||
                           Physics.BoxCast(transform.position, Vector3.one * ability.AimAssist, ability.mainCam.transform.forward, out hit, Quaternion.identity, ability.PushPullDist, ability.PushPullableMask);

        if (foundTarget)
        {
            currentGrabbed = hit.transform.gameObject.GetComponent<Rigidbody>();
            currentGrabbed.gameObject.layer = LayerMask.NameToLayer("Held");
            currentGrabbed.gameObject.GetComponent<AsteroidBehavior>().currentlyHeld = true;
            currentGrabbedTransform = hit.transform;
            //lr.enabled = true;

            // Creates the object that the rock follows
            //moveToTarget = Instantiate(new GameObject("empty"), currentGrabbed.position - (currentGrabbed.position - ability.mainCam.transform.position).normalized * (Vector3.Distance(currentGrabbed.position, transform.position) - ability.DistFromPlayer), Quaternion.identity, ability.mainCam.transform).transform;
            //moveToTarget.position += ability.OffsetFromCamera * -ability.mainCam.transform.right;

            Vector3 spawnPos = transform.position + pivot.transform.forward * ability.DistFromPlayer;
            if (tpc.CurrentMoveState == ThirdPersonController.moveState.fast)
            {
                spawnPos = transform.position + ability.mainCam.transform.forward * ability.DistFromPlayer;
            }

            moveToTarget = Instantiate(new GameObject("empty"), spawnPos, Quaternion.identity, pivot).transform;
            moveToTarget.position += ability.XOffsetFromPlayer * -pivot.transform.right;

            rockCam.Priority = 100;

            StartCoroutine(PushPullRoutine());
            return true;
        }

        return false;

    }
    

    private IEnumerator PushPullRoutine()
    {
        float startTime = Time.time;

        while (Time.time < startTime+ability.Duration)
        {
            // If the object no longer exists leave this loop
            if(currentGrabbed == null)
            {
                tpc.StopCasting();
                break;
            }

            // Throws the rock in the direction the player is looking
            if (Input.GetKey(KeyCode.Mouse0))
            {
                Click();
                tpc.StopCasting();
                break;
            }

            MakeMoveTargetFollow();
            yield return null;
        }

        Reset();
    }

    private void MakeMoveTargetFollow()
    {
        // Moves the rock towards the follow position
        Vector3 dir = moveToTarget.position - currentGrabbed.position;
        float speed = Mathf.Lerp(0, 1, Vector3.Distance(currentGrabbed.position, moveToTarget.position) / ability.DistMod);
        currentGrabbed.velocity = dir.normalized * ability.FollowTetherSpeed * speed;
    }

    private void Click()
    {
        RaycastHit hit;
        Vector3 target;

        if (Physics.BoxCast(ability.mainCam.transform.position, Vector3.one * 0.5f, ability.mainCam.transform.forward.normalized, out hit, Quaternion.identity, Mathf.Infinity) && !hit.transform.gameObject.layer.Equals(LayerMask.NameToLayer("Held")))
        {
            target = hit.point;
        }
        else
        {
            target = ability.mainCam.transform.position + ability.mainCam.transform.forward * 1000;
        }

        currentGrabbed.velocity = (target - currentGrabbed.position).normalized * 300;
    }

    private void Reset()
    {
        if(currentGrabbed != null)
        {
            //currentGrabbed.gameObject.GetComponent<AsteroidBehavior>().currentlyHeld = false;
            currentGrabbed.gameObject.layer = LayerMask.NameToLayer("Pullable");
        }

        // Resets variables
        currentGrabbed = null;
        StartCooldown();
        //lr.enabled = false;
        rockCam.Priority = 0;
        Destroy(moveToTarget.gameObject);
    }
}
