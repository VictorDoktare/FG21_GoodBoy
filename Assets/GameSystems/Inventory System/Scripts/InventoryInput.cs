using System;
using UnityEngine;
using UnityEngine.UI;

public class InventoryInput : MonoBehaviour
{
    [SerializeField] private KeyCode inputKey;
    [SerializeField] private Button button;
    void Start()
    {
        button = GetComponent<Button>();
    }
    
    void Update()
    {
        if (Input.GetKeyDown(inputKey))
        {
            button.onClick.Invoke();
        }
    }
}
