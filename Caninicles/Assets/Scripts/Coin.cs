using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private float value;
    private Transform player;
    private Money money;
    private Animation anim;


    public bool coinDeath = false;

    public GameObject coinEffect; // The particle effect to instantiate when the coin is collected

    // Parameters for floating effect
    private float originalY;
    public float floatStrength = 1f; // You can change this to increase the float range
    public float floatSpeed = 2f;    // You can change this to adjust the speed of floating

    public bool added = false;
    // Parameters for spinning
    public float spinSpeed = 180f; // Degrees per second

    void Start() {
        player = GameObject.Find("Player").GetComponent<Transform>();
        money = GameObject.Find("Player").GetComponent<Money>();
        anim = gameObject.GetComponent<Animation>();
        // Remember the starting position of the coin
        originalY = transform.position.y;
    }

    void Update() {
        // Perform the floating and spinning animations
        Float();
        Spin();

        // Check the distance to the player and collect the coin if close enough
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer < 4f) {
            if (!added){
                money.AddMoney(value);
            }
            added = true;
            if(coinEffect != null){
                Instantiate(coinEffect, transform.position, Quaternion.identity); // Instantiate at coin's position
            }

            anim.Play("CoinEnd");
        }
        if (coinDeath) {
            Destroy(gameObject);
        }
    }

    void Float() {
        // Calculate the new Y position using a sine wave
        float newY = originalY + (Mathf.Sin(Time.time * floatSpeed) * floatStrength);

        // Update the position of the coin
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    void Spin() {
        // Rotate around the up axis of the coin
        transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime);
    }
}
