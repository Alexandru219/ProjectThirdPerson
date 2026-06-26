using System.Collections;
using UnityEngine;

public class LoadGame : MonoBehaviour
{
    private void Start()
    {
        //yield return null;
        SaveManager.Instance.LoadGame();
    }
    
}
