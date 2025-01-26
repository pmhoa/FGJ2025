using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    private float health;
    [SerializeField]
    private float bulletDamage;
    [SerializeField]
    private float foamDamage;
    [SerializeField]
    private float floatingSpeed;
    [SerializeField]
    private bool projectiles;
    [SerializeField]
    private float projectileCoolDown;
    [SerializeField]
    private Transform barrel;
    [SerializeField]
    private GameObject projectile;
    [SerializeField]
    private float projectileSpeed;
    [SerializeField]
    private GameObject michaelBubble;

    private float projectileTimer;
    private NavMeshAgent enemy;
    private GameObject player;
    private bool dead;
    private bool flipCharacter;
    [SerializeField]
    private Animator animator;

    void Start()
    {
        enemy = GetComponent<NavMeshAgent>();
        dead = false;
        player = GameObject.FindWithTag("Player");
        projectileTimer = projectileCoolDown;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
        if (other.CompareTag("Bullet"))
        {
            TakeDamage(bulletDamage);
            Destroy(other.gameObject);
        }
        if (other.CompareTag("FoamBubble"))
        {
            TakeDamage(foamDamage);
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

        if (enemy.velocity.x > 0 && flipCharacter == false)
        {
            animator.gameObject.transform.localScale = new Vector3(-0.5f, 0.5f, 0.5f);
            flipCharacter = true;
        }
        else if (enemy.velocity.x < 0 && flipCharacter == true)
        {
            animator.gameObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            flipCharacter = false;
        }

        if(projectiles && dead == false)
        {
            projectileTimer -= Time.deltaTime;

            if (projectileTimer < 0f)
            {
                projectileTimer = projectileCoolDown;
                Shoot();
                Debug.Log("Shoot");
            }
        }

    }

    void TakeDamage( float damageAmount)
    {
        health -= damageAmount;
        if(health <= 0f)
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
        michaelBubble.SetActive(true);
        gameObject.GetComponent<Collider>().enabled = false;
        Destroy(gameObject, 5);
    }

    void Shoot()
    {
        GameObject projectileClone = Instantiate(projectile, barrel.position, barrel.rotation);
        Rigidbody rb = projectileClone.GetComponent<Rigidbody>();
        rb.velocity = barrel.forward * projectileSpeed;
    }
}
