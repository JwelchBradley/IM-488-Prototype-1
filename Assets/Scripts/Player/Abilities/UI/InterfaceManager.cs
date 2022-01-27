/*****************************************************************************
// File Name :         InterfaceManager.cs
// Author :            Jacob Welch
// Creation Date :     21 January 2022
//
// Brief Description : Handles the movement of the player.
*****************************************************************************/
using UnityEngine;

public class InterfaceManager : MonoBehaviour
{
    [Header("Ability References")]
    [Tooltip("The prefab for ability UIs")]
    [SerializeField]
    private AbilityUI abilityIconPrefab;

    [Tooltip("The position of the ability prefab")]
    [SerializeField]
    private Transform[] abilityIconSlot;

    private string keybind = "QEX";

    /// <summary>
    /// Initializes the ability UI.
    /// </summary>
    void Start()
    {
        ThirdPersonController player = GameObject.Find("Player").GetComponent<ThirdPersonController>();

        for (int i = 0; i < player.Abilities.Length; i++)
        {
            AbilityUI abilityUi = Instantiate(abilityIconPrefab, abilityIconSlot[i]);
            player.Abilities[i].OnAbilityUse.AddListener(abilityUi.ShowCoolDown);
            abilityUi.SetIcon(player.Abilities[i].Ability.Icon, player.Abilities[i].Ability.Charges, player.Abilities[i].Ability.Cooldown, keybind[i]);
        }
    }
}
