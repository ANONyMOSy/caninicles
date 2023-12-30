using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    private int health;
    public int maxHealth = 100;

    public TextMeshProUGUI healthTex;


    void Awake() {
        health = maxHealth;
    }

    public void TakeDamage(int dmg) {
        health -= dmg;
    }

    void Update() {
        if(health > maxHealth) {
            health = maxHealth;
        }
        if(health <= 0) {
            PlayerDie();
        }
        healthTex.text = health + "/" + maxHealth;
    }

    void PlayerDie() {
        Destroy(gameObject);
    }
}
