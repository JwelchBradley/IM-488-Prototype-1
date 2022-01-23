using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ability", menuName = "Ability")]
public class Ability : ScriptableObject
{
    [Header("Visuals")]
    [Tooltip("The name of the ability (note: must be the same as the script name")]
    public new string name;

    public Sprite icon;

    [Header("Stats")]
    [Tooltip("How many charges this ability has")]
    public int charges = 1;

    [Tooltip("How fast a single charge refreshes")]
    public float cooldown = 4.0f;
}
