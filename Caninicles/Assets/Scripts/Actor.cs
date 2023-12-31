using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Actor : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth { get; private set; }
    private Animator anim;

    private Slider healthBar;

    public bool inCombat = false;
    private float combatTimer = 10f;

    void Awake() {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
        healthBar = GetComponentInChildren<Slider>();
    }

    void Update() {
        combatTimer -= Time.deltaTime;

        if (combatTimer <= 0) {
            inCombat = false;
        }
        healthBar.value = (float)currentHealth / (float)maxHealth;
    }

    public void TakeDamage(int amount) {
        currentHealth -= amount;
        combatTimer = 10f;
        inCombat = true;

        Debug.Log("TOOK DAMAGE");
        if (currentHealth <= 0) {
            Die();
        }
    }

    void Die() {

        Invoke("Destruct", 0.33f);
    }

    void Destruct() {
        Destroy(gameObject);
    }
}
