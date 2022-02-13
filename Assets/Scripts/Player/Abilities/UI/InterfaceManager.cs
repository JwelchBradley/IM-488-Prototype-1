/*****************************************************************************
// File Name :         InterfaceManager.cs
// Author :            Jacob Welch
// Creation Date :     21 January 2022
//
// Brief Description : Handles the movement of the player.
*****************************************************************************/
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InterfaceManager : MonoBehaviour
{
    [Header("Ability References")]
    [Tooltip("The prefab for ability UIs")]
    [SerializeField]
    private AbilityUI abilityIconPrefab;

    [Tooltip("The position of the ability prefab")]
    [SerializeField]
    private Transform[] abilityIconSlot;

    private string keybind = "123";

    [SerializeField]
    private InputActionReference ability1;

    [SerializeField]
    private InputActionReference ability2;

    [SerializeField]
    private InputActionReference ability3;

    [SerializeField]
    private RebindActionUI[] raui;

    /// <summary>
    /// Initializes the ability UI.
    /// </summary>
    void Start()
    {
        ThirdPersonController player = GameObject.Find("Player").GetComponent<ThirdPersonController>();

        string displayString1 = ability1.action.bindings[0].effectivePath;
        string displayString2 = ability2.action.bindings[0].effectivePath;
        string displayString3 = ability3.action.bindings[0].effectivePath;

        List<char> bindings = new List<char>();
        bindings.Add(displayString1[11]);
        bindings.Add(displayString2[11]);
        bindings.Add(displayString3[11]);

        for (int i = 0; i < player.Abilities.Length; i++)
        {
            AbilityUI abilityUi = Instantiate(abilityIconPrefab, abilityIconSlot[i]);
            player.Abilities[i].OnAbilityUse.AddListener(abilityUi.ShowCoolDown);
            abilityUi.SetIcon(player.Abilities[i].Ability.Icon, player.Abilities[i].Ability.Charges, player.Abilities[i].Ability.Cooldown, bindings[i]);

            raui[i].updateBindingUIEvent.AddListener(abilityUi.UpdateKeybindText);
        }
    }
}
