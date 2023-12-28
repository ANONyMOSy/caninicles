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
        Debug.Log("Pee action performed");
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
                agent.destination = hit.point;
                if(clickEffect != null) {
                    Instantiate(clickEffect, hit.point += new Vector3(0, 0.1f, 0), clickEffect.transform.rotation);
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
        FaceTarget();
        SetAnimations();
    }

    void PeeFalse() {
        peeing = false;
    }

    void FaceTarget() {
        Vector3 direction = (agent.destination - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * lookRotationSpeed);
    }

    void SetAnimations() {
        if(agent.velocity == Vector3.zero && !peeing) {
            animator.Play(IDLE);
        } else if (!peeing){
            animator.Play(WALK);
        }
    }
}