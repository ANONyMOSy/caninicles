using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class StatusEffects : MonoBehaviour
{
    // Define an enum for different types of status effects
    public enum EffectType { Poison /*, other effects... */ }
    private PlayerHealth ph;

    public Image poisonIcon; // Reference to the UI Image for the poison icon
    public TextMeshProUGUI timerText; // Reference to the UI Text for the timer
    public Sprite[] poisonSprites; // Array of poison sprites for different levels

    // Structure to hold effect data
    private class EffectData
    {
        public EffectType type;
        public int level;
        public float duration;

        public EffectData(EffectType type, int level, float duration)
        {
            this.type = type;
            this.level = level;
            this.duration = duration;
        }
        public void UpdateEffect(int newLevel, float newDuration)
        {
            // Update level if the new level is higher
            if (newLevel > level) level = newLevel;

            // Update duration if the new duration is longer
            if (newDuration > duration) duration = newDuration;
        }
    }

    // List to keep track of all active effects
    private List<EffectData> activeEffects = new List<EffectData>();

    void Awake() 
    {
        ph = GameObject.Find("Player").GetComponent<PlayerHealth>();
        UpdateEffectUI(); // Initial UI update
    }
    // Update is called once per frame
    void Update()
    {
        // Iterate over active effects and update them
        for (int i = activeEffects.Count - 1; i >= 0; i--)
        {
            // Reduce duration
            activeEffects[i].duration -= Time.deltaTime;

            // Check if the effect has expired
            if (activeEffects[i].duration <= 0)
            {
                RemoveEffect(activeEffects[i]);
                activeEffects.RemoveAt(i);
            }
            else
            {
                // Apply effect based on type
                ApplyEffect(activeEffects[i]);
            }
        }

        foreach (var effect in activeEffects)
        {
            if (effect.type == EffectType.Poison)
            {
                timerText.text = effect.duration.ToString("F1") + "s"; // Update timer text, "F1" for 1 decimal place
                break; // Assuming you only care about the first (most potent) poison effect
            }
        }
    }

    // Method to apply an effect
    private void ApplyEffect(EffectData effect)
    {
        switch(effect.type)
        {
            case EffectType.Poison:
                ApplyPoison(effect.level);
                break;
            // Add cases for other effects here
        }
    }
    public float cooldown = 0f;
    // Poison effect logic
    private void ApplyPoison(int level)
    {
        // Implement poison logic based on level, e.g., damage over time
        if (level == 1)
        {
            if(cooldown <= 0){
                cooldown = 3f;
                ph.TakeDamage(1);
            }
        }
        else if (level == 2)
        {
            if(cooldown <= 0){
                cooldown = 1f;
                ph.TakeDamage(2);
            }
        }
        
        cooldown -= Time.deltaTime;
    }

    // Method to add a new effect
    public void AddEffect(EffectType type, int level, float duration)
    {
        // Check for an existing effect of the same type
        foreach (var existingEffect in activeEffects)
        {
            if (existingEffect.type == type)
            {
                // Update existing effect if new one is stronger or longer
                existingEffect.UpdateEffect(level, duration);
                return; // Exit method as effect is updated
            }
        }
        // If no existing effect is found, add the new one
        activeEffects.Add(new EffectData(type, level, duration));

        UpdateEffectUI();
    }

    // Method to remove an effect
    private void RemoveEffect(EffectData effect)
    {
        Debug.Log(effect.type.ToString() + " has worn off from " + gameObject.name);
        // Add any cleanup or removal logic here
        Invoke("UpdateEffectUI", 0.1f);
    }

    private void UpdateEffectUI()
    {
        EffectData mostPotentPoison = null;
        foreach (var effect in activeEffects)
        {
            if (effect.type == EffectType.Poison)
            {
                if (mostPotentPoison == null || effect.level > mostPotentPoison.level)
                {
                    mostPotentPoison = effect; // Find the most potent poison effect
                }
            }
        }

        if (mostPotentPoison != null)
        {
            poisonIcon.sprite = poisonSprites[mostPotentPoison.level - 1]; // -1 because array is zero-indexed
            poisonIcon.enabled = true; // Enable the icon if there's an active effect
        }
        else
        {
            poisonIcon.enabled = false; // Disable the icon if there are no active poison effects
            timerText.text = ""; // Clear the timer text
        }
    }
}
