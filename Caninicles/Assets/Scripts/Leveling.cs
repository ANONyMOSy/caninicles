using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Leveling : MonoBehaviour
{
    private int level = 1;
    private int experience = 0;
    private int experienceToNext = 100;

    public TextMeshProUGUI graphic;

    void Update() {
        UpdateGraphics();
        UpdateLevel();
    }

    void UpdateLevel() {
        if (experience >= experienceToNext) {
            level += 1;
            experience -= experienceToNext;
            UpdateExperienceToNext();
        }
    }

    void UpdateExperienceToNext() {
        // Cast the level to a float to ensure accurate division
        float levelAsFloat = (float)level;

        // Calculate the level contribution to experience needed
        float levelContribution = (levelAsFloat * 0.1f) + 1;

        // Multiply by 100 to scale the value
        float scaledContribution = levelContribution * 100;

        // Round the scaled contribution to the nearest whole number
        int roundedContribution = Mathf.RoundToInt(scaledContribution);

        // Add the rounded contribution to the total experience needed for the next level
        experienceToNext += roundedContribution;
    }

    void UpdateGraphics() {
        graphic.text = "Cologero - Lvl " + level + "\n\n" + experience + "/" + experienceToNext;
    }


    public void AddExperience(int exp) {
        experience += exp;
        Debug.Log(experience);
    }
}
