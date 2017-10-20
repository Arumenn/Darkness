using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.Core;
using RPG.Weapons;

namespace RPG.Characters {
    public class Enemy : MonoBehaviour, IDamageable {

        [SerializeField] float maxHealthPoints = 100f;
        [SerializeField] float chaseRadius = 10f;

        [SerializeField] float attackRadius = 4f;
        [SerializeField] float damagePerShot = 10f;
        [SerializeField] float firingPeriodInSeconds = 0.5f;
        [SerializeField] float firingPeriodVariation = 0.1f;
        [SerializeField] Vector3 aimOffset = new Vector3(0, 1f, 0);
        [SerializeField] GameObject projectileToUse;
        [SerializeField] GameObject projectileSocket;

        bool isAttacking = false;
        float currentHealthPoints;
        AICharacterControl aICharacterControl = null;
        Player player = null;

        public float healthAsPercentage {
            get {
                return currentHealthPoints / maxHealthPoints;
            }
        }

        // Use this for initialization
        void Start() {
            player = GameObject.FindObjectOfType<Player>();
            aICharacterControl = GetComponent<AICharacterControl>();
            currentHealthPoints = maxHealthPoints;
        }

        // Update is called once per frame
        void Update() {
            if (player.healthAsPercentage <= Mathf.Epsilon) {
                //player is already dead
                StopAllCoroutines();
                Destroy(this); //to stop enemy behaviour
                return;
            }

            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            if ((distanceToPlayer <= attackRadius) && (!isAttacking)) {
                isAttacking = true;
                float randomizedDelay = UnityEngine.Random.Range(firingPeriodInSeconds - firingPeriodVariation, firingPeriodInSeconds + firingPeriodVariation);
                InvokeRepeating("FireProjectile", 0f, randomizedDelay); //TODO switch to Coroutines
            }
            if (distanceToPlayer > attackRadius) {
                CancelInvoke("FireProjectile");
                isAttacking = false;
            }

            if (distanceToPlayer <= chaseRadius) {
                //start chasing
                aICharacterControl.SetTarget(player.transform);
            } else {
                aICharacterControl.SetTarget(transform);
            }
        }

        //TODO separate projectile firing from enemy/player
        void FireProjectile() {
            transform.LookAt(player.transform); //make sure we look at the player
                                                //instantiate projectile
            GameObject newProjectile = Instantiate(projectileToUse, projectileSocket.transform.position, Quaternion.identity);
            Projectile projectileComponent = newProjectile.GetComponent<Projectile>();
            //sets damage
            projectileComponent.SetDamage(damagePerShot);
            projectileComponent.SetShooter(gameObject);
            //sets velocity
            Vector3 unitVectorToPlayer = (player.transform.position + aimOffset - projectileSocket.transform.position).normalized;
            float projectileSpeed = projectileComponent.GetDefaultLaunchSpeed();
            newProjectile.GetComponent<Rigidbody>().velocity = unitVectorToPlayer * projectileSpeed;
        }

        public void TakeDamage(float damage) {
            currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
            if (currentHealthPoints <= 0) {
                Destroy(gameObject);
            }
        }

        public void Heal(float health) {
            currentHealthPoints = Mathf.Clamp(currentHealthPoints + health, 0f, maxHealthPoints);
        }

        void OnDrawGizmos() {
            //Draw attack radius
            Gizmos.color = new Color(255f, 0f, 0, 0.5f);
            Gizmos.DrawWireSphere(transform.position, attackRadius);

            //Draw chase radius
            Gizmos.color = new Color(0f, 0f, 255f, 0.5f);
            Gizmos.DrawWireSphere(transform.position, chaseRadius);
        }
    }
}
