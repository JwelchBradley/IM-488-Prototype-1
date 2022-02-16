/******************************************************************************
// File Name :         BoosterController.cs
// Author :            Avery Macke
// Creation Date :     14 February 2022
//
// Brief Description : Toggles boosters on/off with movement
******************************************************************************/
using System.Collections.Generic;
using UnityEngine;

public class BoosterController : MonoBehaviour
{
    [Tooltip("List of all booster particle systems on player")]
    /* 0 - 1:   Forward
     * 2 - 3:   Back
     * 4 - 9:   Up
     * 10 - 11: Down
     * 12 - 14: Right
     * 15 - 17: Left
     */
    public List<GameObject> boosterList;

    private void Start()
    {
        for(int i = 0; i < boosterList.Count; i++)
        {
            boosterList[i].GetComponent<ParticleSystem>().Stop();
        }
    }

    /// <summary>
    /// Called every frame; tracks key presses
    /// </summary>
    private void Update()
    {
        // Forward
        if(Input.GetKeyDown(KeyCode.W))
        {
            ToggleBoosterOn(boosterList[0]);
            ToggleBoosterOn(boosterList[1]);
        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            ToggleBoosterOff(boosterList[0]);
            ToggleBoosterOff(boosterList[1]);
        }

        // Back
        if (Input.GetKeyDown(KeyCode.S))
        {
            ToggleBoosterOn(boosterList[2]);
            ToggleBoosterOn(boosterList[3]);
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            ToggleBoosterOff(boosterList[2]);
            ToggleBoosterOff(boosterList[3]);
        }

        // Up                                       // Inverted w flying
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ToggleBoosterOn(boosterList[4]);
            ToggleBoosterOn(boosterList[5]);
            ToggleBoosterOn(boosterList[6]);
            ToggleBoosterOn(boosterList[7]);
            ToggleBoosterOn(boosterList[8]);
            ToggleBoosterOn(boosterList[9]);
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            ToggleBoosterOff(boosterList[4]);
            ToggleBoosterOff(boosterList[5]);
            ToggleBoosterOff(boosterList[6]);
            ToggleBoosterOff(boosterList[7]);
            ToggleBoosterOff(boosterList[8]);
            ToggleBoosterOff(boosterList[9]);
        }

        // Down                                     // Inverted w flying
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            ToggleBoosterOn(boosterList[10]);
            ToggleBoosterOn(boosterList[11]);
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            ToggleBoosterOff(boosterList[10]);
            ToggleBoosterOff(boosterList[11]);
        }

        // Right
        if (Input.GetKeyDown(KeyCode.D))
        {
            ToggleBoosterOn(boosterList[12]);
            ToggleBoosterOn(boosterList[13]);
            ToggleBoosterOn(boosterList[14]);
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            ToggleBoosterOff(boosterList[12]);
            ToggleBoosterOff(boosterList[13]);
            ToggleBoosterOff(boosterList[14]);
        }

        // Left
        if (Input.GetKeyDown(KeyCode.A))
        {
            ToggleBoosterOn(boosterList[15]);
            ToggleBoosterOn(boosterList[16]);
            ToggleBoosterOn(boosterList[17]);
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            ToggleBoosterOff(boosterList[15]);
            ToggleBoosterOff(boosterList[16]);
            ToggleBoosterOff(boosterList[17]);
        }


    }


    private void ToggleBoosterOn(GameObject ps)
    {
        ps.GetComponent<ParticleSystem>().Play();
    }

    private void ToggleBoosterOff(GameObject ps)
    {
        ps.GetComponent<ParticleSystem>().Stop();
        
    }
}
