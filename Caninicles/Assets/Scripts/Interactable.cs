using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InteractableType { Enemy, Item };

public class Interactable : MonoBehaviour
{
    public Actor myActor { get; private set; }

    public InteractableType interactionType;

    public bool isMushroomPlant = false;

    public GameObject mushroomEnemy;

    void Awake() {
        if(interactionType == InteractableType.Enemy) {
            myActor = GetComponent<Actor>();
        }
    }

    public void InteractWithItem() {
        if(isMushroomPlant) {
            Instantiate(mushroomEnemy, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }

}
