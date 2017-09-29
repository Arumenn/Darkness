using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public float projectileSpeed = 10f;
    private float damage = 5f;

    public void SetDamage(float d) {
        damage = d;
    }

    private void OnTriggerEnter(Collider collider) {
        var damageable = collider.gameObject.GetComponent(typeof(IDamageable));
        if (damageable) {
            (damageable as IDamageable).TakeDamage(damage);
        }
    }
}
