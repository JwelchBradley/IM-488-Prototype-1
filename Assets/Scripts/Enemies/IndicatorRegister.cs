using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorRegister : MonoBehaviour
{
    [SerializeField] float destroyTimer = 20f;
    
    
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Register", Random.Range(0, 8));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Register()
    {
        if(!Indicate.CheckIfObjectInSight(this.transform))
        {
            Indicate.CreateIndicator(this.transform);
        }
        Destroy(this.gameObject, destroyTimer);
    }
}
