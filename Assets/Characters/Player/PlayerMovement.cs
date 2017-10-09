using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine.AI;

[RequireComponent(typeof (ThirdPersonCharacter))]
[RequireComponent(typeof (NavMeshAgent))]
[RequireComponent(typeof (AICharacterControl))]
public class PlayerMovement : MonoBehaviour
{
    ThirdPersonCharacter player = null;   // A reference to the ThirdPersonCharacter on the object
    CameraRaycaster cameraRaycaster = null;
    AICharacterControl aICharacterControl = null;

    Vector3 clickPoint;
    GameObject walkTarget;

    // TODO solve fight between serialize and const
    [SerializeField] const int walkableLayerNumber = 8;
    [SerializeField] const int enemyLayerNumber = 9;
    [SerializeField] const int stiffLayerNumber = 12;

    void Start()
    {
        cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        player = GetComponent<ThirdPersonCharacter>();
        aICharacterControl = GetComponent<AICharacterControl>();

        cameraRaycaster.notifyMouseClickObservers += ProcessMouseClick; //attach event listener
        walkTarget = new GameObject("walkTarget");
    }

    void ProcessMouseClick(RaycastHit raycastHit, int layerHit) {
        switch (layerHit) {
            case enemyLayerNumber:
                //navigate to enemy
                GameObject enemy = raycastHit.collider.gameObject;
                aICharacterControl.SetTarget(enemy.transform);
                break;
            case walkableLayerNumber:
                //navigate to point on the ground
                walkTarget.transform.position = raycastHit.point;
                aICharacterControl.SetTarget(walkTarget.transform);
                break;
            default:
                Debug.LogWarning("Don't know how to handle mouse click for player movement");
                return;
        }
    }


    //TODO make this called again
    void ProcessDirectMovement() {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        // calculate camera relative direction to move:
        Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 movement = v * cameraForward + h * Camera.main.transform.right;

        player.Move(movement, false, false);
    }
    
}

