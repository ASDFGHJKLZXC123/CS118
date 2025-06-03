using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[AddComponentMenu("Nokobot/Modern Guns/Simple Shoot (XR)")]
public class SimpleShootXR : MonoBehaviour
{
    [Header("Prefab References")]
    public GameObject bulletPrefab;
    public GameObject casingPrefab;
    public GameObject muzzleFlashPrefab;

    [Header("Location References")]
    [SerializeField] private Animator gunAnimator;
    [SerializeField] private Transform barrelLocation;
    [SerializeField] private Transform casingExitLocation;

    [Header("Settings")]
    [Tooltip("Time (in seconds) before destroying muzzle flash or casing")]
    [SerializeField] private float destroyTimer = 2f;
    [Tooltip("Force applied to the bullet")]
    [SerializeField] private float shotPower = 500f;
    [Tooltip("Ejection force for the casing")]
    [SerializeField] private float ejectPower = 150f;

    // Reference to the XRGrabInteractable on this same GameObject (or a parent)
    private XRGrabInteractable xrGrab;

    void Awake()
    {
        // Cache references; fall back if not assigned in Inspector
        if (barrelLocation == null) barrelLocation = transform;
        if (gunAnimator == null) gunAnimator = GetComponentInChildren<Animator>();

        // Try to get an XRGrabInteractable on this object (or parent)
        xrGrab = GetComponent<XRGrabInteractable>();
        if (xrGrab == null)
            Debug.LogWarning("SimpleShootXR: No XRGrabInteractable found on this GameObject!");
    }

    void OnEnable()
    {
        if (xrGrab != null)
        {
            // Subscribe to onActivate
            xrGrab.activated.AddListener(OnActivate);
            // (Optional) If you want to respond when the user stops pressing the 'activate' button:
            xrGrab.deactivated.AddListener(OnDeactivate);
        }
    }

    void OnDisable()
    {
        if (xrGrab != null)
        {
            xrGrab.activated.RemoveListener(OnActivate);
            xrGrab.deactivated.RemoveListener(OnDeactivate);
        }
    }

    // Called when the user presses the "Activate" action on the XR controller while holding this object.
    private void OnActivate(ActivateEventArgs args)
    {
        // Trigger the same "Fire" Ani judger you previously used in Update()
        if (gunAnimator != null)
            gunAnimator.SetTrigger("Fire");
    }

    // (Optional) Called when the user releases the "Activate" button
    private void OnDeactivate(DeactivateEventArgs args)
    {
        // You could stop automatic fire here, or do nothing if it's single‐shot.
        // For example:
        // Debug.Log("Activate button released");
    }

    // --- These two methods are invoked via animation events in your Animator:
    //  (1) At the exact frame your Fire animation should spawn the bullet/muzzle flash
    //  (2) At the frame it should eject the casing.
    //  Make sure your "Fire" animation clip has these named events wired up!
    //
    //  In other words, your Animator's Fire clip should call 
    //    1) Shoot() 
    //    2) CasingRelease()
    //
    //  just as it did before.
    //

    /// <summary>
    /// Called by an Animation Event (on your Fire animation clip) to spawn muzzle flash + bullet.
    /// </summary>
    public void Shoot()
    {
        // Spawn muzzle flash
        if (muzzleFlashPrefab && barrelLocation != null)
        {
            GameObject tempFlash = Instantiate(muzzleFlashPrefab, barrelLocation.position, barrelLocation.rotation);
            Destroy(tempFlash, destroyTimer);
        }

        // Spawn & propel bullet
        if (bulletPrefab && barrelLocation != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, barrelLocation.position, barrelLocation.rotation);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
                rb.AddForce(barrelLocation.forward * shotPower);
        }
    }

    /// <summary>
    /// Called by an Animation Event (on your Fire animation clip) to spawn and eject the casing.
    /// </summary>
    public void CasingRelease()
    {
        if (!casingExitLocation || !casingPrefab) 
            return;

        GameObject tempCasing = Instantiate(casingPrefab, casingExitLocation.position, casingExitLocation.rotation);
        Rigidbody rb = tempCasing.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Push it out
            Vector3 explosionDir = (casingExitLocation.position 
                                    - casingExitLocation.right * 0.3f 
                                    - casingExitLocation.up * 0.6f);
            rb.AddExplosionForce(Random.Range(ejectPower * 0.7f, ejectPower), explosionDir, 1f);
            // Spin it randomly
            rb.AddTorque(new Vector3(
                0f, 
                Random.Range(100f, 500f), 
                Random.Range(100f, 1000f)
            ), ForceMode.Impulse);
        }
        Destroy(tempCasing, destroyTimer);
    }
}
