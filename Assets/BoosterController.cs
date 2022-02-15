using System.Collections.Generic;
using UnityEngine;

public class BoosterController : MonoBehaviour
{
    public List<ParticleSystem> BoosterList;

    public void ToggleBoosterOn(ParticleSystem ps)
    {
        ps.GetComponent<ParticleSystem>().Play();
    }

    public void ToggleBoosterOff(ParticleSystem ps)
    {
        ps.GetComponent<ParticleSystem>().Stop();
    }
}
