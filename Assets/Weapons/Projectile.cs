using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public float projectileSpeed = 10f;
    private float damage = 5f;

    //projectile lifetime
    private float timeToLive = 5f;
    private float timeBorn = 0f;

    private void LateUpdate() {
        if (Time.time - timeBorn >= timeToLive) {
            Destroy(gameObject);
        }
    }

    public void SetDamage(float d) {
        damage = d;
        timeBorn = Time.time;
    }

    private void OnCollisionEnter(Collision collision) {
        var damageable = collision.gameObject.GetComponent(typeof(IDamageable));
        if (damageable) {
            (damageable as IDamageable).TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}
