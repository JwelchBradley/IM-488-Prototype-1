/*
 * Jessica Barthelt
Code for this used from https://www.youtube.com/watch?v=BC3AKOQUx04
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageIndicator : MonoBehaviour
{

    private const float timerMax = 5.0f;
    private float timer = timerMax;

    private CanvasGroup canvasGroup = null;
    protected CanvasGroup CanvasGroup
    {
        get
        {
            if (canvasGroup == null)
            {
                canvasGroup = GetComponent<CanvasGroup>();
                if (canvasGroup == null)
                {
                    canvasGroup = gameObject.AddComponent<CanvasGroup>();
                }
            }
            return canvasGroup;
        }
    }

    private RectTransform rec = null;
    protected RectTransform Rect
    {
       get
        {
            if (rec == null)
            {
                rec = GetComponent<RectTransform>();
                if (rec == null)
                {
                    rec = gameObject.AddComponent<RectTransform>();
                }
            }
            return rec;
        }

    }

    public Transform target;
    Transform playerPos;

    IEnumerator indicatorCD = null;
    private Action unRegister = null;

    private Quaternion targetRot = Quaternion.identity;
    private Vector3 targetPos = Vector3.zero;

    public void Register(Transform target, Transform player, Action unResister)
    {
        this.target = target;
        this.playerPos = player;
        this.unRegister = unRegister;

        StartCoroutine(TurntoTarget());
        StartTimer();

    }

    public void Restart()
    {
        timer = timerMax;
        StartTimer();
    }

    private void StartTimer()
    {
        if(indicatorCD != null)
        {
            StopCoroutine("indicatorCD");
        }
        indicatorCD = Countdown();
        StartCoroutine("indicatorCD");
    }


    IEnumerator TurntoTarget()
    {
        while(enabled)
        {
            if(target)
            {
                targetPos = target.position;
                targetRot = target.rotation;
            }
            Vector3 direction = playerPos.position - targetPos;
            targetRot = Quaternion.LookRotation(direction);
            targetRot.z = -targetRot.y;
            targetRot.y = 0;
            targetRot.x = 0;

            Vector3 northDirection = new Vector3(0, 0, playerPos.eulerAngles.y);
            Rect.localRotation = targetRot * Quaternion.Euler(northDirection);

            yield return null;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator Countdown()
    {
        while(CanvasGroup.alpha < 1.0f)
        {
            CanvasGroup.alpha += 4 * Time.deltaTime;
            yield return null;
        }
        while(timer > 0)
        {
            timer--;
            yield return new WaitForSeconds(1f);
        }
        while (CanvasGroup.alpha > 0.0f)
        {
            CanvasGroup.alpha -= 2 * Time.deltaTime;
            yield return null;
        }
        unRegister();
        Destroy(gameObject);
    }
}
