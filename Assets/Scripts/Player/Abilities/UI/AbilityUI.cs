/*****************************************************************************
// File Name :         AbilityUI.cs
// Author :            Jacob Welch
// Creation Date :     23 January 2022
//
// Brief Description : Handles the UI for abilities.
*****************************************************************************/
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class AbilityUI : MonoBehaviour
{
    [Tooltip("The icon image that will show the ability sprite")]
    [SerializeField] private Image icon = default;

    [Tooltip("The image that shows the cooldown of the ability")]
    [SerializeField] private Image coolDownImage = default;

    [Tooltip("The text showing the number of charges for this ability")]
    [SerializeField] private TextMeshProUGUI chargesText;

    [Tooltip("The text showing the keybind")]
    [SerializeField] private TextMeshProUGUI keyBindText;

    /// <summary>
    /// How fast charges of this ability recharge.
    /// </summary>
    private float cooldown;

    /// <summary>
    /// The max amount of charges for this ability.
    /// </summary>
    private int totalCharges = 1;

    /// <summary>
    /// The current number of charges.
    /// </summary>
    private int charges = 1;

    /// <summary>
    /// Holds reference to the cooldown timer.
    /// </summary>
    private Coroutine updateTextTimer;

    /// <summary>
    /// Initializes the icon and other necessary values.
    /// </summary>
    /// <param name="sprite">The sprite of the UI.</param>
    /// <param name="totalCharges">The starting number of charges for this ability.</param>
    /// /// <param name="cooldown">How fast charges recharge.</param>
    public void SetIcon(Sprite sprite, int totalCharges, float cooldown, char binding)
    {
        icon.sprite = sprite;
        this.totalCharges = totalCharges;
        charges = totalCharges;
        this.cooldown = cooldown;
        keyBindText.text = binding.ToString();
        UpdateText();
    }

    /// <summary>
    /// Updates the text for the number of charges the player has.
    /// </summary>
    private void UpdateText()
    {
        chargesText.text = charges.ToString();
    }

    /// <summary>
    /// Starts showing the abilities cooldown.
    /// </summary>
    public void ShowCoolDown()
    {
        charges--;
        chargesText.text = charges.ToString();

        if(updateTextTimer == null)
        {
            updateTextTimer = StartCoroutine(UpdateTextTimer());
        }
    }

    /// <summary>
    /// Updates the ability recharging process.
    /// </summary>
    /// <returns></returns>
    private IEnumerator UpdateTextTimer()
    {
        while(charges != totalCharges)
        {
            transform.DOComplete();
            coolDownImage.fillAmount = 0;
            coolDownImage.DOFillAmount(1, cooldown).SetEase(Ease.Linear).OnComplete(() => transform.DOPunchScale(Vector3.one / 10, .2f, 10, 1));
            yield return new WaitForSeconds(cooldown);
            charges++;
            UpdateText();
        }

        updateTextTimer = null;
    }
}
