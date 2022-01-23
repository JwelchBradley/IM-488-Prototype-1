using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceManager : MonoBehaviour
{
    public CanvasGroup retical;

    [Header("Ability References")]
    public AbilityUI abilityIconPrefab;
    public Transform[] abilityIconSlot;

    void Start()
    {
        ThirdPersonController player = GameObject.Find("Player").GetComponent<ThirdPersonController>();

        for (int i = 0; i < player.Abilities.Length; i++)
        {
            AbilityUI abilityUi = Instantiate(abilityIconPrefab, abilityIconSlot[i]);
            player.Abilities[i].OnAbilityUse.AddListener((cooldownTime) => abilityUi.ShowCoolDown(cooldownTime));
            abilityUi.SetIcon(player.Abilities[i].Ability.icon);
        }
    }
}
