using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Behavior : MonoBehaviour
{
    bool healing;
    public Slider healthBar;
    public Text hpTxt;
    public AudioSource aSource;
    public AudioClip heal;



    // Start is called before the first frame update
    void Start()
    {
        healing = false;
        healthBar.value = 40;
        hpTxt.text = "HP: " + healthBar.value;
    }

    // Update is called once per frame
    void Update()
    {
        if(healthBar.value <= 0)
        {
            SceneManager.LoadScene("Level");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "heal" && healthBar.value < 50)
        {
            healing = true;
            aSource.clip = heal;
            aSource.Play();
            StartCoroutine(HealPlayer());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "heal")
        {
            healing = false;
        }
    }

    IEnumerator HealPlayer()
    {
        yield return new WaitForSeconds(1f);
        healthBar.value += 2;
        hpTxt.text = "HP: " + healthBar.value;
    }
}