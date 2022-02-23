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
    private KeybindInputHandler input;
    private ThirdPersonController controller;

    private Animator anim;

    private bool combatIdle;

    [Tooltip("Lists of all booster particle systems on player")]

    public List<ParticleSystem> boosterUp, boosterDown, boosterRight, boosterLeft,
                            boosterForward, boosterBack;

    private void Start()
    {
        input = transform.root.GetComponent<KeybindInputHandler>();
        controller = transform.root.GetComponent<ThirdPersonController>();

        anim = GetComponent<Animator>();

        ToggleBoostersOffAll();
    }

    private void Update()
    {
        //if(input.)

        if (controller.CurrentMoveState == ThirdPersonController.moveState.normal ||
            controller.CurrentMoveState == ThirdPersonController.moveState.ADS)
            BoostersNormalMovement();

        if (controller.CurrentMoveState == ThirdPersonController.moveState.fast)
            BoostersFlyingMovement();
    }

    private void BoostersNormalMovement()
    {
        Vector2 inputDirection = new Vector3(input.Move.x, input.Move.y).normalized;

        // Backwards movement
        if (inputDirection.y < 0)
        {
            ToggleBoostersBackOn();
            ToggleBoostersForwardOff();
        }
        // Forwards movement
        else if (inputDirection.y > 0)
        {
            ToggleBoostersForwardOn();
            ToggleBoostersBackOff();
        }
        // No forwaards/backwards movement
        else
        {
            ToggleBoostersForwardOff();
            ToggleBoostersBackOff();
        }

        // Right movement
        if (inputDirection.x > 0)
        {
            ToggleBoostersRightOn();
            ToggleBoostersLeftOff();
        }
        // Left movement
        else if (inputDirection.x < 0)
        {
            ToggleBoostersLeftOn();
            ToggleBoostersRightOff();
        }
        // No left/right movement
        else if (inputDirection.x == 0)
        {
            ToggleBoostersRightOff();
            ToggleBoostersLeftOff();
        }

        // Up movement
        if (input.MoveVertical > 0)
        {
            ToggleBoostersUpOn();
            ToggleBoostersDownOff();
        }
        // Down movement
        else if (input.MoveVertical < 0)
        {
            ToggleBoostersDownOn();
            ToggleBoostersUpOff();
        }
        // No veritcal movement
        else if (input.MoveVertical == 0)
        {
            ToggleBoostersUpOff();
            ToggleBoostersDownOff();
        }
    }

    private void BoostersFlyingMovement()
    {
        ToggleBoostersUpOn();

        ToggleBoostersDownOff();
        ToggleBoostersForwardOff();
        ToggleBoostersBackOff();

        Vector2 inputDirection = new Vector3(input.Move.x, input.Move.y).normalized;

        // Right movement
        if (inputDirection.x > 0)
        {
            ToggleBoostersRightOn();
            ToggleBoostersLeftOff();
        }
        // Left movement
        else if (inputDirection.x < 0)
        {
            ToggleBoostersLeftOn();
            ToggleBoostersRightOff();
        }
        // No left/right movement
        else if (inputDirection.x == 0)
        {
            ToggleBoostersRightOff();
            ToggleBoostersLeftOff();
        }
    }


    /**************************************************************************/
    public void ToggleBoostersOnAll()
    {
        ToggleBoostersOn(boosterUp);
        ToggleBoostersOn(boosterDown);
        ToggleBoostersOn(boosterRight);
        ToggleBoostersOn(boosterLeft);
        ToggleBoostersOn(boosterForward);
        ToggleBoostersOn(boosterBack);
    }

    public void ToggleBoostersOffAll()
    {
        ToggleBoostersOff(boosterUp);
        ToggleBoostersOff(boosterDown);
        ToggleBoostersOff(boosterRight);
        ToggleBoostersOff(boosterLeft);
        ToggleBoostersOff(boosterForward);
        ToggleBoostersOff(boosterBack);
    }

    /**************************************************************************/
    public void ToggleBoostersUpOn()
    {
        ToggleBoostersOn(boosterUp);
    }

    public void ToggleBoostersUpOff()
    {
        ToggleBoostersOff(boosterUp);
    }

    /**************************************************************************/
    public void ToggleBoostersDownOn()
    {
        ToggleBoostersOn(boosterDown);
    }

    public void ToggleBoostersDownOff()
    {
        ToggleBoostersOff(boosterDown);
    }

    /**************************************************************************/
    public void ToggleBoostersRightOn()
    {
        ToggleBoostersOn(boosterRight);
    }

    public void ToggleBoostersRightOff()
    {
        ToggleBoostersOff(boosterRight);
    }

    /**************************************************************************/
    public void ToggleBoostersLeftOn()
    {
        ToggleBoostersOn(boosterLeft);
    }

    public void ToggleBoostersLeftOff()
    {
        ToggleBoostersOff(boosterLeft);
    }

    /**************************************************************************/
    public void ToggleBoostersForwardOn()
    {
        ToggleBoostersOn(boosterForward);
    }

    public void ToggleBoostersForwardOff()
    {
        ToggleBoostersOff(boosterForward);
    }

    /**************************************************************************/
    public void ToggleBoostersBackOn()
    {
        ToggleBoostersOn(boosterBack);
    }

    public void ToggleBoostersBackOff()
    {
        ToggleBoostersOff(boosterBack);
    }

    /**************************************************************************/
    private void ToggleBoostersOn(List<ParticleSystem> psList)
    {
        for(int i = 0; i < psList.Count; i++)
        {
            if(!psList[i].isPlaying)
                psList[i].Play();
        }
    }

    private void ToggleBoostersOff(List<ParticleSystem> psList)
    {
        for (int i = 0; i < psList.Count; i++)
        {
            if(psList[i].isPlaying)
                psList[i].Stop();
        }
    }

    //public void ToggleBoosterOn(GameObject ps)
    //{
    //    ps.GetComponent<ParticleSystem>().Play();
    //}

    //private void ToggleBoosterOff(GameObject ps)
    //{
    //    ps.GetComponent<ParticleSystem>().Stop();

    //}



    //public void ToggleBoostersUp()
    //{

    //}

    // toggle 
}
