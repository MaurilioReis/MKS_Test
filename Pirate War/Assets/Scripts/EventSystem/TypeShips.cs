using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypeShips : MonoBehaviour
{
    [Header("Ships")]
    public Sprite[] ship1;
    public Sprite[] ship2;

    [Space(10)]
    [Header("BigSail")]
    public Sprite[] bigSailWhite;
    public Sprite[] bigSailBlack;
    public Sprite[] bigSailRed;
    public Sprite[] bigSailGreen;
    public Sprite[] bigSaiBlue;
    public Sprite[] bigSailYellow;

    [Space(10)]
    [Header("SmallSail")]
    public Sprite[] smallSailWhite;
    public Sprite[] smallSailBlack;
    public Sprite[] smallSailRed;
    public Sprite[] smallSailGreen;
    public Sprite[] smallSailBlue;
    public Sprite[] smallSailYellow;

    [Space(10)]
    [Header("Flag")]
    public Sprite[] flagWhite;
    public Sprite[] flagBlack;
    public Sprite[] flagRed;
    public Sprite[] flagGreen;
    public Sprite[] flagBlue;
    public Sprite[] flagYellow;

    [Space(10)]
    [Header("Current selection")]

    [HideInInspector]
    public Sprite[] currentSelectedShip;

    [HideInInspector]
    public Sprite[] currentSelectedBigSail;

    [HideInInspector]
    public Sprite[] currentSelectedSmallSail;

    [HideInInspector]
    public Sprite[] currentSelectedFlag;


    private void Start()
    {
        //GenerateShip();
    }
    public void GenerateShip()
    {
        int randomColorSelect = (int)Random.Range(1, 6.9f);

        if (randomColorSelect == 1)
        {
            currentSelectedBigSail = bigSailWhite;
            currentSelectedSmallSail = smallSailWhite;
            currentSelectedFlag = flagWhite;
        }
        else if (randomColorSelect == 2)
        {
            currentSelectedBigSail = bigSailBlack;
            currentSelectedSmallSail = smallSailBlack;
            currentSelectedFlag = flagBlack;
        }
        else if (randomColorSelect == 3)
        {
            currentSelectedBigSail = bigSailRed;
            currentSelectedSmallSail = smallSailRed;
            currentSelectedFlag = flagRed;
        }
        else if (randomColorSelect == 4)
        {
            currentSelectedBigSail = bigSailGreen;
            currentSelectedSmallSail = smallSailGreen;
            currentSelectedFlag = flagGreen;
        }
        else if (randomColorSelect == 5)
        {
            currentSelectedBigSail = bigSaiBlue;
            currentSelectedSmallSail = smallSailBlue;
            currentSelectedFlag = flagBlue;
        }
        else if (randomColorSelect == 6)
        {
            currentSelectedBigSail = bigSailYellow;
            currentSelectedSmallSail = smallSailYellow;
            currentSelectedFlag = flagYellow;
        }

        int randomShipSelect = (int)Random.Range(1, 2.9f);

        if (randomShipSelect == 1)
        {
            currentSelectedShip = ship1;
        }
        else
        {
            currentSelectedShip = ship2;
        }
    }
}
