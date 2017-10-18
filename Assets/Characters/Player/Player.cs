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
        [SerializeField] SpecialAbility[] abilities;

        AudioSource audioSource;
        Animator animator;
        float currentHealthPoints;
        CameraRaycaster cameraRaycaster;
        float lastHitTime = 0f;

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

        public void TakeDamage(float damage) {
            ReduceHealth(damage);
            bool playerDies = (currentHealthPoints <= 0);
            if (playerDies) {
                StartCoroutine(KillPlayer());
            } else {
                audioSource.clip = hurtSounds[UnityEngine.Random.Range(0, hurtSounds.Length)];
                audioSource.Play();
            }
        }

        private void ReduceHealth(float damage) {
            currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
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
            if ((Input.GetMouseButton(0)) && (IsTargetInRange(enemy.gameObject))) {
                AttackTarget(enemy);
            }else if ((Input.GetMouseButtonDown(1)) && (IsTargetInRange(enemy.gameObject))) {
                AttempSpecialAbility(0, enemy);
            }else if (Input.GetMouseButtonDown(2)) {
                AttempSpecialAbility(1, enemy);
            }
        }

        void AttachAbilities() {
            foreach (SpecialAbility sa in abilities) {
                sa.AttachComponentTo(gameObject);
            }
        }

        private void AttempSpecialAbility(int abilityIndex, Enemy enemy) {
            print("Attempt Special Ability " + abilityIndex + " on " + enemy);
            var energyComponent = GetComponent<Energy>();
            SpecialAbility specialAbility = abilities[abilityIndex];
            if (energyComponent.IsEnergyAvailable(specialAbility.GetEnergyCost())) {
                energyComponent.ConsumeEnergy(specialAbility.GetEnergyCost());
                var abilityParams = new AbilityUseParams(enemy, baseDamage);
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
                animator.SetTrigger("Attack"); //TODO make const
                enemy.TakeDamage(baseDamage);
                lastHitTime = Time.time;
            }
        }        

        IEnumerator KillPlayer() {
            audioSource.clip = deathSounds[UnityEngine.Random.Range(0, deathSounds.Length)];
            audioSource.Play();

            Debug.Log("death anim");

            yield return new WaitForSecondsRealtime(audioSource.clip.length);

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
