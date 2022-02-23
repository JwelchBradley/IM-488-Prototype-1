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

    private AudioSource boosterSFX;

    [Tooltip("Lists of all booster particle systems on player")]

    public List<ParticleSystem> boosterUp, boosterDown, boosterRight, boosterLeft,
                            boosterForward, boosterBack, boosterDash;

    private void Start()
    {
        input = transform.root.GetComponent<KeybindInputHandler>();
        controller = transform.root.GetComponent<ThirdPersonController>();

        anim = GetComponent<Animator>();
        boosterSFX = GetComponent<AudioSource>();

        ToggleBoostersOffAll();
        ToggleBoostersOff(boosterDash);
    }

    private void Update()
    {
        if (controller.CurrentMoveState == ThirdPersonController.moveState.normal ||
            controller.CurrentMoveState == ThirdPersonController.moveState.ADS)
            BoostersNormalMovement();

        else if (controller.CurrentMoveState == ThirdPersonController.moveState.fast)
            BoostersFlyingMovement();

        else
            ToggleBoostersOffAll();

        BoosterSFX();
    }

    private void BoostersNormalMovement()
    {
        Vector2 inputDirection = new Vector3(input.Move.x, input.Move.y).normalized;

        // Backwards movement
        if (inputDirection.y < 0)
        {
            ToggleBoostersOn(boosterBack);
            ToggleBoostersOff(boosterForward);
        }
        // Forwards movement
        else if (inputDirection.y > 0)
        {
            ToggleBoostersOn(boosterForward);
            ToggleBoostersOff(boosterBack);
        }
        // No forwaards/backwards movement
        else
        {
            ToggleBoostersOff(boosterForward);
            ToggleBoostersOff(boosterBack);
        }

        // Right movement
        if (inputDirection.x > 0)
        {
            ToggleBoostersOn(boosterRight);
            ToggleBoostersOff(boosterLeft);
        }
        // Left movement
        else if (inputDirection.x < 0)
        {
            ToggleBoostersOn(boosterLeft);
            ToggleBoostersOff(boosterRight);
        }
        // No left/right movement
        else if (inputDirection.x == 0)
        {
            ToggleBoostersOff(boosterRight);
            ToggleBoostersOff(boosterLeft);
        }

        // Up movement
        if (input.MoveVertical > 0)
        {
            ToggleBoostersOn(boosterUp);
            ToggleBoostersOff(boosterDown);
        }
        // Down movement
        else if (input.MoveVertical < 0)
        {
            ToggleBoostersOn(boosterDown);
            ToggleBoostersOff(boosterUp);
        }
        // No veritcal movement
        else if (input.MoveVertical == 0)
        {
            ToggleBoostersOff(boosterUp);
            ToggleBoostersOff(boosterDown);
        }
    }

    private void BoostersFlyingMovement()
    {
        ToggleBoostersOn(boosterUp);

        ToggleBoostersOff(boosterDown);
        ToggleBoostersOff(boosterForward);
        ToggleBoostersOff(boosterBack);

        Vector2 inputDirection = new Vector3(input.Move.x, input.Move.y).normalized;

        // Right movement
        if (inputDirection.x > 0)
        {
            ToggleBoostersOn(boosterRight);
            ToggleBoostersOff(boosterLeft);
        }
        // Left movement
        else if (inputDirection.x < 0)
        {
            ToggleBoostersOn(boosterLeft);
            ToggleBoostersOff(boosterRight);
        }
        // No left/right movement
        else if (inputDirection.x == 0)
        {
            ToggleBoostersOff(boosterRight);
            ToggleBoostersOff(boosterLeft);
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

    public void ToggleDashBack()
    {
        ToggleDashBooster(boosterDash[0]);
        ToggleDashBooster(boosterDash[1]);
    }

    public void ToggleDashDown()
    {
        ToggleDashBooster(boosterDash[2]);
        ToggleDashBooster(boosterDash[3]);
    }

    public void ToggleDashForward()
    {
        ToggleDashBooster(boosterDash[4]);
        ToggleDashBooster(boosterDash[5]);
    }

    public void ToggleDashLeft()
    {
        ToggleDashBooster(boosterDash[6]);
        ToggleDashBooster(boosterDash[10]);
    }

    public void ToggleDashRight()
    {
        ToggleDashBooster(boosterDash[7]);
        ToggleDashBooster(boosterDash[11]);
    }

    public void ToggleDashUp()
    {
        ToggleDashBooster(boosterDash[8]);
        ToggleDashBooster(boosterDash[9]);
    }

    private void ToggleDashBooster(ParticleSystem ps)
    {
        if (ps.isPlaying)
            ps.Stop();
        else
            ps.Play();
    }

    private void BoosterSFX()
    {
        Vector2 inputDirection = new Vector3(input.Move.x, input.Move.y).normalized;

        if (input.Move.x == 0 && input.Move.y == 0 && input.MoveVertical == 0 &&
            (controller.CurrentMoveState == ThirdPersonController.moveState.normal ||
            controller.CurrentMoveState == ThirdPersonController.moveState.ADS))
            boosterSFX.Pause();
        else if(!boosterSFX.isPlaying)
            boosterSFX.Play();
    }
}
