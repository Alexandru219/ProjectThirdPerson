using UnityEngine;

public class GameExit : MonoBehaviour
{
    // Această metodă poate fi apelată de la un buton de UI sau din cod
    public void QuitGame()
    {
        Debug.Log("[GameExit] Ieșire din joc inițiată...");

#if UNITY_EDITOR
        // Această linie rulează DOAR când ești în Unity Editor
        UnityEditor.EditorApplication.isPlaying = false;
        
#else
        // Această linie rulează DOAR în jocul final (Build)
        Application.Quit();
#endif
    }

    // Exemplu opțional: Ieșire la apăsarea tastei Escape
  
}