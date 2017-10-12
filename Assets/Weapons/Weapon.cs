using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Weapons {
    [CreateAssetMenu(menuName = ("RPG/Weapon"))]
    public class Weapon : ScriptableObject {

        public Transform gripTransform;

        [SerializeField] GameObject weaponPrefab;
        [SerializeField] AnimationClip attackAnimation;

        public GameObject GetWeaponPrefab() {
            return weaponPrefab;
        }

        public AnimationClip GetAttackAnimation() {
            RemoveAnimationEvents();
            return attackAnimation;
        }

        private void RemoveAnimationEvents() {
            //to prevent errors with custom events in imported assets packs
            attackAnimation.events = new AnimationEvent[0];
        }

    }
}
