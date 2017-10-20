using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
using System;

namespace RPG.Characters {
    public class SelfHealBehaviour : MonoBehaviour, ISpecialAbility {

        SelfHealConfig config;
        ParticleSystem myParticleSystem;
        Player player = null;
        AudioSource audioSource = null;


        public void SetConfig(SelfHealConfig configToSet) {
            config = configToSet;
        }

        // Use this for initialization
        void Start() {
            player = GetComponent<Player>();
            audioSource = GetComponent<AudioSource>();
        }

        // Update is called once per frame
        void Update() {

        }

        public void Use(AbilityUseParams useParams) {
            DoSelfHeal(useParams);
            PlaySound();
            PlayParticleEffect();
        }

        private void DoSelfHeal(AbilityUseParams useParams) {
            float pointsToHeal = config.GetHealingAmount();
            player.Heal(pointsToHeal);
        }

        private void PlaySound() {
            audioSource.clip = config.GetAudioClip();
            audioSource.Play();
        }

        private void PlayParticleEffect() {
            var prefab = Instantiate(config.GetParticlePrefab(), transform.position, Quaternion.identity);
            prefab.transform.parent = transform; //attach to player
            myParticleSystem = prefab.GetComponent<ParticleSystem>();
            myParticleSystem.Play();
            Destroy(prefab, myParticleSystem.main.duration);
        }
    }
}