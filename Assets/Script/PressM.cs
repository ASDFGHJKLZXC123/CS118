using UnityEngine;
using UnityEngine.InputSystem;

public class PressM : MonoBehaviour
{
    public InputActionReference menuInputActionReference;
    public GameObject mainMenuObject;

    void Start(){
        mainMenuObject.SetActive(false);
    }

    private void OnEnable()
    {
        menuInputActionReference.action.Enable();
        menuInputActionReference.action.started += MenuPressed;
    }

    private void OnDisable()
    {
        menuInputActionReference.action.started -= MenuPressed;
        menuInputActionReference.action.Disable();

    }

    private void MenuPressed(InputAction.CallbackContext context)
    {
        mainMenuObject.SetActive(true);
    }
}
