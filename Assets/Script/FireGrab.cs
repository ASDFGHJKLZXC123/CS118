using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.InputSystem;

public class GrabFire: UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable
{
    public float fireForce = 8f;

    bool selected = false;
    IXRSelectInteractor interactor;
    public InputActionReference fireAction;
    
    void Start(){
        fireAction.action.Enable();
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args){
        base.OnSelectEntered(args);
        selected = true;
        interactor = args.interactorObject;
    }

    protected override void OnSelectExited(SelectExitEventArgs args){
        base.OnSelectExited(args);
        selected = false;
    }

    private void FireObject(){
        throwOnDetach = false;
        interactionManager.SelectExit(interactor, this);

        Rigidbody rb = GetComponent<Rigidbody>();
        rb.linearVelocity = interactor.GetAttachTransform(this).forward * fireForce;

        Debug.Log("Fired object at " + rb.linearVelocity);
    }

    private void Update(){
        if(selected){
            if(fireAction.action.IsPressed()){
                FireObject();
            }
        }
    }
}
