using UnityEngine;

public class QuitButton : MonoBehaviour
{
    public void OnQuitClose(){
        Application.Quit();

        // If we are in the editor, stop playing
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
