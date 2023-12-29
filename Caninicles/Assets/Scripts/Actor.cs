using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth { get; private set; }

    void Awake() {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount) {
        currentHealth -= amount;

        Debug.Log("TOOK DAMAGE");
        if (currentHealth <= 0) {
            Die();
        }
    }

    void Die() {


        Destroy(gameObject);
    }
}
