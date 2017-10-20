using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using RPG.CameraUI;
using RPG.Core;
using RPG.Weapons;

namespace RPG.Characters {
    public class Player : MonoBehaviour, IDamageable {

        [Header("General")]
        [SerializeField] AnimatorOverrideController animatorOverrideController;

        [Header("Reaction Sounds")]
        [SerializeField] AudioClip[] hurtSounds;
        [SerializeField] AudioClip[] deathSounds;

        [Header("Combat")]
        [SerializeField] int level = 8;
        [SerializeField] float maxHealthPoints = 100f;
        [SerializeField] float baseDamage = 10f;
        [SerializeField] Weapon weaponInUse;
        [SerializeField] AbilityConfig[] abilities;

        AudioSource audioSource;
        Animator animator;
        float currentHealthPoints = 0f;
        CameraRaycaster cameraRaycaster = null;
        Enemy currentEnemy = null;
        float lastHitTime = 0f;

        const string ATTACK_TRIGGER = "Attack";
        const string DEATH_TRIGGER = "Death";

        public float healthAsPercentage {
            get {
                return currentHealthPoints / maxHealthPoints;
            }
        }

        void Start() {
            SetCurrentMaxHealth();
            RegisterForMouseClick();
            PutWeaponInHand();
            SetupRuntimeAnimator();
            AttachAbilities();
            audioSource = GetComponent<AudioSource>();
        }

        void Update() {
            if (healthAsPercentage > Mathf.Epsilon) { //if alive
                ScanForAbilityKeyDown();
            }
        }

        public void TakeDamage(float damage) {
            currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);

            if (currentHealthPoints <= 0) {
                StartCoroutine(KillPlayer());
            } else {
                audioSource.clip = hurtSounds[UnityEngine.Random.Range(0, hurtSounds.Length)];
                audioSource.Play();
            }
        }

        public void Heal(float health) {
            currentHealthPoints = Mathf.Clamp(currentHealthPoints + health, 0f, maxHealthPoints);
            //audioSource.clip = hurtSounds[UnityEngine.Random.Range(0, hurtSounds.Length)];
            //audioSource.Play();
        }

        private void SetCurrentMaxHealth() {
            currentHealthPoints = maxHealthPoints;
        }

        private void SetupRuntimeAnimator() {
            animator = GetComponent<Animator>();
            animator.runtimeAnimatorController = animatorOverrideController;
            animatorOverrideController["DEFAULT_ATTACK"] = weaponInUse.GetAttackAnimation();
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
            cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemy;
        }

        void OnMouseOverEnemy(Enemy enemy) {
            this.currentEnemy = enemy;
            if ((Input.GetMouseButton(0)) && (IsTargetInRange(enemy.gameObject))) {
                AttackTarget(enemy);
            }else if ((Input.GetMouseButtonDown(1)) && (IsTargetInRange(enemy.gameObject))) {
                AttempSpecialAbility(0);                
            }
        }

        private void ScanForAbilityKeyDown(){
            for (int keyIndex = 1; keyIndex < abilities.Length; keyIndex++) {
                if (Input.GetKeyDown(keyIndex.ToString())) {
                    AttempSpecialAbility(keyIndex);
                }
            }
        }

        void AttachAbilities() {
            foreach (AbilityConfig sa in abilities) {
                sa.AttachComponentTo(gameObject);
            }
        }

        private void AttempSpecialAbility(int abilityIndex) {
            var energyComponent = GetComponent<Energy>();
            AbilityConfig specialAbility = abilities[abilityIndex];
            if (energyComponent.IsEnergyAvailable(specialAbility.GetEnergyCost())) {
                energyComponent.ConsumeEnergy(specialAbility.GetEnergyCost());
                var abilityParams = new AbilityUseParams(this.currentEnemy, baseDamage);
                specialAbility.Use(abilityParams);
            } else {
                print("Not enough mana");
            }
        }

        private bool IsTargetInRange(GameObject target) {
            float distanceToTarget = (target.transform.position - transform.position).magnitude;
            return distanceToTarget <= weaponInUse.GetMaxAttackRange();
        }

        private void AttackTarget(Enemy enemy) {
            //do damage
            if (Time.time - lastHitTime > weaponInUse.GetMinTimeBetweenHits()) {
                transform.LookAt(enemy.transform);
                animator.SetTrigger(ATTACK_TRIGGER);
                enemy.TakeDamage(baseDamage);
                lastHitTime = Time.time;
            }
        }        

        IEnumerator KillPlayer() {
            animator.SetTrigger(DEATH_TRIGGER);

            audioSource.clip = deathSounds[UnityEngine.Random.Range(0, deathSounds.Length)];
            audioSource.Play();
            yield return new WaitForSecondsRealtime(audioSource.clip.length);

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
