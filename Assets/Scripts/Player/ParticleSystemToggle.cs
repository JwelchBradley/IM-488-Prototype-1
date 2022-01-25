using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ParticleSystemToggle : MonoBehaviour
{
    ParticleSystem ps;

    private void spawnLines(bool shouldSpawn)
    {
        if(shouldSpawn)
        ps.Play();

        else
        {
            despawnLines();
        }
    }

    private void despawnLines()
    {
        ps.Stop();
    }

    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();

        GameObject.Find("Player").GetComponent<ThirdPersonController>().MoveFast.AddListener(spawnLines);
    }
}