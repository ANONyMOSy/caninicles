using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    private Transform cam;

    void Awake() {
        cam = GameObject.Find("Main Camera").transform;
    }

    void LateUpdate() {
        transform.LookAt(cam);
    }
}
