/*****************************************************************************
// File Name :         PauseMenuBehavior.cs
// Author :            Jacob Welch
// Creation Date :     28 August 2021
//
// Brief Description : Handles the pause menu and allows players to pause the game.
*****************************************************************************/
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using Cinemachine;

public class PauseMenuBehavior : MenuBehavior
{
    #region Variables
    /// <summary>
    /// Holds true if the game is currently paused.
    /// </summary>
    private bool isPaused = false;

    /// <summary>
    /// Enables and disables the pause feature.
    /// </summary>
    private bool canPause = false;

    private bool canClosePauseMenu = true;

    public GameObject rekeybindOverlay;

    /// <summary>
    /// 
    /// </summary>
    public bool CanPause
    {
        get => canPause;
        set
        {
            canPause = value;
        }
    }

    [Space]
    [SerializeField]
    [Tooltip("The panels that can be activated in the pause menu")]
    private List<GameObject> menuPanels = new List<GameObject>();

    [Space]
    [SerializeField]
    [Tooltip("The pause menu gameobject")]
    private GameObject pauseMenu = null;

    //private CinemachineVirtualCamera vCam;
    private CinemachineVirtualCamera[] vCams;
    #endregion

    #region Functions
    /// <summary>
    /// Initializes components.
    /// </summary>
    private void Awake()
    {
        //vCam = GameObject.Find("PlayerFollowCamera").GetComponent<CinemachineVirtualCamera>(); ;
        vCams = GameObject.FindObjectsOfType<CinemachineVirtualCamera>();
        StartCoroutine(WaitFadeIn());
    }

    private IEnumerator WaitFadeIn()
    {
        yield return new WaitForSeconds(crossfadeAnim.GetCurrentAnimatorStateInfo(0).length);

        canPause = true;
    }

    bool wasActiveBefore = false;
    /// <summary>
    /// If the player hits the pause game key, the game is paused.
    /// </summary>
    public void PauseGame()
    {
        // Opens pause menu and pauses the game
        if (canPause && canClosePauseMenu)
        {
            PauseAim();
            isPaused = !isPaused;
            pauseMenu.SetActive(isPaused);
            AudioListener.pause = isPaused;
            Time.timeScale = Convert.ToInt32(!isPaused);

            if (isPaused)
            {
                if(Cursor.lockState == CursorLockMode.Confined)
                {
                    wasActiveBefore = true;
                }
                else
                {
                    wasActiveBefore = false;
                }
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
            }
            else if(!wasActiveBefore)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }

    private CinemachineVirtualCamera currentCam;

    private void PauseAim()
    {
        if(currentCam == null)
        foreach(CinemachineVirtualCamera vcam in vCams)
        {
                if (vcam.enabled) 
                {
                    vcam.enabled = false;
                    currentCam = vcam;
                    break;
                }
        }

        else
        {
            currentCam.enabled = true;
            currentCam = null;
        }
    }

    public void CanClosePauseMenu(bool canClose)
    {
        canClosePauseMenu = canClose;
    }

    /// <summary>
    /// Restarts the current level from the beginning.
    /// </summary>
    public void RestartLevel()
    {
        canPause = false;

        if (!hasLoadScreen)
        {
            StartCoroutine(LoadSceneHelper(SceneManager.GetActiveScene().name));
        }
    }
    #endregion
}