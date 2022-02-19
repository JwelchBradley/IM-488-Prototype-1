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
        set
        {
            ability = value;
        }
    }

    /// <summary>
    /// An event call for when this ability is used.
    /// </summary>
    public class MyFloatEvent : UnityEvent { }

    /// <summary>
    /// An instance for the MyFloatEvent call.
    /// </summary>
    public MyFloatEvent OnAbilityUse = new MyFloatEvent();

    public class MyBoolEvent : UnityEvent<bool> { }

    public MyBoolEvent OnAbilityHighlight = new MyBoolEvent();

    /// <summary>
    /// The amount of charges the player currently has available for this ability.
    /// </summary>
    private int currentCharges;

    /// <summary>
    /// Is true if there is an ability able to be used.
    /// </summary>
    protected bool canUse = true;

    /// <summary>
    /// Holds true if the user is currently casting.
    /// </summary>
    protected bool isCasting = false;

    /// <summary>
    /// Holds reference to the cooldown timer.
    /// </summary>
    private Coroutine cooldownTimerReference;

    protected ThirdPersonController tpc;

    [SerializeField]
    protected bool isManager = false;
    #endregion

    #region Functions
    /// <summary>
    /// Initializes the current number of charges.
    /// </summary>
    protected virtual void Start()
    {
        tpc = GetComponent<ThirdPersonController>();
        ability.mainCam = Camera.main;
        currentCharges = ability.Charges;
    }

    /// <summary>
    /// Attempts to use this ability.
    /// </summary>
    public bool TriggerAbility(ref Ability ability)
    {
        if (canUse)
        {
            if (AbilityActivate())
            {
                ability = this.ability;
                isCasting = true;

                if (--currentCharges == 0)
                {
                    canUse = false;
                }

                if (ability.StartCooldownOnCast)
                    StartCooldown();
                else
                    HighlightAbility();

                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Handles the usage of this ability.
    /// </summary>
    protected abstract bool AbilityActivate();

    private void HighlightAbility()
    {
        OnAbilityHighlight.Invoke(true);
    }

    /// <summary>
    /// Starts the cooldown process for this ability.
    /// </summary>
    public void StartCooldown()
    {
        OnAbilityUse.Invoke();
        OnAbilityHighlight.Invoke(false);

        // If there isn't already a time on this ability, create one
        if (cooldownTimerReference == null)
            cooldownTimerReference = StartCoroutine(Cooldown());

        // Refreshes a charge until max charges are gained.
        IEnumerator Cooldown()
        {
            yield return new WaitForSeconds(ability.Cooldown);

            currentCharges++;
            canUse = true;

            while(currentCharges != ability.Charges)
            {
                yield return new WaitForSeconds(ability.Cooldown);
                currentCharges++;
                canUse = true;
            }

            cooldownTimerReference = null;
        }
    }
    #endregion
}
