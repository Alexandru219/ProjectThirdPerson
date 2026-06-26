using System;
using UnityEngine;
using UnityEngine.InputSystem;



public class ToggleInputActionMap : MonoBehaviour
{
    public StartInputActionMap startInputActionMap;
    
    public InputReaderNew inputReader;
    
    private void OnEnable()
    {
        if (startInputActionMap == StartInputActionMap.GameplayMap)
        {
           inputReader.EnableGameplay();
            
        }
    }

    private void OnDisable()
    {
        InputReaderNew.Instance.DisableAll();
    }
}
