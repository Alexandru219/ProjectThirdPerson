using UnityEngine;

public class GameExit : MonoBehaviour
{
    public void QuitGame()
    {
    

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        
#else
        // Această linie rulează DOAR în jocul final (Build)
        Application.Quit();
#endif
    }

  
}