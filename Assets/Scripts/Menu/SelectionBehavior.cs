/*****************************************************************************
// File Name :         GravityPull.cs
// Author :            Jessica Barthelt
// Creation Date :     23 January 2022
//
// Brief Description : Determines the abilities that the player
                       chooses from at the start of the game
*****************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectionBehavior : MonoBehaviour
{
    public GameObject weaponPanel;
    public GameObject abilityPanel;

    public int abilityNum;

    public string ability1;
    public string ability2;
    public string ability3;
    public bool a2select;

    public Button gravityBttn;
    public Button empBttn;
    public Button shieldBttn;
    
    // Start is called before the first frame update
    void Start()
    {
        weaponPanel.SetActive(true);
        abilityPanel.SetActive(false);
        abilityNum = 0;

        gravityBttn.interactable = true;
        shieldBttn.interactable = true;

        ability1 = "gravity";
        ability2 = "shield";
        ability3 = "EMP";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FullAutoSelect()
    {
        weaponPanel.SetActive(false);
        abilityPanel.SetActive(true);
    }

    /*
    public void SnipeSelect()
    {
        weaponPanel.SetActive(false);
        abilityPanel.SetActive(true);
    }

    public void SemiAutoSelect()
    {
        weaponPanel.SetActive(false);
        abilityPanel.SetActive(true);
    }
    */

    public void Gravity()
    {
        if(abilityNum == 0)
        {
            ability1 = "gravity";
            gravityBttn.interactable = false;
            print("ability 1 = gravity");
            abilityNum = 1;
            
        }
        else if (abilityNum == 1)
        {
            ability2 = "gravity";
            print("ability 2 = gravity");
            PlayerDone();
        }
    }

    public void Shield()
    {

        if (abilityNum == 0)
        {
            ability1 = "shield";
            shieldBttn.interactable = false;
            print("ability 1 = shield");
            abilityNum = 1;
        }
        else if (abilityNum == 1)
        {
            ability2 = "shield";
            print("ability 2 = shield");
            PlayerDone();
        }
    }

    public void EMP()
    {

        if (abilityNum == 0)
        {
            ability1 = "EMP";
            empBttn.interactable = false;
            print("ability 1 = EMP");
            abilityNum = 1;
        }
        else if (abilityNum == 1)
        {
            ability2 = "EMP";
            print("ability 2 = EMP");
            PlayerDone();
        }
    }

    void PlayerDone()
    {
        DontDestroyOnLoad(this.gameObject);
        SceneManager.LoadScene("Level");
    }
}
