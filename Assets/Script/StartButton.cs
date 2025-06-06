using UnityEngine;
using UnityEngine.UI;         
using UnityEngine.EventSystems;

public class StartButton : MonoBehaviour
{
    void Start(){
        //Unlock Cursor and make it visible when the game starts
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void OnClickStart(){
        // Load the game scene when the button is clicked
        Canvas canvas = GameObject.Find("MainMenu").GetComponent<Canvas>();
        canvas.gameObject.SetActive(false);
    }
}
