using UnityEngine;
using UnityEngine.EventSystems;
using System;
using RPG.Characters;

namespace RPG.CameraUI {
    public class CameraRaycaster : MonoBehaviour {
        [SerializeField] Texture2D walkCursor = null;
        [SerializeField] Texture2D enemyCursor = null;
        [SerializeField] Texture2D lootCursor = null;
        [SerializeField] Texture2D unknownCursor = null;
        [SerializeField] Vector2 cursorHotspot = new Vector2(0, 0);

        const int WALKABLE_LAYER = 8;
        const float MAX_RAYCAST_DEPTH = 100f; // Hard coded value

        Camera mainCamera;
        Rect screenRect;

        public delegate void OnMouseOverWalkable(Vector3 destination);
        public event OnMouseOverWalkable onMouseOverWalkable;
        public delegate void OnMouseOverEnemy(Enemy enemy);
        public event OnMouseOverEnemy onMouseOverEnemy;

        void Start() {
            mainCamera = this.GetComponent<Camera>();
        }

        void Update() {
            screenRect = new Rect(0, 0, Screen.width, Screen.height);

            // Check if pointer is over an interactable UI element
            if (EventSystem.current.IsPointerOverGameObject()) {
                //Implement UI interaction
            } else {
                PerformRaycasts();
            }
        }

        void PerformRaycasts() {
            //only if mouse in inside screen
            if (screenRect.Contains(Input.mousePosition)) {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                Debug.DrawRay(ray.origin, ray.direction * 10);
                // Specify layer priorities below, order matters
                if (RaycastForEnemy(ray)) { return; }
                //if (RaycastForLootable(ray)) { return; }
                if (RaycastForWalkable(ray)) { return; }
                Cursor.SetCursor(unknownCursor, cursorHotspot, CursorMode.Auto);
            }
        }

        bool RaycastForEnemy(Ray ray) {
            /*RaycastHit hitInfo;
            bool somethingHit = Physics.Raycast(ray, out hitInfo, MAX_RAYCAST_DEPTH);
            if (somethingHit) {
                var gameObjectHit = hitInfo.collider.gameObject;
                print(gameObject.name);
                var enemyHit = gameObjectHit.GetComponent<Enemy>();
                if (enemyHit) {
                    print("Enemy hit");
                    Cursor.SetCursor(enemyCursor, cursorHotspot, CursorMode.Auto);
                    onMouseOverEnemy(enemyHit);
                    return true;
                }
            }
            return false;*/
            RaycastHit hitInfo;
            Physics.Raycast(ray, out hitInfo, MAX_RAYCAST_DEPTH);
            var gameObjectHit = hitInfo.collider.gameObject;
            //print(gameObjectHit.name);
            var enemyHit = gameObjectHit.GetComponent<Enemy>();
            if (!enemyHit) { enemyHit = gameObjectHit.GetComponentInParent<Enemy>(); }

            if (enemyHit) {
                Cursor.SetCursor(enemyCursor, cursorHotspot, CursorMode.Auto);
                onMouseOverEnemy(enemyHit);
                return true;
            }
            return false;
        }

        bool RaycastForLootable(Ray ray) {
            throw new NotImplementedException();
        }

        bool RaycastForWalkable(Ray ray) {
            RaycastHit hitInfo;
            LayerMask walkableLayer = 1 << WALKABLE_LAYER;
            bool walkableHit = Physics.Raycast(ray, out hitInfo, MAX_RAYCAST_DEPTH, walkableLayer);
            if (walkableHit) {
                Cursor.SetCursor(walkCursor, cursorHotspot, CursorMode.Auto);
                onMouseOverWalkable(hitInfo.point);
                return true;
            }
            return false;
        }
    }
}