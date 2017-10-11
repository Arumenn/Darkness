using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.CameraUI {
    [RequireComponent(typeof(CameraRaycaster))]
    public class CursorAffordance : MonoBehaviour {

        [SerializeField] Texture2D walkCursor = null;
        [SerializeField] Texture2D attackCursor = null;
        [SerializeField] Texture2D unknownCursor = null;
        [SerializeField] Vector2 cursorHotspot = new Vector2(0, 0);

        // TODO solve fight between serialize and const
        [SerializeField] const int walkableLayerNumber = 8;
        [SerializeField] const int enemyLayerNumber = 9;
        [SerializeField] const int stiffLayerNumber = 12;


        CameraRaycaster cameraRaycaster;

        // Use this for initialization
        void Start() {
            cameraRaycaster = GetComponent<CameraRaycaster>();
            cameraRaycaster.notifyLayerChangeObservers += OnLayerChange; //registers an observer for the camera layer change
        }

        void OnLayerChange(int newLayer) {
            switch (newLayer) {
                case enemyLayerNumber:
                    Cursor.SetCursor(attackCursor, cursorHotspot, CursorMode.Auto);
                    break;
                case walkableLayerNumber:
                    Cursor.SetCursor(walkCursor, cursorHotspot, CursorMode.Auto);
                    break;
                default:
                    Cursor.SetCursor(unknownCursor, cursorHotspot, CursorMode.Auto);
                    return;
            }
        }

        // TODO consider de-registering OnLayerChange on leaving all game scenes
    }
}