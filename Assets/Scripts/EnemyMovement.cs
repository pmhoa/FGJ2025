using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    private float health;
    [SerializeField]
    private float bulletDamage;

    private NavMeshAgent enemy;
    private GameObject player;

    void Start()
    {
        enemy = GetComponent<NavMeshAgent>();

        player = GameObject.FindWithTag("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            TakeDamage(bulletDamage);
        }
    }

    void Update()
    {
        enemy.destination = player.transform.position;
    }

    void TakeDamage( float damageAmount)
    {
        health -= damageAmount;
        if(health < 0f)
        {
            health = 0f;
            Die();
        }
    }

    void Die()
    {
        enemy.isStopped = true;
        Destroy(gameObject, 5);
    }
}
