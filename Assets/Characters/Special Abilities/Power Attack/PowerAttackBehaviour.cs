﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters {
    public class PowerAttackBehaviour : MonoBehaviour, ISpecialAbility {

        PowerAttackConfig config;
        ParticleSystem myParticleSystem;

        public void SetConfig(PowerAttackConfig configToSet) {
            config = configToSet;
        }

        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }

        public void Use(AbilityUseParams useParams) {
            DealPowerDamage(useParams);
            PlayParticleEffect();
        }

        private void DealPowerDamage(AbilityUseParams useParams) {
            float damageToDeal = useParams.baseDamage + config.GetExtraDamage();
            useParams.target.TakeDamage(damageToDeal);
        }

        private void PlayParticleEffect() {
            var prefab = Instantiate(config.GetParticlePrefab(), transform.position, Quaternion.identity);
            myParticleSystem = prefab.GetComponent<ParticleSystem>();
            myParticleSystem.Play();
            Destroy(prefab, myParticleSystem.main.duration);
        }
    }
}