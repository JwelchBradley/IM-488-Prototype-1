using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Hit Sound", menuName = "Game Data/Hit Sound")]
public class HitSoundData : ScriptableObject
{
    public List<MaterialAudioClip> sound;
}

[System.Serializable]
public class MaterialAudioClip
{
    public Material mat;
    public AudioClip aud;
}