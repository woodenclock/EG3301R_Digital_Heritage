using UnityEngine;

public class CameraMover : MonoBehaviour {

    public GameObject rig1;                // Reference to XR Rig1 (initial rig)
    public GameObject rig2;                // Reference to XR Rig2 (target rig)
    public Transform forwardPosition;      // Reference to the target position (near the kitchen table)
    public Transform forwardRotation;      // Reference to the target rotation
    public Transform backPosition;         // Reference to the position to move the camera back to
    public Transform backRotation;         // Reference to the rotation when moving the camera back
    public float moveSpeed = 2.0f;         // Speed of the camera movement
    public float rotationSpeed = 2.0f;     // Speed of the camera rotation

    private bool shouldMoveForward = false; // Whether the camera should move forward
    private bool shouldMoveBack = false;    // Whether the camera should move back


    void Start() {
        // Ensure Rig1 starts active and Rig2 starts inactive
        rig1.SetActive(true);
        rig2.SetActive(false);
    }


    // Call this method to move forward (transition to Rig2)
    public void MoveCameraForward() {
        shouldMoveForward = true;
        shouldMoveBack = false;
        Debug.Log("Moving to Rig2");
    }

    // Call this method to move back (transition to Rig1)
    public void MoveCameraBack() {
        shouldMoveForward = false;
        shouldMoveBack = true;
        Debug.Log("Moving to Rig1");
    }

    void Update() {
        // If shouldMoveForward is true, move towards the target and switch to Rig2
        if (shouldMoveForward) {
            MoveRigTowards(forwardPosition, forwardRotation, rig1);

            if (HasReachedTarget(forwardPosition, forwardRotation, rig1)) {
                SwitchToRig2();
            }
        }

        // If shouldMoveBack is true, move back and switch to Rig1
        if (shouldMoveBack) {
            MoveRigTowards(backPosition, backRotation, rig2);

            if (HasReachedTarget(backPosition, backRotation, rig2)) {
                SwitchToRig1();
            }
        }
    }

    // Helper method to move the active rig towards a target position and rotation
    private void MoveRigTowards(Transform targetPos, Transform targetRot, GameObject rig) {
        rig.transform.position = Vector3.Lerp(rig.transform.position, targetPos.position, moveSpeed * Time.deltaTime);
        rig.transform.rotation = Quaternion.Lerp(rig.transform.rotation, targetRot.rotation, rotationSpeed * Time.deltaTime);
    }

    // Helper method to check if the active rig has reached the target
    private bool HasReachedTarget(Transform targetPos, Transform targetRot, GameObject rig) {
        return Vector3.Distance(rig.transform.position, targetPos.position) < 0.01f &&
               Quaternion.Angle(rig.transform.rotation, targetRot.rotation) < 0.5f;
    }

    // Switch to Rig2
    private void SwitchToRig2() {
        rig1.SetActive(false);
        rig2.SetActive(true);
        shouldMoveForward = false;
        Debug.Log("Switched to Rig2");
    }

    // Switch back to Rig1
    private void SwitchToRig1() {
        rig2.SetActive(false);
        rig1.SetActive(true);
        shouldMoveBack = false;
        Debug.Log("Switched to Rig1");
    }
}
