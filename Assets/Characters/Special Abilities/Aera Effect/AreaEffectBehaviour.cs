using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
using System;

namespace RPG.Characters {
    public class AreaEffectBehaviour : MonoBehaviour, ISpecialAbility {

        AreaEffectConfig config;
        ParticleSystem myParticleSystem;

        public void SetConfig(AreaEffectConfig configToSet) {
            config = configToSet;
        }

        // Use this for initialization
        void Start() {
            
        }

        // Update is called once per frame
        void Update() {

        }

        public void Use(AbilityUseParams useParams) {
            DealRadialDamage(useParams);
            PlayParticleEffect();
        }

        private void DealRadialDamage(AbilityUseParams useParams) {
            float damageToDeal = useParams.baseDamage + config.GetDamageToEachTarget();
            //static sphere cast for targets around me
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, config.GetRadius(), Vector3.up, config.GetRadius());
            int count = 0;
            foreach (RaycastHit hit in hits) {
                var damageable = hit.collider.gameObject.GetComponent<IDamageable>();
                var hitPlayer = hit.collider.gameObject.GetComponent<Player>();
                if ((damageable != null) && (!hitPlayer)) {
                    damageable.TakeDamage(damageToDeal);
                    count++;
                }
            }
        }

        private void PlayParticleEffect() {
            var prefab = Instantiate(config.GetParticlePrefab(), transform.position, Quaternion.identity);
            myParticleSystem = prefab.GetComponent<ParticleSystem>();
            myParticleSystem.Play();
            Destroy(prefab, myParticleSystem.main.duration);
        }
    }
}