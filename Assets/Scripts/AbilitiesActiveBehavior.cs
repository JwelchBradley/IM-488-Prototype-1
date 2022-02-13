using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitiesActiveBehavior : MonoBehaviour
{
    
    SelectionBehavior sb;
    GameObject aManager;
    GameObject selectControl;
    
    // Start is called before the first frame update
    void Start()
    {
        selectControl = GameObject.Find("SelectControl");
        sb = selectControl.GetComponent<SelectionBehavior>();
        aManager = GameObject.Find("Ability Manager");

        if(sb.ability1 != "gravity" && sb.ability2 != "gravity")
        {
            aManager.GetComponent<GravityPull>().enabled = false;
        }
        if (sb.ability1 != "EMP" && sb.ability2 != "EMP")
        {
            aManager.GetComponent<FreezeEnemy>().enabled = false;
        }
        if (sb.ability1 != "shield" && sb.ability2 != "shield")
        {
            aManager.GetComponent<Shield>().enabled = false;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
