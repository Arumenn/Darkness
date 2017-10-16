using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.CameraUI;

namespace RPG.Characters {
    [RequireComponent(typeof(RawImage))]
    public class Energy : MonoBehaviour {

        [SerializeField] RawImage energyBarRawImage;
        [SerializeField] float maxEnergyPoints = 100f;
        [SerializeField] float pointsPerHit = 10f;

        CameraRaycaster cameraRaycaster = null;
        public float currentEnergyPoints;

        // Use this for initialization
        void Start() {
            cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
            cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemy;

            currentEnergyPoints = maxEnergyPoints;
        }

        void Update() {
            UpdateEnergyBar();
        }

        private void UpdateEnergyBar() {
            float xValue = -(EnergyAsPercent() / 2f) - 0.5f;
            energyBarRawImage.uvRect = new Rect(xValue, 0f, 0.5f, 1f);
        }

        void OnMouseOverEnemy(Enemy enemy) {
            if (Input.GetMouseButtonDown(1)) {
                float newEnergyPoints = currentEnergyPoints - pointsPerHit;
                currentEnergyPoints = Mathf.Clamp(newEnergyPoints, 0, maxEnergyPoints);
            }
        }

        float EnergyAsPercent() {
            return currentEnergyPoints / maxEnergyPoints;
        }
    }
}