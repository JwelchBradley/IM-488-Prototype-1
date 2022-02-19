using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RebindingEnabled : MonoBehaviour
{
    private MenuBehavior mb;

    private void Awake()
    {
        mb = transform.root.gameObject.GetComponentInChildren<MenuBehavior>();
    }

    private void OnEnable()
    {
        MenuBehavior.isDisable = false;
        mb.StopCoroutines();
    }

    private void OnDisable()
    {
        mb.UpdateRebind();
    }
}
