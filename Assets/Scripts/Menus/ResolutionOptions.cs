using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//[RequireComponent(typeof(Dropdown))]
public class ResolutionOptions : MonoBehaviour
{
   
    [SerializeField] private Dropdown resolutionDropdown;
    private Resolution[] resolutions;
    private GraphicsQuality _graphicsQuality;

    private void Awake()
    {
        //_graphicsQuality = GetComponent<GraphicsQuality>();
        //_graphicsQuality.SetGraphicsQuality(3);

        resolutions = Screen.resolutions;

        //Clears resolution dropdown options.
        resolutionDropdown.ClearOptions();

        //Creates a list of options
        List<string> options = new List<string>();
        //The current resolution in the options list
        int currentResolutionIndex = 0;

        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            string option =
                $"{Screen.resolutions[i].width} x {Screen.resolutions[i].height} : {Screen.resolutions[i].refreshRate}";
            
            //Adds the options to the list
            options.Add(option);
            
            if (Screen.resolutions[i].width == Screen.currentResolution.width && Screen.resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = 1;
            }
        }

        //Adds the options from the list to the resolution dropdown
        resolutionDropdown.AddOptions(options);
        
        
        //Sets the resolution to the resolutionindex
        resolutionDropdown.value = currentResolutionIndex;
        
        
        
        //refresh the text and image of the currently selected option if the list has been modified
        resolutionDropdown.RefreshShownValue();

    }

    private void Start()
    {
        QualitySettings.SetQualityLevel(5);
        Screen.SetResolution(1920, 1080, true, 0);
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        //Saves the currently selected resolution
        PlayerPrefs.SetInt("resolution", resolutionIndex);
        PlayerPrefs.Save();
        Debug.Log($"Current resolution {Screen.currentResolution}");
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
        if (isFullScreen == true)
        {
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
            Debug.Log($"fullscreen on");
        }
        else if(isFullScreen == false)
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
            Debug.Log($"fullscreen off");
        }
    }
}
