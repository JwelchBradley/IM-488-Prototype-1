using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMPBulletController : BulletController
{
    private float stunDuration;

    private Renderer renderer;

    private void OnEnable()
    {
        renderer = GetComponent<Renderer>();
    }

    public float StunDuration
    {
        set
        {
            stunDuration = value;
        }
    }

    protected override void CollisionEvent(Collision other)
    {
        if(other.gameObject.TryGetComponent(out BaseEnemy be))
        {
            be.enabled = false;
            StartCoroutine(reenableEnemy(be));
            renderer.enabled = false;
        }
    } 

    private IEnumerator reenableEnemy(BaseEnemy be)
    {
        yield return new WaitForSeconds(stunDuration);
        be.enabled = true;
        Debug.Log(true);
        Destroy(gameObject);
    }
}
