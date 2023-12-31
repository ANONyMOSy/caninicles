using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    private StatusEffects statusEffects;
    private PlayerHealth playerHealth;
    private Actor actor;

    public ParticleSystem enemyDeath;
    public ParticleSystem enemyGas;

    [Header("Attacks")]
    [SerializeField] bool readyToAttack = true;
    [SerializeField] float cooldown = 4f;
    private Animator anim;

    bool isDead = false;

    void Awake() {
        statusEffects = GameObject.Find("Player").GetComponent<StatusEffects>();
        playerHealth = GameObject.Find("Player").GetComponent<PlayerHealth>();
        actor = GetComponent<Actor>();
        anim = GetComponent<Animator>();
    }

    void Update() {
        if(actor.inCombat && actor.currentHealth > 0) {
            anim.Play("EnemyCombat");
            if(readyToAttack) {
                readyToAttack = false;
                Invoke("ResetAttack", cooldown);

                Invoke("DealDamage", 1f);
            }
        } else if (actor.currentHealth <= 0 && isDead == false) {
            isDead = true;
            anim.Play("EnemyDeath");
            Invoke("PlayDeathParticles", 0.33f);
        } else if (isDead == false) {
            anim.Play("EnemyIdle");
        }
    }

    void PlayDeathParticles() {
        Instantiate(enemyDeath, transform.position + new Vector3(0, 3, 0), Quaternion.identity);
    }

    void DealDamage() {
        playerHealth.TakeDamage(5);
        statusEffects.AddEffect(StatusEffects.EffectType.Poison, 1, 5);
        ParticleSystem instantiatedObject = Instantiate(enemyGas, transform.position, Quaternion.identity);
    
        instantiatedObject.transform.Rotate(Vector3.right, -90f);
        // I love Taylor Traynor
    }

    void ResetAttack() {
        readyToAttack = true;
    }
}
