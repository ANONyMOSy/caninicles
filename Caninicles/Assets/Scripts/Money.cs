using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Money : MonoBehaviour
{
    private float balance = 0;

    public TextMeshProUGUI text;

    private void Update() {
        float rounded = (float)System.Math.Round(balance, 2); 
        text.text = "$" + rounded;
    }

    public void AddMoney(float amt) {
        balance += amt;
    }
}
