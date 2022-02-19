using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealStationBehavior : MonoBehaviour
{
    bool healing;
    public AudioSource aSource;
    public AudioClip heal;
    [SerializeField] private AudioClip disableSound;
    [SerializeField] private MeshRenderer barrierRender;

    [Header("Heal rate")]
    [SerializeField] private int healPerTick = 5;
    [SerializeField] private float tickRate = 0.8f;
    [SerializeField] private float timeBeforeHealStart = 1.0f;

    [Header("Heal cooldown")]
    [SerializeField] private float cooldownTime = 50.0f;
    [SerializeField] private int healAmount = 30;
    private int currentAmountHealed = 0;
    private bool disabled = false;


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !healing)
        {
            healing = true;
            StartCoroutine(HealPlayer(other.transform.parent.gameObject.GetComponent<IDamagable>()));
        }
        else if(healing && !disabled && other.gameObject.layer.Equals(LayerMask.NameToLayer("Enemy Bullet")))
        {
            other.gameObject.GetComponent<BulletController>().PlayHitSound(other.gameObject.transform.position, other.gameObject.GetComponentInChildren<Collider>().sharedMaterial);
            other.gameObject.SetActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            healing = false;
        }
    }

    IEnumerator HealPlayer(IDamagable player)
    {
        if (disabled)
            yield break;

        yield return new WaitForSeconds(timeBeforeHealStart);

        while (healing && player.HealthAmount() < 100)
        {
            yield return new WaitForSeconds(tickRate);
            aSource.clip = heal;
            aSource.Play();
            player.UpdateHealth(healPerTick);
            currentAmountHealed += healPerTick;
        }

        if(currentAmountHealed > healAmount)
        {
            DisableHealStation();
        }

        healing = false;
    }

    private void DisableHealStation()
    {
        disabled = true;
        barrierRender.enabled = false;
        aSource.PlayOneShot(disableSound);
        StartCoroutine(EnableHealStation());
    }

    private IEnumerator EnableHealStation()
    {
        yield return new WaitForSeconds(cooldownTime);
        barrierRender.enabled = true;
        disabled = false;
        currentAmountHealed = 0;
    }
}