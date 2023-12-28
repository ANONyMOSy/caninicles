using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroyParticleSystem : MonoBehaviour
{
    public float time = 5f;

    void Start()
    {
        Invoke("SelfDestruct", time);
    }

    void SelfDestruct() {
        Destroy(gameObject);
    }
}
