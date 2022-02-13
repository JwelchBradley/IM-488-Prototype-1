using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GWhipBttnDescription : MonoBehaviour
{
    public Text descriptTxt;

    private bool mouse_over = false;

    void Start()
    {
        descriptTxt = GameObject.Find("descriptText").GetComponent<Text>();
    }

    void Update()
    {
        if (mouse_over)
        {
            Debug.Log("Mouse Over");
        }
    }

    public void OnPointerEnter()
    {
        mouse_over = true;
        descriptTxt.text = "Grab and throw asteroids at will";
    }

    public void OnPointerExit()
    {
        mouse_over = false;
        descriptTxt.text = "";
    }
}
