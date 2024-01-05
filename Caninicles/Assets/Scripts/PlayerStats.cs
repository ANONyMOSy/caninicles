using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [Header("Combat Stats")]
    public int physicalDamageLevel = 1;
    public int magicDamageLevel = 1;
    public int attackSpeedLevel = 1;

    PlayerController pc;

    void Awake() {
        pc = GetComponent<PlayerController>();
    }

    public void StatsLeveling(int level) {
        if (level % 3 == 1) {
            physicalDamageLevel++;
            magicDamageLevel++;
        } else if (level % 3 == 0) {
            magicDamageLevel++;
            attackSpeedLevel++;
        } else {
            physicalDamageLevel++;
            attackSpeedLevel++;
        }
        AdjustStats();
    }

    void AdjustStats() {
        pc.attackDamage = Mathf.RoundToInt(10f * Mathf.Pow(1.5f, (float)magicDamageLevel));
    }
}
