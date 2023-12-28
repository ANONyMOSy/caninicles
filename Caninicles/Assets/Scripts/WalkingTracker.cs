using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WalkingTracker : MonoBehaviour
{
    public TextMeshProUGUI dw;

    private Vector3 lastPosition;
    private float totalDistanceTraveled;
    private Leveling level;
    private float total = 0;

    void Start() {
        lastPosition = transform.position;
        level = GetComponent<Leveling>();
    }

    void Update() {
        float distanceMoved = Vector3.Distance(transform.position, lastPosition);
        totalDistanceTraveled += distanceMoved;
        lastPosition = transform.position;
        total += distanceMoved;
        if (total >= 1) {
            total -= 1;
            level.AddExperience(1);
        }

        UpdateCounter();
    }


    void UpdateCounter() {
        dw.text = "Distance Walked: " + Mathf.Round(totalDistanceTraveled);
    }

}
