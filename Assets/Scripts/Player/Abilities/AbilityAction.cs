using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class AbilityAction : MonoBehaviour
{
    [SerializeField]
    protected Ability ability;

    public Ability Ability
    {
        get => ability;
    }

    public class MyFloatEvent : UnityEvent<float> { }

    public MyFloatEvent OnAbilityUse = new MyFloatEvent();

    private int currentCharges;

    /// <summary>
    /// Is true if there is an ability able to be used.
    /// </summary>
    private bool canUse = true;

    protected virtual void Start()
    {
        currentCharges = ability.charges;
    }

    public void TriggerAbility()
    {
        if (canUse)
        {
            OnAbilityUse.Invoke(ability.cooldown);
            AbilityActivate();
            StartCooldown();
        }
    }

    protected abstract void AbilityActivate();

    private void StartCooldown()
    {
        if(currentCharges == ability.charges)
        {
            StartCoroutine(Cooldown());
        }
        IEnumerator Cooldown()
        {
            if(--currentCharges == 0)
            {
                canUse = false;
            }

            yield return new WaitForSeconds(ability.cooldown);
            currentCharges++;
            canUse = true;
        }
    }
}
