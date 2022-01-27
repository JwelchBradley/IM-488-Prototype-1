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
    public bool a2select;

    public Button gravityBttn;
    public Button shieldBttn;
    
    // Start is called before the first frame update
    void Start()
    {
        weaponPanel.SetActive(true);
        abilityPanel.SetActive(false);
        abilityNum = 0;

        gravityBttn.interactable = true;
        shieldBttn.interactable = true;

        ability1 = "";
        ability2 = "";
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
            abilityNum = 1;
        }
        else if (abilityNum == 1)
        {
            ability2 = "gravity";
            PlayerDone();
        }
    }

    public void Shield()
    {

        if (abilityNum == 0)
        {
            ability1 = "shield";
            shieldBttn.interactable = false;
            abilityNum = 1;
        }
        else if (abilityNum == 1)
        {
            ability2 = "shield";
            PlayerDone();
        }
    }

    void PlayerDone()
    {
        
        SceneManager.LoadScene("Level");
    }
}
