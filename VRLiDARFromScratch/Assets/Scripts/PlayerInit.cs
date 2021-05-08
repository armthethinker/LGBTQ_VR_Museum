using UnityEngine;


// This is the script which will either enable gaze movement or controllers.
public class PlayerInit : MonoBehaviour
{
    [SerializeField] private OVRPlayerController ovrPlayerController;
    [SerializeField] private GameObject fpsController;
    [SerializeField] private GameObject locomotion;
    [SerializeField] private CharacterController charController;
    public bool Freemovement;
    private bool oculusPresent = false;

    void CheckOculusPresence()
    {
        oculusPresent = OVRManager.isHmdPresent;
    }

    void Awake()
    {
        CheckOculusPresence();
        OVRManager.HMDAcquired += CheckOculusPresence;
        OVRManager.HMDLost += CheckOculusPresence;

        if (!oculusPresent)
        {
            fpsController.SetActive(true);
            ovrPlayerController.gameObject.SetActive(false);
            return;
        }

        //if oculus present
        fpsController.SetActive(false);
        ovrPlayerController.gameObject.SetActive(true);

        // teleportation not enabled
        if (Freemovement)
        {
            ovrPlayerController.enabled = true;
            charController.enabled = true;
            locomotion.SetActive(false);
            return;
        }

        // teleportation enabled
        charController.enabled = false;
        ovrPlayerController.enabled = false;
        locomotion.SetActive(true);

    }

}
