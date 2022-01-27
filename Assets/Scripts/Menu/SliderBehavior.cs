/*****************************************************************************
// File Name :         SliderBehavior.cs
// Author :            Jacob Welch
// Creation Date :     15 June 2021
//
// Brief Description : Handles the sliders in the opitons menu.
*****************************************************************************/
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SliderBehavior : MonoBehaviour
{
    #region Variables
    #region Defaults
    [Header("Defaults")]
    [SerializeField]
    [Tooltip("The variable name used by player prefs")]
    private string variableName;

    [SerializeField]
    [Tooltip("This slider object")]
    private Slider slider;

    public Slider M_Slider
    {
        get => slider;
    }

    [SerializeField]
    [Tooltip("The input field for this slider")]
    private TMP_InputField inputField;
    #endregion

    #region Sensitivity
    [Header("Sensitivity")]
    [SerializeField]
    [Tooltip("True if this is for horizontal sensitivity")]
    private bool isSens;

    private ThirdPersonController tpc;
    #endregion

    #region Volume
    [Header("Volume")]
    [SerializeField]
    [Tooltip("Set true if this slider controls volume")]
    private bool isVolume;

    [SerializeField]
    [Tooltip("The audio mixer that is to be modified")]
    private AudioMixer audioMixer;
    #endregion
    #endregion

    #region Initialize
    /// <summary>
    /// Before the first frame is sets the slider or creates a playerpref.
    /// </summary>
    void Awake()
    {
        InitializePlayerPrefs();

        if (isVolume)
        {
            SetVolume(PlayerPrefs.GetFloat(variableName));
        }        
        else if(isSens)
        {
            GameObject player = GameObject.Find("Player");

            if(player != null)
            {
                tpc = player.GetComponent<ThirdPersonController>();
            }
        }
    }

    private void InitializePlayerPrefs()
    {
        if (PlayerPrefs.HasKey(variableName))
        {
            slider.value = PlayerPrefs.GetFloat(variableName);

            if (isSens)
            {
                slider.value *= 100;
            }

            SetInputField();
        }
        else
        {
            if (isVolume)
            {
                PlayerPrefs.SetFloat(variableName, 1);
            }
            else if (isSens)
            {
                switch (variableName)
                {
                    case "X Sens":
                            PlayerPrefs.SetFloat(variableName, 0.01f);
                            break;
                    case "Y Sens":
                            PlayerPrefs.SetFloat(variableName, 0.01f);
                            break;
                    case "X Sens Fast":
                        PlayerPrefs.SetFloat(variableName, 0.001f);
                        break;
                    case "Y Sens Fast":
                        PlayerPrefs.SetFloat(variableName, 0.001f);
                        break;
                    case "X Sens ADS":
                        PlayerPrefs.SetFloat(variableName, 0.005f);
                        break;
                    case "Y Sens ADS":
                        PlayerPrefs.SetFloat(variableName, 0.005f);
                        break;
                }
            }

            SetSetting(PlayerPrefs.GetFloat(variableName));
        }
    }
    #endregion

    public void SetSetting(float sliderValue)
    {
        if (isVolume)
        {
            SetVolume(sliderValue);
        }
        else if (isSens)
        {
            SetSensitivity(sliderValue);
        }
    }

    private void SetSensitivity(float sliderValue)
    {
        PlayerPrefs.SetFloat(variableName, sliderValue/100);

        SetInputField();

        if(tpc != null)
        {
            switch(variableName)
            {
                case "X Sens":
                    tpc.NormalCamPOV.m_HorizontalAxis.m_MaxSpeed = sliderValue/100;
                    break;
                case "Y Sens":
                    tpc.NormalCamPOV.m_VerticalAxis.m_MaxSpeed = sliderValue/100;
                    break;
                case "X Sens Fast":
                    tpc.FastCamPOV.m_HorizontalAxis.m_MaxSpeed = sliderValue/100;
                    break;
                case "Y Sens Fast":
                    tpc.FastCamPOV.m_VerticalAxis.m_MaxSpeed = sliderValue/100;
                    break;
                case "X Sens ADS":
                    tpc.AdsCamPOV.m_HorizontalAxis.m_MaxSpeed = sliderValue/100;
                    break;
                case "Y Sens ADS":
                    Debug.Log(sliderValue);
                    tpc.AdsCamPOV.m_VerticalAxis.m_MaxSpeed = sliderValue/100;
                    break;
            }
        }
    }

    /// <summary>
    /// Sets the volume of of the audiomixer and playerpref.
    /// </summary>
    /// <param name="sliderValue">The value from the slider.</param>
    private void SetVolume(float sliderValue)
    {
        // Converts linear slider value to exponential Audio Group value
        float vol = Mathf.Log10(sliderValue) * 20;

        audioMixer.SetFloat(variableName, vol);

        // Saves player audio adjustment
        PlayerPrefs.SetFloat(variableName, slider.value);

        SetInputField();
    }

    private void SetInputField()
    {
        if(inputField != null)
        {
            float value = PlayerPrefs.GetFloat(variableName);

            if (isSens)
            {
                value *= 100;
            }

            if (value == 0.001f)
            {
                value = 0;
            }

            value *= 100;
            value = (int) value;
            value /= 100;

            inputField.text = value.ToString();
        }
    }
}
