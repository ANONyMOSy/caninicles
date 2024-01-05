using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCharacter : MonoBehaviour
{
    public Transform player;
    public GameObject questionMarkPrefab; // Assign yellow question mark in the inspector
    public GameObject exclamationMarkPrefab; // Assign red exclamation mark in the inspector
    public Actor playerActor; // Assign the player's Actor script in the inspector

    [SerializeField] private float range = 10f;
    private GameObject instantiatedSymbol; // Can be a question mark or an exclamation mark
    private bool isPlayerInRange = false; // Tracks if the player is in range
    private bool isInCombat = false; // Tracks the combat state

    void Awake() {
        // Find and assign the Player's transform and Actor script
        player = GameObject.Find("Player").transform;
        playerActor = GetComponent<Actor>(); // Ensure the Player has an Actor script attached
    }

    void LateUpdate() {
        // Calculate the distance between this object and the player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if(playerActor.currentHealth <= 0) {
            Destroy(instantiatedSymbol);
            instantiatedSymbol = null;
        }
        // Check if the player is within range
        if (distanceToPlayer <= range) {
            if (!isPlayerInRange) {
                // Player just entered the range
                isPlayerInRange = true;
                isInCombat = playerActor.inCombat; // Update the combat state
                InstantiateSymbol(isInCombat); // Instantiate the initial symbol
            } else if (playerActor.inCombat != isInCombat) {
                // Player's combat state has changed
                isInCombat = playerActor.inCombat;
                // Stop any existing animations and start the transition
                StopAllCoroutines();
                StartCoroutine(TransitionSymbol(isInCombat));
            }

            // Make the object face the player
            transform.LookAt(player);
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);

        } else {
            // If the player is out of range and a symbol is active, destroy it
            if (isPlayerInRange) {
                isPlayerInRange = false;
                StopAllCoroutines();
                if (instantiatedSymbol != null) {
                    Destroy(instantiatedSymbol);
                    instantiatedSymbol = null;
                }
            }
        }
    }

    private void InstantiateSymbol(bool inCombat) {
        // Instantiate the appropriate symbol based on the combat state, with an upward offset
        GameObject prefabToInstantiate = inCombat ? exclamationMarkPrefab : questionMarkPrefab;
        instantiatedSymbol = Instantiate(prefabToInstantiate, transform.position + new Vector3(0, 3.5f, 0), Quaternion.identity);
        // Start the symbol's upward animation
        StartCoroutine(AnimateSymbol(instantiatedSymbol, true));
    }

    IEnumerator TransitionSymbol(bool toCombat) {
        // Animate the existing symbol moving down into the enemy
        yield return StartCoroutine(AnimateSymbol(instantiatedSymbol, false));
        Destroy(instantiatedSymbol);

        // Instantiate and animate the new symbol
        InstantiateSymbol(toCombat);
    }

    IEnumerator AnimateSymbol(GameObject symbol, bool movingUp) {
        float timeToAnimate = 0.25f; // Half duration of the animation since it's a two-part animation
        Vector3 startPosition = symbol.transform.position;
        Vector3 endPosition = movingUp ? startPosition + new Vector3(0, 3.5f, 0) : startPosition - new Vector3(0, 3.5f, 0);

        for (float t = 0; t < 1f; t += Time.deltaTime / timeToAnimate) {
            symbol.transform.position = Vector3.Lerp(startPosition, endPosition, t);
            yield return null;
        }

        symbol.transform.position = endPosition; // Ensure it ends exactly at the end position
    }
}
