using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Counter : MonoBehaviour
{
    public int countDamage;
    public int countDeads;
    public int points;

    public TMP_Text textDamage;
    public TMP_Text textDeads;
    public TMP_Text textPoints;

    void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        countDamage = 0;
        countDeads = 0;
        points = 0;
    }

    public void CalculeFinalGame()
    {
        points = countDamage + countDeads * 500;
        textDamage.text = "DAMAGE: "+countDamage;
        textDeads.text = "DEADS: "+ countDeads;
        textPoints.text = "POINTS: "+ points;
    }
}
