using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    const string IDLE = "Idle";
    const string WALK = "Walk";

    CustomActions input;

    NavMeshAgent agent;
    Animator animator;

    [Header("Movement")] 
    [SerializeField] ParticleSystem clickEffect;
    [SerializeField] LayerMask clickableLayers;

    float lookRotationSpeed = 8f;

    void Awake() {
        agent = GetComponent<NavMeshAgent>();
    }
}
