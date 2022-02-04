using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Behavior : MonoBehaviour
{
    bool healing;
    public AudioSource aSource;
    public AudioClip heal;
    [SerializeField] private int healAmount;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !healing)
        {
            healing = true;
            StartCoroutine(HealPlayer(other.transform.parent.gameObject.GetComponent<IDamagable>()));
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
        while (healing && player.HealthAmount() < 100)
        {
            yield return new WaitForSeconds(1f);
            aSource.clip = heal;
            aSource.Play();
            player.UpdateHealth(healAmount);
        }

        healing = false;
    }
}