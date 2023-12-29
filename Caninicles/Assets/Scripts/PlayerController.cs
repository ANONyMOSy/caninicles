using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    const string IDLE = "Idle";
    const string WALK = "Walk";
    const string PEE = "Pee";
    const string ATTACK = "Attack";

    private bool peeing = false;

    CustomActions input;

    NavMeshAgent agent;
    Animator animator;

    [Header("Movement")] 
    [SerializeField] ParticleSystem clickEffect;
    [SerializeField] LayerMask clickableLayers;

    [SerializeField] ParticleSystem pee;

    public Transform peeLocation;

    float lookRotationSpeed = 8f;

    [Header("Attack")]
    [SerializeField] float attackSpeed = 1.5f;
    [SerializeField] float attackDelay = 0.3f;
    [SerializeField] float attackDistance = 1.5f;
    [SerializeField] int attackDamage = 1;
    [SerializeField] ParticleSystem hitEffect;

    bool playerBusy = false;
    Interactable target;

    void Awake() {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        input = new CustomActions();
        AssignInputs();
    }

    void AssignInputs() {
        input.Main.Move.performed += ctx => ClickToMove();
        input.Main.Pee.performed += ctx => TryPee();
    }

    void TryPee() {
    if (agent.velocity == Vector3.zero && !peeing) {
        peeing = true;
        animator.Play(PEE);
        Invoke("PeeFalse", 5f);  // This will set peeing back to false after 5 seconds
        Instantiate(pee, peeLocation.position, pee.transform.rotation);
    }
}

    void ClickToMove() {
        if(!peeing){
            RaycastHit hit;
            if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100, clickableLayers)){
                if(hit.transform.CompareTag("Interactable")) {
                    target = hit.transform.GetComponent<Interactable>();
                } else {
                    target = null;

                    agent.destination = hit.point;
                    if(clickEffect != null) {
                        Instantiate(clickEffect, hit.point += new Vector3(0, 0.1f, 0), clickEffect.transform.rotation);
                    }
                }   
            }
        }
    }

    void OnEnable() {
        input.Enable();
    }

    void OnDisable() {
        input.Disable();
    }

    void Update() {
        FollowTarget();
        FaceTarget();
        SetAnimations();
    }

    void FollowTarget() {
        if(target == null) return;

        if(Vector3.Distance(target.transform.position, transform.position) <= attackDistance) {
            ReachDistance();
        } else {
            agent.SetDestination(target.transform.position);
        }
    }

    void ReachDistance() {
        agent.SetDestination(transform.position);

        if(playerBusy) return;
        playerBusy = true;

        switch(target.interactionType){
            case InteractableType.Enemy:
                animator.Play("Attack");

                Invoke(nameof(SendAttack), attackDelay);
                Invoke(nameof(ResetBusyState), attackSpeed);
                break;
            case InteractableType.Item:
                target.InteractWithItem();
                target = null;

                Invoke(nameof(ResetBusyState), 0.5f);
                break;
        }
    }
    
    void SendAttack() {
        if(target == null) return;

        if(target.myActor.currentHealth <= 0) {
            target = null;
            return;
        }

        Instantiate(hitEffect, target.transform.position + new Vector3(0, 1, 0), Quaternion.identity);
        target.GetComponent<Actor>().TakeDamage(attackDamage);
    }
    
    void ResetBusyState() {
        playerBusy = false;
        SetAnimations();
    }

    void PeeFalse() {
        peeing = false;
    }

    void FaceTarget() {
        if(agent.destination == transform.position) return;

        Vector3 facing = Vector3.zero;
        if(target != null) {
            facing = target.transform.position; 
        } else {
            facing = agent.destination;
        }

        Vector3 direction = (facing - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * lookRotationSpeed);
    }

    void SetAnimations() {
        if(playerBusy) return;

        if(agent.velocity == Vector3.zero && !peeing) {
            animator.Play(IDLE);
        } else if (!peeing){
            animator.Play(WALK);
        }
    }
}