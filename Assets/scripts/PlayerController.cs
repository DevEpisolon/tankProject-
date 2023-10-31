using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Healthbar healthBar;
    public int maxHealth;
    int currentHealth;
    public GameObject explosionPrefab;
    public GameObject firePrefab;

    public RespawnManager respawnManager;
    
    private bool isDead = false; // Flag to track player's death status

    void Start()
    {
        currentHealth = maxHealth;
        healthBar.setMaxHealth(maxHealth);
    }

    public void Update()
    {
        if (isDead)
        {
            // Player is dead, do not process input
            return;
        }

        if (Input.GetKeyDown("space"))
        {
            TakeDamage(2);
        }
    }

    public void Die()
    {
        if (isDead)
        {
            return; // Already in the process of respawning
        }

        isDead = true; // Set the flag to indicate the player is dead

        GameObject explosionObject = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        ParticleSystem instantiatedEffect = explosionObject.GetComponent<ParticleSystem>();
        GameObject fireObject = Instantiate(firePrefab, transform.position, Quaternion.identity);
        ParticleSystem instantiatedEffect2 = fireObject.GetComponent<ParticleSystem>();
        instantiatedEffect2.Play();
        instantiatedEffect.Play();
        Destroy(explosionObject, instantiatedEffect.main.duration);
        Destroy(fireObject, instantiatedEffect2.main.duration);    
        respawnManager.RespawnPlayer();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.setHealth(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }
}
