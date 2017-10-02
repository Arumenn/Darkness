﻿using System.Collections;
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
            print("dead projectile");
            Destroy(gameObject);
        }
    }

    public void SetDamage(float d) {
        damage = d;
        timeBorn = Time.time;
        print("new projectile");
    }

    private void OnTriggerEnter(Collider collider) {
        var damageable = collider.gameObject.GetComponent(typeof(IDamageable));
        if (damageable) {
            (damageable as IDamageable).TakeDamage(damage);
        }
    }
}
