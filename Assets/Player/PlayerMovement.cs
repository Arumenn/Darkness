using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof (ThirdPersonCharacter))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float walkMoveStopRadius = 0.2f;
    [SerializeField] float attackStopMoveRadius = 5f;

    ThirdPersonCharacter player;   // A reference to the ThirdPersonCharacter on the object
    CameraRaycaster cameraRaycaster;
    Vector3 currentDestination;
    Vector3 clickPoint;

    bool isInDirectMode = false;

    private void Start()
    {
        cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        player = GetComponent<ThirdPersonCharacter>();
        currentDestination = transform.position;
    }

    // Fixed update is called in sync with physics
    private void FixedUpdate() {
        if (Input.GetKeyDown(KeyCode.G)) { //G for gamepad TODO add menu
            isInDirectMode = !isInDirectMode;
            currentDestination = transform.position; //clear the click target
        }

        if (isInDirectMode) {
            ProcessDirectMovement();
        } else {
            ProcessMouseMovement();
        }
    }

    private void ProcessDirectMovement() {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // calculate camera relative direction to move:
        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 movement = v * cameraForward + h * Camera.main.transform.right;

        player.Move(movement, false, false);
    }

    private void ProcessMouseMovement() {
        if (Input.GetMouseButton(0)) {
            clickPoint = cameraRaycaster.hit.point;
            switch (cameraRaycaster.currentLayerHit) {
                case Layer.Walkable:
                    currentDestination = ShortDestination(clickPoint, walkMoveStopRadius);
                    break;
                case Layer.Enemy:
                    currentDestination = ShortDestination(clickPoint, attackStopMoveRadius);
                    break;
                default:
                    print("Unexpected layer found");
                    return;
            }
        }

        WalkToDestination();
    }

    private void WalkToDestination() {
        var playerToClickPoint = currentDestination - transform.position;
        if (playerToClickPoint.magnitude >= 0) {
            player.Move(playerToClickPoint, false, false);
        } else {
            player.Move(Vector3.zero, false, false);
        }
    }

    private Vector3 ShortDestination(Vector3 destination, float shortening) {
        Vector3 reductionVector = (destination - transform.position).normalized * shortening;
        return destination - reductionVector;
    }

    private void OnDrawGizmos() {
        //movement
        Gizmos.color = Color.black;
        Gizmos.DrawLine(transform.position, currentDestination);
        Gizmos.DrawSphere(currentDestination, 0.1f);
        Gizmos.DrawSphere(clickPoint, 0.15f);

        //attack
        Gizmos.color = new Color(255f, 0f, 0f, 0.5f);
        Gizmos.DrawWireSphere(transform.position, attackStopMoveRadius);
    }
}

