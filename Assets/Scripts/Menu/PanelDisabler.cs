/*****************************************************************************
// File Name :         PanelDisabler.cs
// Author :            Jacob Welch
// Creation Date :     28 August 2021
//
// Brief Description : Disables this panel if the player presses escape.
*****************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.UI;

public class PanelDisabler : MonoBehaviour
    {
        PauseMenuBehavior pauseMenu;
    [SerializeField] private bool shouldImmmediate = false;

        private void Awake()
        {
            GameObject pause = GameObject.Find("Pause Menu Templates Canvas");

            if (pause != null)
            {
                pauseMenu = pause.GetComponent<PauseMenuBehavior>();
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && (MenuBehavior.isDisable || shouldImmmediate))
            {
                if (pauseMenu != null)
                {
                    pauseMenu.CanClosePauseMenu(true);
                }

                gameObject.SetActive(false);
            }
        }
    }
