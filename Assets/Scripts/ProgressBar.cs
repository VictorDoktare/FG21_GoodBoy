using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBar : MonoBehaviour
{
    private Material progressBarMaterial;
    private static readonly int Progress = Shader.PropertyToID("_Progress");
    // Start is called before the first frame update
    void Start()
    {
        progressBarMaterial = GetComponent<MeshRenderer>().material;
    }


    private void OnProgressChange(float progress)
    {
        progressBarMaterial.SetFloat(Progress, progress);
    }
}
