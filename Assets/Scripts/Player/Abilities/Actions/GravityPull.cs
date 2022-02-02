/*****************************************************************************
// File Name :         GravityPull.cs
// Author :            Jacob Welch
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

    private Rigidbody currentGrabbed;

    private Transform moveToTarget;

    private CinemachineVirtualCamera rockCam;

    private Transform pivot;

    private void Awake()
    {
        gameObject.AddComponent<LineRenderer>();

        pivot = transform.Find("Pivot");
        gunTip = GameObject.Find("Bullet Spawn Pos").transform;
        rockCam = GameObject.Find("RockCam").GetComponent<CinemachineVirtualCamera>();
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
            //lr.enabled = true;

            // Creates the object that the rock follows
            //moveToTarget = Instantiate(new GameObject("empty"), currentGrabbed.position - (currentGrabbed.position - ability.mainCam.transform.position).normalized * (Vector3.Distance(currentGrabbed.position, transform.position) - ability.DistFromPlayer), Quaternion.identity, ability.mainCam.transform).transform;
            //moveToTarget.position += ability.OffsetFromCamera * -ability.mainCam.transform.right;

            moveToTarget = Instantiate(new GameObject("empty"), transform.position + pivot.transform.forward * ability.DistFromPlayer, Quaternion.identity, pivot).transform;
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

        if (Physics.Raycast(ability.mainCam.transform.position, ability.mainCam.transform.forward, out hit, Mathf.Infinity))
        {
            target = hit.point;
        }
        else
        {
            target = ability.mainCam.transform.position + ability.mainCam.transform.forward * 1000;
        }

        currentGrabbed.velocity = (target - currentGrabbed.position).normalized * ability.PushSpeed;
    }

    private void Reset()
    {
        // Resets variables
        currentGrabbed = null;
        //lr.enabled = false;
        rockCam.Priority = 0;
        Destroy(moveToTarget.gameObject);
    }

    #region Draw Line
    /*
    /// <summary>
    /// Renderers the line after everything else.
    /// </summary>
    private void LateUpdate()
    {
        //DrawLine();
    }

    /// <summary>
    /// Draws the line between the player and the target.
    /// </summary>
    private void DrawLine()
    {
        if (currentGrabbed != null)
        {
            lr.SetPosition(0, gunTip.position);
            lr.SetPosition(1, currentGrabbed.position);
            return;
        }
    }*/
    #endregion
}
