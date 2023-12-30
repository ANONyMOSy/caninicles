using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StatusEffects : MonoBehaviour
{
    // Define an enum for different types of status effects
    public enum EffectType { Poison /*, other effects... */ }
    private PlayerHealth ph;

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
    }

    // List to keep track of all active effects
    private List<EffectData> activeEffects = new List<EffectData>();

    void Awake() 
    {
        ph = GameObject.Find("Player").GetComponent<PlayerHealth>();
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
        activeEffects.Add(new EffectData(type, level, duration));
    }

    // Method to remove an effect
    private void RemoveEffect(EffectData effect)
    {
        Debug.Log(effect.type.ToString() + " has worn off from " + gameObject.name);
        // Add any cleanup or removal logic here
    }
}
