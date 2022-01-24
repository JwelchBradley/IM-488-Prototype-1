/*****************************************************************************
// File Name :         AbilityAction.cs
// Author :            Jacob Welch
// Creation Date :     23 January 2022
//
// Brief Description : Abstract class for ability actions.
*****************************************************************************/
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public abstract class AbilityAction : MonoBehaviour
{
    #region Variables
    [Tooltip("The stats for this ability")]
    [SerializeField]
    protected Ability ability;

    /// <summary>
    /// Allows other classes to access the data of this ability.
    /// </summary>
    public Ability Ability
    {
        get => ability;
    }

    /// <summary>
    /// An event call for when this ability is used.
    /// </summary>
    public class MyFloatEvent : UnityEvent { }

    /// <summary>
    /// An instance for the MyFloatEvent call.
    /// </summary>
    public MyFloatEvent OnAbilityUse = new MyFloatEvent();

    /// <summary>
    /// The amount of charges the player currently has available for this ability.
    /// </summary>
    private int currentCharges;

    /// <summary>
    /// Is true if there is an ability able to be used.
    /// </summary>
    private bool canUse = true;

    /// <summary>
    /// Holds reference to the cooldown timer.
    /// </summary>
    private Coroutine cooldownTimerReference;
    #endregion

    #region Functions
    /// <summary>
    /// Initializes the current number of charges.
    /// </summary>
    protected virtual void Start()
    {
        currentCharges = ability.charges;
    }

    /// <summary>
    /// Attempts to use this ability.
    /// </summary>
    public void TriggerAbility()
    {
        if (canUse)
        {
            if (--currentCharges == 0)
            {
                canUse = false;
            }

            if (AbilityActivate())
            {
                OnAbilityUse.Invoke();
                StartCooldown();
            }
        }
    }

    /// <summary>
    /// Handles the usage of this ability.
    /// </summary>
    protected abstract bool AbilityActivate();

    /// <summary>
    /// Starts the cooldown process for this ability.
    /// </summary>
    private void StartCooldown()
    {
        // If there isn't already a time on this ability, create one
        if(cooldownTimerReference == null)
            cooldownTimerReference = StartCoroutine(Cooldown());

        // Refreshes a charge until max charges are gained.
        IEnumerator Cooldown()
        {
            yield return new WaitForSeconds(ability.cooldown);

            currentCharges++;
            canUse = true;

            while(currentCharges != ability.charges)
            {
                yield return new WaitForSeconds(ability.cooldown);
                currentCharges++;
                canUse = true;
            }

            cooldownTimerReference = null;
        }
    }
    #endregion
}
