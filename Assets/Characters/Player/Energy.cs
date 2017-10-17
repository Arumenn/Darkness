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

        public float currentEnergyPoints;

        // Use this for initialization
        void Start() {
            currentEnergyPoints = maxEnergyPoints;
        }

        void Update() {
            UpdateEnergyBar();
        }

        public bool IsEnergyAvailable(float amount) {
            return amount <= currentEnergyPoints;
        }

        public void ConsumeEnergy(float amount) {
            float newEnergyPoints = currentEnergyPoints - amount;
            currentEnergyPoints = Mathf.Clamp(newEnergyPoints, 0, maxEnergyPoints);
        }

        private void UpdateEnergyBar() {
            float xValue = -(EnergyAsPercent() / 2f) - 0.5f;
            energyBarRawImage.uvRect = new Rect(xValue, 0f, 0.5f, 1f);
        }

        float EnergyAsPercent() {
            return currentEnergyPoints / maxEnergyPoints;
        }
    }
}