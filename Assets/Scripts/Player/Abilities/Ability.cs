/*****************************************************************************
// File Name :         Ability.cs
// Author :            Jacob Welch
// Creation Date :     21 January 2022
//
// Brief Description : Holds data for abilities
*****************************************************************************/
using UnityEditor;
using UnityEngine;
using MyBox;

[CreateAssetMenu(fileName = "New Ability", menuName = "Game Data/Ability")]
public class Ability : ScriptableObject
{
    #region Visuals
    [Header("Visuals")]
    [Tooltip("The name of the ability (note: must be the same as the script name")]
    public new string name;

    [Tooltip("The icon used for the UI of abilities")]
    public Sprite icon;
    #endregion

    #region Stats
    [Header("Stats")]
    #region Usage Time
    [Tooltip("How many charges this ability has")]
    [Range(1, 3)]
    public int charges = 1;

    [Tooltip("How fast a single charge refreshes")]
    [Range(1.0f, 40.0f)]
    public float cooldown = 4.0f;

    [Tooltip("How long the ability lasts for")]
    [Range(0.0f, 50.0f)]
    public float duration = 2.0f;
    #endregion

    #region Cast values
    [Space(20)]
    [Tooltip("How long it tasks the ability to be cast")]
    [Range(0.0f, 5.0f)]
    public float castTime = 0.5f;

    [Tooltip("How long after casting before the user can take other actions")]
    [Range(0.0f, 5.0f)]
    public float uncastTime = 0.3f;

    [Tooltip("Holds true if the user can move while casting")]
    public bool movementDuringCast = false;

    [ConditionalField("movementDuringCast")]
    [Tooltip("How fast the player can move while casting")]
    public float newCastMoveSpeed = 5.0f;
    #endregion

    #region Specific
    [Header("Specific Values")]
    [Tooltip("The type of ability this is")]
    public type abilityType = type.gravity;

    public enum type
    {
        gravity,
        freeze,
        shield
    }

    #region Gravity Specific
    [ConditionalField("abilityType", type.gravity)]
    [Tooltip("How fast the player pulls something towards them")]
    public float pullSpeed;
    #endregion
    #endregion
    #endregion
}