using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Characters {
    public class PowerAttackBehaviour : MonoBehaviour, ISpecialAbility {

        PowerAttackConfig config;

        public void SetConfig(PowerAttackConfig configToSet) {
            config = configToSet;
        }

        // Use this for initialization
        void Start() {
            print("Power Attack Behaviour attached to " + gameObject);
        }

        // Update is called once per frame
        void Update() {

        }

        public void Use(AbilityUseParams useParams) {
            print("Power Attack used on " + useParams.target + " Damage: " + useParams.baseDamage + " + " + config.GetExtraDamage());
            float damageToDeal = useParams.baseDamage + config.GetExtraDamage();
            useParams.target.TakeDamage(damageToDeal);
        }
    }
}