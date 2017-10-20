using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Weapons {
    [CreateAssetMenu(menuName = ("RPG/Weapon"))]
    public class Weapon : ScriptableObject {

        public Transform gripTransform;

        [SerializeField] GameObject weaponPrefab;
        [SerializeField] AnimationClip attackAnimation;
        [SerializeField] float minTimeBetweenHits = 0.5f;
        [SerializeField] float maxAttackRange = 2.5f;
        [SerializeField] float additionalDamage = 10f;

        public GameObject GetWeaponPrefab() {
            return weaponPrefab;
        }

        public AnimationClip GetAttackAnimation() {
            RemoveAnimationEvents();
            return attackAnimation;
        }

        public float GetMinTimeBetweenHits() {
            return minTimeBetweenHits;
        }

        public float GetMaxAttackRange() {
            return maxAttackRange;
        }

        public float GetAdditionalDamage() {
            return additionalDamage;
        }

        private void RemoveAnimationEvents() {
            //to prevent errors with custom events in imported assets packs
            attackAnimation.events = new AnimationEvent[0];
        }

    }
}
