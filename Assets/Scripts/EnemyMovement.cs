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
    [SerializeField]
    private float floatingSpeed;

    private NavMeshAgent enemy;
    private GameObject player;
    private bool dead;

    void Start()
    {
        enemy = GetComponent<NavMeshAgent>();
        dead = false;
        player = GameObject.FindWithTag("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            TakeDamage(bulletDamage);
            Destroy(other.gameObject);
        }
    }

    void Update()
    {
        if (dead == false)
        { 
            enemy.destination = player.transform.position; 
        }
        else
        {
            transform.Translate(Vector3.up * Time.deltaTime * floatingSpeed, Space.World);
        }
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
        dead = true;
        enemy.enabled = false;
        gameObject.GetComponent<Collider>().enabled = false;
        Destroy(gameObject, 5);
    }
}
