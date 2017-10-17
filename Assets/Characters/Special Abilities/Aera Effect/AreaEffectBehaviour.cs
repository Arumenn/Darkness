using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;

namespace RPG.Characters {
    public class AreaEffectBehaviour : MonoBehaviour, ISpecialAbility {

        AreaEffectConfig config;

        public void SetConfig(AreaEffectConfig configToSet) {
            config = configToSet;
        }

        // Use this for initialization
        void Start() {
            print("Area Effect Behaviour attached to " + gameObject);
        }

        // Update is called once per frame
        void Update() {

        }

        public void Use(AbilityUseParams useParams) {
            float damageToDeal = useParams.baseDamage + config.GetDamageToEachTarget();
            print("Area Effect used with a radius of " + config.GetRadius() + " Damage: " + useParams.baseDamage + " + " + config.GetDamageToEachTarget() + " = " + damageToDeal);
            //static sphere cast for targets around me
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, config.GetRadius(), Vector3.up, config.GetRadius());
            int count = 0;
            foreach (RaycastHit hit in hits) {
                var damageable = hit.collider.gameObject.GetComponent<IDamageable>();
                if (damageable != null) {
                    damageable.TakeDamage(damageToDeal);
                    count++;
                }
            }
            print("Area Effect used on " + count + " targets");
        }
    }
}