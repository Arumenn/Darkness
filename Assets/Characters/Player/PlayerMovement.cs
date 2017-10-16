using System;
using UnityEngine;
using UnityEngine.AI;
using RPG.CameraUI;

namespace RPG.Characters {
    [RequireComponent(typeof(ThirdPersonCharacter))]
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(AICharacterControl))]
    public class PlayerMovement : MonoBehaviour {
        ThirdPersonCharacter player = null;   // A reference to the ThirdPersonCharacter on the object
        CameraRaycaster cameraRaycaster = null;
        AICharacterControl aICharacterControl = null;

        Vector3 clickPoint;
        GameObject walkTarget;

        void Start() {
            cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
            player = GetComponent<ThirdPersonCharacter>();
            aICharacterControl = GetComponent<AICharacterControl>();

            cameraRaycaster.onMouseOverWalkable += OnMouseOverWalkable;
            cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemy;

            walkTarget = new GameObject("walkTarget");
        }

        void OnMouseOverWalkable(Vector3 destination) {
            if (Input.GetMouseButton(0)) {
                walkTarget.transform.position = destination;
                aICharacterControl.SetTarget(walkTarget.transform);
            }
        }

        void OnMouseOverEnemy(Enemy enemy) {
            if ((Input.GetMouseButton(0)) || (Input.GetMouseButtonDown(1))) {
                //walk to enemy
                walkTarget.transform.position = enemy.transform.position;
                aICharacterControl.SetTarget(walkTarget.transform);
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
}

