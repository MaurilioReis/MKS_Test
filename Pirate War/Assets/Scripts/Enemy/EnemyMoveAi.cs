using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveAi : MonoBehaviour
{

    [Space(15)]
    [Header(" --------------------------------- Dir orientation")]
    [Space(15)]

    public bool sensor_Col_Front;
    public Transform frontDir;
    //SpriteRenderer frontDirSprite;

    public bool sensor_Col_Front_R;
    public Transform frontRDir;
    //SpriteRenderer frontRDirSprite;

    public bool sensor_Col_Front_L;
    public Transform frontLDir;
    //SpriteRenderer frontLDirSprite;

    public bool sensor_Col_R;
    public Transform RDir;
    //SpriteRenderer RDirSprite;

    public bool sensor_Col_L;
    public Transform LDir;
    //SpriteRenderer LDirSprite;

    public bool sideEnemyL;
    public bool sideEnemyR;

    public int lastSide;

    public bool keepDistance;

    public float distance;

    [Space(10)]
    [Header("Space to atack")]

    public bool freeToAtack = false;
    public bool waitingToAtack = false;

    [Space(10)]
    public float range = 5;
    public LayerMask colMask;
    public LayerMask colMaskOnlyPlayer;
    bool frontPlayer;
    public float speedRot = 40;

    [Space(10)]
    public AimDirectionAndFire scriptAimDirection;
    public GameObject verifySide;

    //[Space(10)]
    //[Header("Debug")]
    //public Color rayFree;
    //public Color rayCol;

    [Space(15)]
    [Header(" --------------------------------- Set behavior")]
    [Space(15)]
    [Header("0  running away, 1 advancing, 2 keep distance, 3 surrounding, 4 kamikaze")]

    public int typeBehavior; // 0  running away, 1 advancing, 2 keep distance, 3 surrounding, 4 kamikaze

    [Space(15)]
    [Header(" --------------------------------- Find/Dir player")]
    [Space(15)]

    Transform player;
    Rigidbody2D rb;
    AttributesBase attB;

    private void Start()
    {
        //frontDirSprite = frontDir.GetChild(0).GetComponent<SpriteRenderer>();
        //frontRDirSprite = frontRDir.GetChild(0).GetComponent<SpriteRenderer>();
        //frontLDirSprite = frontLDir.GetChild(0).GetComponent<SpriteRenderer>();
        //RDirSprite = RDir.GetChild(0).GetComponent<SpriteRenderer>();
        //LDirSprite = LDir.GetChild(0).GetComponent<SpriteRenderer>();

        player = GameObject.FindGameObjectWithTag("Player").transform;

        attB = gameObject.GetComponent<AttributesBase>();

        rb = gameObject.GetComponent<Rigidbody2D>();

        //scriptAimDirection = gameObject.GetComponent<AimDirectionAndFire>();
    }

    void Update()
    {
        distance = Vector2.Distance(transform.position, player.position);

        // ---------------------------------------------------------------------------------------------- CHECK PLAYER

        RaycastHit2D hitPlayer = Physics2D.Raycast(transform.position, verifySide.transform.right, float.PositiveInfinity, colMaskOnlyPlayer);
        if (hitPlayer) // Front Player
        {
            frontPlayer = true;
           // Debug.DrawRay(transform.position, transform.right, rayCol, hitPlayer.distance);
        }
        else
        {
            frontPlayer = false;
          //  Debug.DrawRay(transform.position, transform.right, rayFree, float.PositiveInfinity);
        }

        // ---------------------------------------------------------------------------------------------- CHECK AROUND

        RaycastHit2D hitFrontDir = Physics2D.Raycast(transform.position, frontDir.up, range, colMask);
        if(hitFrontDir) // front
        {
            sensor_Col_Front = true;

            //frontDirSprite.color = Color.red;
        }
        else
        {
            sensor_Col_Front = false;

            //frontDirSprite.color = Color.green;
        }

        RaycastHit2D hitFrontRDir = Physics2D.Raycast(transform.position, frontRDir.up, range, colMask);
        if (hitFrontRDir) // front R
        {
            sensor_Col_Front_R = true;

            //frontRDirSprite.color = Color.red;
        }
        else
        {
            sensor_Col_Front_R = false;

            //frontRDirSprite.color = Color.green;
        }

        RaycastHit2D hitFrontLDir = Physics2D.Raycast(transform.position, frontLDir.up, range, colMask);
        if (hitFrontLDir) // front L
        {
            sensor_Col_Front_L = true;

            //frontLDirSprite.color = Color.red;
        }
        else
        {
            sensor_Col_Front_L = false;

            //frontLDirSprite.color = Color.green;
        }

        RaycastHit2D hitRDir = Physics2D.Raycast(transform.position, RDir.up, range, colMask);
        if (hitRDir) // R
        {
            sensor_Col_R = true;

            //RDirSprite.color = Color.red;
        }
        else
        {
            sensor_Col_R = false;

            //RDirSprite.color = Color.green;
        }

        RaycastHit2D hitLDir = Physics2D.Raycast(transform.position, LDir.up, range, colMask);
        if (hitLDir) // L
        {
            sensor_Col_L = true;

            //LDirSprite.color = Color.red;
        }
        else
        {
            sensor_Col_L = false;

            //LDirSprite.color = Color.green;
        }

        // ---------------------------------------------------------------------------------------------- MOVES
        if(waitingToAtack == true)
        {
            if(distance < 5)
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
        else if (scriptAimDirection.sideAim == 1 || (frontPlayer == true && freeToAtack == true) || keepDistance == true) // Aim front or in Atack
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
