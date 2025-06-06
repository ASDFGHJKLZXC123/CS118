using UnityEngine;
using UnityEngine.UI;         
using UnityEngine.EventSystems;

public class ResumeButton : MonoBehaviour
{   
    public GameObject resumeMenu;

    public void onClickResume(){
        resumeMenu.SetActive(false);
    }
}
