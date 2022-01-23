using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FullAutoGun : Gun
{
    public override void Shoot(bool shouldShoot)
    {
        if (shouldShoot)
        {
            StartCoroutine(ShootRoutine());
        }
        else
        {
            StopAllCoroutines();
        }
    }

    private IEnumerator ShootRoutine()
    {
        while (true)
        {
            ShootBullet();

            yield return new WaitForSeconds(1.0f/fireRate);
        }
    }
}
