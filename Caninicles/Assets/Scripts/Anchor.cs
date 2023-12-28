using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anchor : MonoBehaviour
{
    public Transform obj1;
    public Transform obj2;

    void Update() {
        obj2.position = obj1.position;
    }
}
