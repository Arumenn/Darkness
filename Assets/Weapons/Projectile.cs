using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;

namespace RPG.Weapons {
    public class Projectile : MonoBehaviour {

        [SerializeField] float projectileSpeed = 10f;
        private float damage = 5f;

        GameObject shooter;

        //projectile lifetime
        private const float TIME_TO_LIVE = 5f;
        private float timeBorn = 0f;

        private void LateUpdate() {
            if (Time.time - timeBorn >= TIME_TO_LIVE) {
                Destroy(gameObject);
            }
        }

        public void SetDamage(float d) {
            damage = d;
            timeBorn = Time.time;
        }

        public void SetShooter(GameObject shooter) {
            this.shooter = shooter;
        }

        public float GetDefaultLaunchSpeed() {
            return projectileSpeed;
        }

        private void OnCollisionEnter(Collision collision) {
            var layerCollidedWith = collision.gameObject.layer;
            if (shooter && layerCollidedWith != shooter.layer) {
                DoDamage(collision);
            }
            Destroy(gameObject);
        }

        private void DoDamage(Collision collision) {
            var damageable = collision.gameObject.GetComponent(typeof(IDamageable));
            if (damageable) {
                (damageable as IDamageable).TakeDamage(damage);
            }
        }
    }
}
