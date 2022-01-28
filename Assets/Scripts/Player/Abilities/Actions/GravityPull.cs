/*****************************************************************************
// File Name :         GravityPull.cs
// Author :            Jacob Welch
// Creation Date :     23 January 2022
//
// Brief Description : Pulls available objects towards the player.
*****************************************************************************/
using System.Collections;
using UnityEngine;

public class GravityPull : AbilityAction
{
    private LineRenderer lr;

    private Transform gunTip;

    private Rigidbody currentGrabbed;

    private Transform moveToTarget;

    private void Awake()
    {
        gameObject.AddComponent<LineRenderer>();

        // Initilaizes line renderer
        Invoke("InitliazeLineRenderer", 0.01f);

        gunTip = GameObject.Find("Bullet Spawn Pos").transform;
    }

    private void InitliazeLineRenderer()
    {
        lr = GetComponent<LineRenderer>();
        lr.material = ability.LineMaterial;
        lr.widthCurve = ability.LineWidthCurve;
        lr.positionCount = 2;
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

            Vector3 dir = transform.position - hit.transform.position;

            //currentGrabbed.velocity = dir.normalized * ability.PushPullSpeed;
            lr.enabled = true;
            moveToTarget = Instantiate(new GameObject("empty"), currentGrabbed.position, Quaternion.identity, Camera.main.transform).transform;

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
            if(currentGrabbed == null)
            {
                break;
            }

            if (Input.GetKey(KeyCode.Mouse0))
            {
                moveToTarget.position += (moveToTarget.position - transform.position).normalized*Time.deltaTime * ability.PushPullSpeed;
            }
            else if (Input.GetKey(KeyCode.Mouse1))
            {
                Vector3 newPos = moveToTarget.position - (moveToTarget.position - transform.position).normalized * Time.deltaTime * ability.PushPullSpeed;

                //isLeft();
                if (true)
                {
                    moveToTarget.position = newPos;
                }
            }

            Vector3 dir = moveToTarget.position - currentGrabbed.position;
            float speed = Mathf.Lerp(0, 1, Vector3.Distance(currentGrabbed.position, moveToTarget.position)/ability.DistMod);
            currentGrabbed.velocity = dir.normalized * ability.FollowTetherSpeed * speed;

            yield return null;
        }

        currentGrabbed = null;
        lr.enabled = false;
    }

    private void isLeft()
    {
        Vector3 delta = (moveToTarget.position - currentGrabbed.position).normalized;
        Vector3 cross = Vector3.Cross(delta, (currentGrabbed.position - gunTip.position).normalized);

        if (cross == Vector3.zero)
        {
            // Target is straight ahead
            Debug.Log(0);
        }
        else if (cross.y > 0)
        {
            // Target is to the right
            Debug.Log(1);
        }
        else
        {
            // Target is to the left
            Debug.Log(-1);
        }
    }

    private void LateUpdate()
    {
        DrawLine();
    }

    private void DrawLine()
    {
        if (currentGrabbed != null)
        {
            lr.SetPosition(0, gunTip.position);
            lr.SetPosition(1, currentGrabbed.position);
            return;
        }
    }
}
