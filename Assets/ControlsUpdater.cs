using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControlsUpdater : MonoBehaviour
{
    TextMeshProUGUI text;

    [SerializeField]
    private InputActionReference movement;

    [SerializeField]
    private InputActionReference movementVertical;

    [SerializeField]
    private InputActionReference movementFast;

    [SerializeField]
    private InputActionReference ability1;

    [SerializeField]
    private InputActionReference ability2;

    [SerializeField]
    private InputActionReference ability3;

    [SerializeField]
    bool isStartingPanel;

    // Start is called before the first frame update
    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        string movementStringUp = StringGetter(movement.action, 1);
        string movementStringLeft = StringGetter(movement.action, 3);
        string movementStringDown = StringGetter(movement.action, 2);
        string movementStringRight = StringGetter(movement.action, 4);
        string lateralMovement = movementStringUp + movementStringLeft + movementStringDown + movementStringRight;

        string movementStringVerticalUp = StringGetter(movementVertical.action, 1);
        string movementStringVerticalDown = StringGetter(movementVertical.action, 2);

        string movementStringFast = StringGetter(movementFast.action, 0);

        string abilityString1 = StringGetter(ability1.action, 0);
        string abilityString2 = StringGetter(ability2.action, 0);
        string abilityString3 = StringGetter(ability3.action, 0);

        if (!isStartingPanel)
        {
            text.text = "<u>Controls</u>" + "\n" +
    "<b>Move:</b> " + lateralMovement + "\n" +
    "<b>Move Vertically:</b> " + movementStringVerticalUp + " = down and " + movementStringVerticalDown + " = up" + "\n" +
    "<b>Fly:</b> toggle " + movementStringFast + "\n" +
    "<b>Dash:</b> double - tap " + lateralMovement + ", " + movementStringVerticalDown + ", or " + movementStringVerticalUp + "\n" +
    "<b>Aim:</b> Mouse" + "\n" +
    "<b>Shoot:</b> Left-click" + "\n" +
    "<b>ADS:</b> Right-click" + "\n" +
    "<b>Open Pause:</b> Escape" + "\n" +
    "<b>Abilities </b> " + abilityString1 + ", " + abilityString2 + ", " + abilityString3 + "\n";
        }
        else
        {
            text.text = "<u>Controls</u>" + "\n" +
    "<b>Move:</b> " + lateralMovement + "\n" +
    "<b>Move Vertically:</b> " + movementStringVerticalUp + " = down and " + movementStringVerticalDown + " = up" + "\n" +
    "<b>Fly:</b> toggle " + movementStringFast + "\n" +
    "<b>Dash:</b> double - tap " + lateralMovement + ", " + movementStringVerticalDown + ", or " + movementStringVerticalUp + "\n" +
    "<b>Aim:</b> Mouse" + "\n" +
    "<b>Shoot:</b> Left - click" + "\n" +
    "<b>ADS:</b> Right - click" + "\n" +
    "<b>Open Pause:</b> Escape" + "\n" +
    abilityString1 + " = Pull an asteroid towards you. Click to throw it" + "\n" + 
    abilityString2 + " = Freeze enemy" + "\n" + 
    abilityString3 + " = Shield" + "\n" +
    "PRESS ENTER TO START";
        }
    }

    private string StringGetter(InputAction action, int bindingIndex)
    {
        var deviceLayoutName = default(string);
        var controlPath = default(string);
        return action.GetBindingDisplayString(bindingIndex, out deviceLayoutName, out controlPath, 0);
    }
}
