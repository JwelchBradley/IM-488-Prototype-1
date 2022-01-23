/*****************************************************************************
// File Name :         FullAutoGun.cs
// Author :            Jacob Welch
// Creation Date :     21 January 2022
//
// Brief Description : Handles the shooting of full auto guns.
*****************************************************************************/
using System.Collections;
using UnityEngine;

public class FullAutoGun : Gun
{
    /// <summary>
    /// Holds true if the player is holding down the shoot key.
    /// </summary>
    private bool continueShooting = false;

    /// <summary>
    /// Holds reference to the shooting routine currently active.
    /// </summary>
    private Coroutine shootRoutine;

    public override void Shoot(bool shouldShoot)
    {
        if (shouldShoot)
        {
            float potentialShootTime = timeLastShot + (1 / fireRate);

            continueShooting = true;

            if(shootRoutine == null)
            {
                shootRoutine = StartCoroutine(ShootRoutine());
            }
            else if (!shotQueued && tapStaggerTime + Time.time >= potentialShootTime)
            {
                shotQueued = true;
                StartCoroutine(QueueShot(potentialShootTime - Time.time));
            }
        }
        else
        {
            continueShooting = false;
        }
    }

    /// <summary>
    /// Queues a shot to shoot so players can tap fire if they want.
    /// </summary>
    /// <param name="shootTime">The wait time before the shot should take place.</param>
    /// <returns></returns>
    private IEnumerator QueueShot(float shootTime)
    {
        yield return new WaitForSeconds(shootTime);

        if(shootRoutine == null)
        {
            ShootBullet();
        }
    }

    /// <summary>
    /// Loops and continues shooting until the player wants to stop shooting.
    /// </summary>
    /// <returns></returns>
    private IEnumerator ShootRoutine()
    {
        while (continueShooting)
        {
            ShootBullet();

            yield return new WaitForSeconds(1.0f/fireRate);
        }

        shootRoutine = null;
    }
}
