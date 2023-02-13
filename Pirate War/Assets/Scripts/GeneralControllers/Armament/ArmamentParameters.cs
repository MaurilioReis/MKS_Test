using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ArmamentParameters : MonoBehaviour
{

    public GameObject ammunition;
    public float maxDistance = 2;
    public Transform[] originsPositionsSpawns;
    
    [Space(10)]
    public Transform[] weaponsRotate;
    public float[] constraint;

    RotateWeaponTrajectory scriptFather;
    AimDirectionAndFire scriptTrajectoryAim;
    Transform mainAim;

    [HideInInspector] public bool inAtack = false;

    Quaternion resetRot;

    void Start()
    {
        resetRot = transform.localRotation;

        scriptFather = transform.parent.gameObject.GetComponent<RotateWeaponTrajectory>();
        mainAim = scriptFather.mainAim.transform;
        scriptTrajectoryAim = scriptFather.mainAim.GetComponent<AimDirectionAndFire>();
    }

    void Update()
    {
        if(scriptTrajectoryAim.sideAim == scriptFather.sideWeapon && scriptTrajectoryAim.sideAim > 0 && inAtack == false)
        {
            foreach (Transform tempTransform in weaponsRotate)
            {
                tempTransform.right = mainAim.position - tempTransform.position;
            }
        }
        else if (inAtack == false)
        {
            foreach (Transform tempTransform in weaponsRotate)
            {
                transform.localRotation = Quaternion.Slerp(transform.localRotation, resetRot, Time.deltaTime * 3);
            }
        }
    }
}
