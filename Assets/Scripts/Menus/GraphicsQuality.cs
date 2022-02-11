using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphicsQuality : MonoBehaviour
{
    public void SetGraphicsQuality(int GraphicsQuality)
    {
        QualitySettings.SetQualityLevel(GraphicsQuality);
        Debug.Log($"Graphics quality {GraphicsQuality}");
    }
}
