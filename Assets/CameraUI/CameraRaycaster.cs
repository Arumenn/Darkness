using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System;
using RPG.Characters;

namespace RPG.CameraUI {
    public class CameraRaycaster : MonoBehaviour {
        [SerializeField] Texture2D walkCursor = null;
        [SerializeField] Texture2D enemyCursor = null;
        [SerializeField] Texture2D unknownCursor = null;
        [SerializeField] Vector2 cursorHotspot = new Vector2(0, 0);

        const int WALKABLE_LAYER = 8;
        const float MAX_RAYCAST_DEPTH = 100f; // Hard coded value

        int topPriorityLayerLastFrame = -1; // So get ? from start with Default layer terrain

        public delegate void OnMouseOverWalkable(Vector3 destination);
        public event OnMouseOverWalkable onMouseOverWalkable;
        public delegate void OnMouseOverEnemy(Enemy enemy);
        public event OnMouseOverEnemy onMouseOverEnemy;

        void Update() {
            // Check if pointer is over an interactable UI element
            if (EventSystem.current.IsPointerOverGameObject()) {
                //Implement UI interaction
            } else {
                PerformRaycasts();
            }
        }

        void PerformRaycasts() {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            // Specify layer priorities below, order matters
            if (RaycastForEnemy(ray)) { return; }
            //if (RaycastForLootable(ray)) { return; }
            if (RaycastForWalkable(ray)) { return; }
        }

        private bool RaycastForLootable(Ray ray) {
            throw new NotImplementedException();
        }

        private bool RaycastForWalkable(Ray ray) {
            RaycastHit hitInfo;
            LayerMask walkableLayer = 1 << WALKABLE_LAYER;
            bool walkableHit = Physics.Raycast(ray, out hitInfo, walkableLayer);
            if (walkableHit) {
                Cursor.SetCursor(walkCursor, cursorHotspot, CursorMode.Auto);
                onMouseOverWalkable(hitInfo.point);
                return true;
            }
            return false;
        }

        private bool RaycastForEnemy(Ray ray) {
            RaycastHit hitInfo;
            Physics.Raycast(ray, out hitInfo, MAX_RAYCAST_DEPTH);
            var gameObjectHit = hitInfo.collider.gameObject;
            var enemyHit = gameObjectHit.GetComponent<Enemy>();
            if (enemyHit) {
                print("Enemy hit");
                Cursor.SetCursor(enemyCursor, cursorHotspot, CursorMode.Auto);
                onMouseOverEnemy(enemyHit);
                return true;
            }
            return false;
        }
    }
}