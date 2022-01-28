using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Hit Sound", menuName = "Game Data/Hit Sound")]
public class HitSoundData : ScriptableObject
{
    [SerializeField] private List<MaterialAudioClip> sound;

    [SerializeField] private AudioClip defaultSound;

    public AudioClip DefaultSound
    {
        get => defaultSound;
    }

    public static Dictionary<PhysicMaterial, AudioClip> Sound = new Dictionary<PhysicMaterial, AudioClip>();

    public void AddHitSoundData()
    {
        if(Sound.Count == 0)
        {
            foreach (MaterialAudioClip aud in sound)
            {
                Sound.Add(aud.mat, aud.aud);
            }
        }
    }
}

[System.Serializable]
public class MaterialAudioClip
{
    public PhysicMaterial mat;
    public AudioClip aud;
}