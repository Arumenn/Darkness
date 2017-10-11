using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using RPG.CameraUI;
using RPG.Core;
using RPG.Weapons;

namespace RPG.Characters {
    public class Player : MonoBehaviour, IDamageable {

        [SerializeField] int level = 8;
        [SerializeField] float maxHealthPoints = 100f;
        [SerializeField] float damagePerHit = 10f;
        [SerializeField] float minTimeBetweenHits = 0.5f;
        [SerializeField] float maxAttackRange = 2f;

        [SerializeField] Weapon weaponInUse;

        float currentHealthPoints;
        GameObject currentTarget;
        CameraRaycaster cameraRaycaster;
        float lastHitTime = 0f;

        public float healthAsPercentage {
            get {
                return currentHealthPoints / maxHealthPoints;
            }
        }

        void Start() {
            currentHealthPoints = maxHealthPoints;
            RegisterForMouseClick();
            PutWeaponInHand();
        }

        private void PutWeaponInHand() {
            var weaponPrefab = weaponInUse.GetWeaponPrefab();
            GameObject dominantHand = RequestDominantHand();
            var weapon = Instantiate(weaponPrefab, dominantHand.transform);
            weapon.transform.localPosition = weaponInUse.gripTransform.localPosition;
            weapon.transform.localRotation = weaponInUse.gripTransform.localRotation;
        }

        private GameObject RequestDominantHand() {
            var dominantHands = GetComponentsInChildren<DominantHand>();
            int numberOfDominantHands = dominantHands.Length;
            //throws errors to the designer
            Assert.IsFalse(numberOfDominantHands <= 0, "No DominantHand found on Player, please add one");
            Assert.IsFalse(numberOfDominantHands > 1, "Multiple DominantHand found on Player, please remove some");
            //all is good we return the ONLY dominantHand;
            return dominantHands[0].gameObject;
        }

        private void RegisterForMouseClick() {
            cameraRaycaster = FindObjectOfType<CameraRaycaster>();
            cameraRaycaster.notifyMouseClickObservers += OnMouseClick;
        }

        private void OnMouseClick(RaycastHit raycastHit, int layerHit) {
            if (layerHit == 9) {
                var enemy = raycastHit.collider.gameObject;
                //check enemy is in range
                if ((enemy.transform.position - transform.position).magnitude > maxAttackRange) {
                    return;
                }
                currentTarget = enemy;
                var enemyComponent = enemy.GetComponent<Enemy>();
                //do damage
                if (Time.time - lastHitTime > minTimeBetweenHits) {
                    transform.LookAt(currentTarget.transform);
                    enemyComponent.TakeDamage(damagePerHit);
                    lastHitTime = Time.time;
                }
            }
        }

        public void TakeDamage(float damage) {
            currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
        }
    }
}
