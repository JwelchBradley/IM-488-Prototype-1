using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetHitEffect : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(Reset());
    }

    private IEnumerator Reset()
    {
        yield return new WaitForSeconds(3f);
        gameObject.SetActive(false);
    }
}
