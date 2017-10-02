﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.ThirdPerson;

public class Enemy : MonoBehaviour, IDamageable {

    [SerializeField] float maxHealthPoints = 100f;
    [SerializeField] float chaseRadius = 10f;

    [SerializeField] float attackRadius = 4f;
    [SerializeField] float damagePerShot = 10f;
    [SerializeField] float secondsBetweenShots = 0.5f;
    [SerializeField] Vector3 aimOffset = new Vector3(0, 1f, 0);
    [SerializeField] GameObject projectileToUse;
    [SerializeField] GameObject projectileSocket;

    bool isAttacking = false;
    float currentHealthPoints;
    AICharacterControl aICharacterControl = null;
    GameObject player = null;

    public float healthAsPercentage {
        get {
            return currentHealthPoints / maxHealthPoints;
        }
    }

    // Use this for initialization
    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        aICharacterControl = GetComponent<AICharacterControl>();
        currentHealthPoints = maxHealthPoints;
    }

    // Update is called once per frame
    void Update() {
        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        if ((distanceToPlayer <= attackRadius) && (!isAttacking)) {
            isAttacking = true;
            InvokeRepeating("SpawnProjectile", 0f, secondsBetweenShots); //TODO switch to Coroutines
        }
        if (distanceToPlayer > attackRadius) {
            CancelInvoke("SpawnProjectile");
            isAttacking = false;
        }

        if (distanceToPlayer <= chaseRadius) {
            //start chasing
            aICharacterControl.SetTarget(player.transform);
        } else {
            aICharacterControl.SetTarget(transform);
        }
    }

    void SpawnProjectile() {
        transform.LookAt(player.transform); //make sure we look at the player
        //instantiate projectile
        GameObject newProjectile = Instantiate(projectileToUse, projectileSocket.transform.position, Quaternion.identity);
        Projectile projectileComponent = newProjectile.GetComponent<Projectile>();
        //sets damage
        projectileComponent.SetDamage(damagePerShot);
        //sets velocity
        Vector3 unitVectorToPlayer = (player.transform.position + aimOffset - projectileSocket.transform.position).normalized;
        float projectileSpeed = projectileComponent.projectileSpeed;
        newProjectile.GetComponent<Rigidbody>().velocity = unitVectorToPlayer * projectileSpeed;
    }

    public void TakeDamage(float damage) {
        currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
        if (currentHealthPoints <= 0) {
            Destroy(gameObject);
        }
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