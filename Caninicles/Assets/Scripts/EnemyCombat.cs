using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    private StatusEffects statusEffects;
    private PlayerHealth playerHealth;
    private Actor actor;

    [Header("Attacks")]
    [SerializeField] bool readyToAttack = true;
    [SerializeField] float cooldown = 4f;

    void Awake() {
        statusEffects = GameObject.Find("Player").GetComponent<StatusEffects>();
        playerHealth = GameObject.Find("Player").GetComponent<PlayerHealth>();
        actor = GetComponent<Actor>();
    }

    void Update() {
        if(actor.inCombat) {
            if(readyToAttack) {
                readyToAttack = false;
                Invoke("ResetAttack", cooldown);

                playerHealth.TakeDamage(5);
                statusEffects.AddEffect(StatusEffects.EffectType.Poison, 1, 5);
            }
        }
    }

    void ResetAttack() {
        readyToAttack = true;
    }
}
