using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWeaponTrajectory : MonoBehaviour
{
    [Header("the number represents: 1 front / 2 left / 3 right / 4 back")]
    [Range(1,4)]
    public int sideWeapon = 1;
    public GameObject mainAim;
}