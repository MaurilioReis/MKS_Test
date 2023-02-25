using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveAi : MonoBehaviour
{
    [Header(" --------------------------------- Set Enemy Type")]
    [Space(15)]

    [Header("1 Shooter, 2 kamikaze")]
    [Range (1,2)]
    public int typeBehavior; // 1 Shooter, 2 kamikaze

    [Space(15)]
    [Header(" --------------------------------- Dir orientation")]
    [Space(15)]


    public Transform frontDir;
    [HideInInspector]
    public bool sensor_Col_Front;

    public Transform frontRDir;
    [HideInInspector]
    public bool sensor_Col_Front_R;

    public Transform frontLDir;
    [HideInInspector]
    public bool sensor_Col_Front_L;

    public Transform RDir;
    [HideInInspector]
    public bool sensor_Col_R;

    public Transform LDir;
    [HideInInspector]
    public bool sensor_Col_L;

    [HideInInspector]
    public bool sideEnemyL;
    [HideInInspector]
    public bool sideEnemyR;

    [HideInInspector]
    public int lastSide;

    [HideInInspector]
    public bool keepDistance;
    [HideInInspector]
    public float distanceArea;

    [HideInInspector]
    public float distance;

    // Space to atack

    [HideInInspector]
    public bool freeToAtack = false;
    [HideInInspector]
    public bool waitingToAtack = false;

    [Space(10)]
    [Header("Parameters Orientation")]
    public AimDirectionAndFire scriptAimDirection;
    public GameObject verifySide;

    Transform player;
    Rigidbody2D rb;
    AttributesBase attB;

    [Space(10)]
    float range = 1;
    public LayerMask colMask;
    public LayerMask colMaskOnlyPlayer;
    bool frontPlayer;
    private float speedRot = 300;

    public SystemGame sg;
    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        sg = GameObject.FindGameObjectWithTag("EventSystem").GetComponent<SystemGame>();
        if (sg.inGame == true)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }   

        if (player == null)
        {
            Destroy(gameObject);
        }

        attB = gameObject.GetComponent<AttributesBase>();

        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (player != null)
        {
            distance = Vector2.Distance(transform.position, player.position);
        }
        else
        {
            Initialize();
        }

        if (attB != null)
        {
            speedRot = attB.speedRotation * 30;
        }
        else
        {
            Initialize();
        }

        // ---------------------------------------------------------------------------------------------- CHECK PLAYER

        RaycastHit2D hitPlayer = Physics2D.Raycast(transform.position, verifySide.transform.right, float.PositiveInfinity, colMaskOnlyPlayer);
        if (hitPlayer) // Front Player
        {
            frontPlayer = true;
        }
        else
        {
            frontPlayer = false;
        }

        // ---------------------------------------------------------------------------------------------- CHECK AROUND

        RaycastHit2D hitFrontDir = Physics2D.Raycast(transform.position, frontDir.up, range, colMask);
        if(hitFrontDir) // front
        {
            sensor_Col_Front = true;
        }
        else
        {
            sensor_Col_Front = false;
        }

        RaycastHit2D hitFrontRDir = Physics2D.Raycast(transform.position, frontRDir.up, range, colMask);
        if (hitFrontRDir) // front R
        {
            sensor_Col_Front_R = true;
        }
        else
        {
            sensor_Col_Front_R = false;
        }

        RaycastHit2D hitFrontLDir = Physics2D.Raycast(transform.position, frontLDir.up, range, colMask);
        if (hitFrontLDir) // front L
        {
            sensor_Col_Front_L = true;
        }
        else
        {
            sensor_Col_Front_L = false;
        }

        RaycastHit2D hitRDir = Physics2D.Raycast(transform.position, RDir.up, range, colMask);
        if (hitRDir) // R
        {
            sensor_Col_R = true;
        }
        else
        {
            sensor_Col_R = false;
        }

        RaycastHit2D hitLDir = Physics2D.Raycast(transform.position, LDir.up, range, colMask);
        if (hitLDir) // L
        {
            sensor_Col_L = true;
        }
        else
        {
            sensor_Col_L = false;
        }

        // ---------------------------------------------------------------------------------------------- MOVES

        if(waitingToAtack == true && keepDistance == false)
        {
            if(distance < distanceArea)
            {
                if (scriptAimDirection.sideAim != 4)
                {
                    keepDistance = false;

                    if (scriptAimDirection.sideRL == "R") // Front R
                    {
                        rb.velocity = transform.right * attB.speed / 10;

                        transform.Rotate(Vector3.forward * Time.deltaTime * speedRot / 3);
                    }
                    else
                    {
                        rb.velocity = transform.right * attB.speed / 10;

                        transform.Rotate(Vector3.forward * -Time.deltaTime * speedRot / 3);
                    }
                }
                else
                {
                    keepDistance = true;
                }
            }
        }
        else if (scriptAimDirection.sideAim == 1 || (frontPlayer == true && freeToAtack == true && typeBehavior != 2) || keepDistance == true) // Aim front or in Atack or in keepDistance
            {
                if (scriptAimDirection.sideRL == "R") // Front R
                {
                    if (sensor_Col_L == true && sensor_Col_Front_R == false && sensor_Col_R == false)
                    {
                        rb.velocity = transform.right * attB.speed / 10;

                        transform.Rotate(Vector3.forward * -Time.deltaTime * speedRot / 3);
                    }
                    else if (sensor_Col_R == true && sensor_Col_Front_L == false && sensor_Col_L == false)
                    {
                        rb.velocity = transform.right * attB.speed / 10;

                        transform.Rotate(Vector3.forward * Time.deltaTime * speedRot / 3);
                    }
                    else if (sensor_Col_Front == false)
                    {
                        rb.velocity = transform.right * attB.speed / 5;

                        if (sensor_Col_Front_L == true)
                        {
                            transform.Rotate(Vector3.forward * -Time.deltaTime * speedRot / 2);
                        }
                        else if (sensor_Col_Front_R == true)
                        {
                            transform.Rotate(Vector3.forward * Time.deltaTime * speedRot / 2);
                        }
                    }
                    else if (sensor_Col_Front_R == false)
                    {
                        rb.velocity = transform.right * attB.speed / 10;

                        transform.Rotate(Vector3.forward * -Time.deltaTime * speedRot);
                    }
                    else if (sensor_Col_R == false && sensor_Col_Front_L == true)
                    {
                        lastSide = 1;
                        transform.Rotate(Vector3.forward * -Time.deltaTime * speedRot / 2);
                    }
                    else if (sensor_Col_Front_L == false)
                    {
                        rb.velocity = transform.right * attB.speed / 10;

                        transform.Rotate(Vector3.forward * Time.deltaTime * speedRot);
                    }
                    else if (sensor_Col_L == false)
                    {
                        lastSide = 0;
                        transform.Rotate(Vector3.forward * Time.deltaTime * speedRot / 2);
                    }
                    else
                    {
                        if (lastSide == 1)
                        {
                            transform.Rotate(Vector3.forward * -Time.deltaTime * speedRot);
                        }
                        else
                        {
                            transform.Rotate(Vector3.forward * Time.deltaTime * speedRot);
                        }
                    }
                }
                else // Front L
                {
                    if (sensor_Col_R == true && sensor_Col_Front_L == false && sensor_Col_L == false)
                    {
                        rb.velocity = transform.right * attB.speed / 10;

                        transform.Rotate(Vector3.forward * Time.deltaTime * speedRot / 3);
                    }
                    else if (sensor_Col_L == true && sensor_Col_Front_R == false && sensor_Col_R == false)
                    {
                        rb.velocity = transform.right * attB.speed / 10;

                        transform.Rotate(Vector3.forward * -Time.deltaTime * speedRot / 3);
                    }
                    if (sensor_Col_Front == false)
                    {
                        rb.velocity = transform.right * attB.speed / 5;

                        if (sensor_Col_Front_R == true)
                        {
                            transform.Rotate(Vector3.forward * Time.deltaTime * speedRot / 2);
                        }
                        else if (sensor_Col_Front_L == true)
                        {
                            transform.Rotate(Vector3.forward * -Time.deltaTime * speedRot / 2);
                        }
                    }
                    else if (sensor_Col_Front_L == false)
                    {
                        rb.velocity = transform.right * attB.speed / 10;

                        transform.Rotate(Vector3.forward * Time.deltaTime * speedRot);
                    }
                    else if (sensor_Col_L == false && sensor_Col_Front_R == true)
                    {
                        lastSide = 0;
                        transform.Rotate(Vector3.forward * Time.deltaTime * speedRot / 2);
                    }
                    else if (sensor_Col_Front_R == false)
                    {
                        rb.velocity = transform.right * attB.speed / 10;

                        transform.Rotate(Vector3.forward * -Time.deltaTime * speedRot);
                    }
                    else if (sensor_Col_R == false)
                    {
                        lastSide = 1;
                        transform.Rotate(Vector3.forward * -Time.deltaTime * speedRot / 2);
                    }
                    else
                    {
                        if (lastSide == 1)
                        {
                            transform.Rotate(Vector3.forward * -Time.deltaTime * speedRot);
                        }
                        else
                        {
                            transform.Rotate(Vector3.forward * Time.deltaTime * speedRot);
                        }
                    }
                }
            }
            else if (scriptAimDirection.sideAim == 3) // Aim Right
            {
                if (sensor_Col_Front_R == false)
                {
                    if (sensor_Col_Front == false)
                    {
                        rb.velocity = transform.right * attB.speed / 10; // move
                        transform.Rotate(Vector3.forward * -Time.deltaTime * speedRot / 3); // rot
                    }
                    else
                    {
                        transform.Rotate(Vector3.forward * -Time.deltaTime * speedRot / 2); // rot
                    }
                }
                else if(sensor_Col_R == false)
                {
                    lastSide = 1;
                    transform.Rotate(Vector3.forward * -Time.deltaTime * speedRot);
                }
                else if (sensor_Col_Front == false)
                {
                    rb.velocity = transform.right * attB.speed / 5; // move
                }
                else if (sensor_Col_Front_L == false)
                {
                    transform.Rotate(Vector3.forward * Time.deltaTime * speedRot / 2);
                }
                else if (sensor_Col_L == false)
                {
                    lastSide = 0;
                    transform.Rotate(Vector3.forward * Time.deltaTime * speedRot);
                }
                else
                {
                    if (lastSide == 1)
                    {
                        transform.Rotate(Vector3.forward * -Time.deltaTime * speedRot);
                    }
                    else
                    {
                        transform.Rotate(Vector3.forward * Time.deltaTime * speedRot);
                    }
                }

            }
            else if (scriptAimDirection.sideAim == 2) // Aim Left
            {
                if (sensor_Col_Front_L == false)
                {
                    if (sensor_Col_Front == false)
                    {
                        rb.velocity = transform.right * attB.speed / 10; // move
                    }

                    transform.Rotate(Vector3.forward * Time.deltaTime * speedRot / 2); // rot

                }
                else if (sensor_Col_L == false)
                {
                    lastSide = 0;
                    transform.Rotate(Vector3.forward * Time.deltaTime * speedRot);
                }
                else if (sensor_Col_Front == false)
                {
                    rb.velocity = transform.right * attB.speed / 5; // move
                }
                else if (sensor_Col_Front_R == false)
                {
                    transform.Rotate(Vector3.forward * -Time.deltaTime * speedRot / 2);
                }
                else if (sensor_Col_R == false)
                {
                    lastSide = 1;
                    transform.Rotate(Vector3.forward * -Time.deltaTime * speedRot);
                }
                else
                {
                    if (lastSide == 1)
                    {
                        transform.Rotate(Vector3.forward * -Time.deltaTime * speedRot);
                    }
                    else
                    {
                        transform.Rotate(Vector3.forward * Time.deltaTime * speedRot);
                    }
                }
            }
            else if (scriptAimDirection.sideAim == 4) // Aim Back
            {
                if (lastSide == 1)
                {
                    if (sensor_Col_Front_R == false)
                    {
                        if (sensor_Col_Front == false)
                        {
                            rb.velocity = transform.right * attB.speed / 10; // move
                        }
                        
                        transform.Rotate(Vector3.forward * -Time.deltaTime * speedRot / 2); // rot
                    }
                    else if (sensor_Col_R == false)
                    {
                        lastSide = 1;
                        transform.Rotate(Vector3.forward * -Time.deltaTime * speedRot);
                    }
                    else if (sensor_Col_Front == false)
                    {
                        rb.velocity = transform.right * attB.speed / 5; // move
                    }
                    else if (sensor_Col_Front_L == false)
                    {
                        transform.Rotate(Vector3.forward * Time.deltaTime * speedRot / 2);
                    }
                    else if (sensor_Col_L == false)
                    {
                        lastSide = 0;
                        transform.Rotate(Vector3.forward * Time.deltaTime * speedRot);
                    }
                    else
                    {
                        if (lastSide == 1)
                        {
                            transform.Rotate(Vector3.forward * -Time.deltaTime * speedRot);
                        }
                        else
                        {
                            transform.Rotate(Vector3.forward * Time.deltaTime * speedRot);
                        }
                    }

                }
                else
                {
                    if (sensor_Col_Front_L == false)
                    {
                        if (sensor_Col_Front == false)
                        {
                            rb.velocity = transform.right * attB.speed / 10; // move
                            transform.Rotate(Vector3.forward * Time.deltaTime * speedRot / 3); // rot
                        }
                        else
                        {
                            transform.Rotate(Vector3.forward * Time.deltaTime * speedRot / 2); // rot
                        }

                    }
                    else if (sensor_Col_L == false)
                    {
                        lastSide = 0;
                        transform.Rotate(Vector3.forward * Time.deltaTime * speedRot);
                    }
                    else if (sensor_Col_Front == false)
                    {
                        rb.velocity = transform.right * attB.speed / 5; // move
                    }
                    else if (sensor_Col_Front_R == false)
                    {
                        transform.Rotate(Vector3.forward * -Time.deltaTime * speedRot / 2);
                    }
                    else if (sensor_Col_R == false)
                    {
                        lastSide = 1;
                        transform.Rotate(Vector3.forward * -Time.deltaTime * speedRot);
                    }
                    else
                    {
                        if (lastSide == 1)
                        {
                            transform.Rotate(Vector3.forward * -Time.deltaTime * speedRot);
                        }
                        else
                        {
                            transform.Rotate(Vector3.forward * Time.deltaTime * speedRot);
                        }
                    }
                }
            }    
        
    }
}
