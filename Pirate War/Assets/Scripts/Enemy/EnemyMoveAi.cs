using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveAi : MonoBehaviour
{

    [Space(15)]
    [Header(" --------------------------------- Dir orientation")]
    [Space(15)]

    public bool front;
    public Transform frontDir;
    SpriteRenderer frontDirSprite;

    public bool frontR;
    public Transform frontRDir;
    SpriteRenderer frontRDirSprite;

    public bool frontL;
    public Transform frontLDir;
    SpriteRenderer frontLDirSprite;

    public bool R;
    public Transform RDir;
    SpriteRenderer RDirSprite;

    public bool L;
    public Transform LDir;
    SpriteRenderer LDirSprite;

    public bool sideEnemyL;
    public bool sideEnemyR;

    public int lastSide;
    public int sideAim;

    [Space(10)]
    [Header("Space to atack")]

    public bool freeToAtack = false;

    [Space(10)]
    public float range = 5;
    public LayerMask colMask;
    public LayerMask colMaskOnlyPlayer;
    bool frontPlayer;
    public float speedRot = 40;

    [Space(10)]
    public AimDirectionAndFire scriptAimDirection;
    public GameObject verifySide;

    [Space(10)]
    [Header("Debug")]
    public Color rayFree;
    public Color rayCol;

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
        frontDirSprite = frontDir.GetChild(0).GetComponent<SpriteRenderer>();
        frontRDirSprite = frontRDir.GetChild(0).GetComponent<SpriteRenderer>();
        frontLDirSprite = frontLDir.GetChild(0).GetComponent<SpriteRenderer>();
        RDirSprite = RDir.GetChild(0).GetComponent<SpriteRenderer>();
        LDirSprite = LDir.GetChild(0).GetComponent<SpriteRenderer>();

        player = GameObject.FindGameObjectWithTag("Player").transform;

        attB = gameObject.GetComponent<AttributesBase>();

        rb = gameObject.GetComponent<Rigidbody2D>();

        //scriptAimDirection = gameObject.GetComponent<AimDirectionAndFire>();
    }

    void Update()
    {
        sideAim = scriptAimDirection.sideAim;

        float distance = Vector2.Distance(transform.position, player.position);

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
            front = true;

            frontDirSprite.color = Color.red;
        }
        else
        {
            front = false;

            frontDirSprite.color = Color.green;
        }

        RaycastHit2D hitFrontRDir = Physics2D.Raycast(transform.position, frontRDir.up, range, colMask);
        if (hitFrontRDir) // front R
        {
            frontR = true;

            frontRDirSprite.color = Color.red;
        }
        else
        {
            frontR = false;

            frontRDirSprite.color = Color.green;
        }

        RaycastHit2D hitFrontLDir = Physics2D.Raycast(transform.position, frontLDir.up, range, colMask);
        if (hitFrontLDir) // front L
        {
            frontL = true;

            frontLDirSprite.color = Color.red;
        }
        else
        {
            frontL = false;

            frontLDirSprite.color = Color.green;
        }

        RaycastHit2D hitRDir = Physics2D.Raycast(transform.position, RDir.up, range, colMask);
        if (hitRDir) // R
        {
            R = true;

            RDirSprite.color = Color.red;
        }
        else
        {
            R = false;

            RDirSprite.color = Color.green;
        }

        RaycastHit2D hitLDir = Physics2D.Raycast(transform.position, LDir.up, range, colMask);
        if (hitLDir) // L
        {
            L = true;

            LDirSprite.color = Color.red;
        }
        else
        {
            L = false;

            LDirSprite.color = Color.green;
        }

        // ---------------------------------------------------------------------------------------------- MOVES

        //if() // mode Atack
        //{
            
        //}   aleatorios so pra testar
        if (scriptAimDirection.sideAim != 56498)// ------------------------------------------------------- all movimentations if hight distance to player
        {
            if (scriptAimDirection.sideAim == 1 || (frontPlayer == true && freeToAtack == true)) // Aim front or in Atack
            {
                if (scriptAimDirection.sideRL == "R") // Front R
                {
                    if (front == false)
                    {
                        rb.velocity = transform.right * attB.speed / 5;

                        if (frontL == true)
                        {
                            transform.Rotate(Vector3.forward * -Time.deltaTime * speedRot / 2);
                        }
                        else if (frontR == true)
                        {
                            transform.Rotate(Vector3.forward * Time.deltaTime * speedRot / 2);
                        }
                    }
                    else if (frontR == false)
                    {
                        rb.velocity = transform.right * attB.speed / 10;

                        transform.Rotate(Vector3.forward * -Time.deltaTime * speedRot);
                    }
                    else if (R == false && frontL == true)
                    {
                        lastSide = 1;
                        transform.Rotate(Vector3.forward * -Time.deltaTime * speedRot / 2);
                    }
                    else if (frontL == false)
                    {
                        rb.velocity = transform.right * attB.speed / 10;

                        transform.Rotate(Vector3.forward * Time.deltaTime * speedRot);
                    }
                    else if (L == false)
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
                    if (front == false)
                    {
                        rb.velocity = transform.right * attB.speed / 5;

                        if (frontR == true)
                        {
                            transform.Rotate(Vector3.forward * Time.deltaTime * speedRot / 2);
                        }
                        else if (frontL == true)
                        {
                            transform.Rotate(Vector3.forward * -Time.deltaTime * speedRot / 2);
                        }
                    }
                    else if (frontL == false)
                    {
                        rb.velocity = transform.right * attB.speed / 10;

                        transform.Rotate(Vector3.forward * Time.deltaTime * speedRot);
                    }
                    else if (L == false && frontR == true)
                    {
                        lastSide = 0;
                        transform.Rotate(Vector3.forward * Time.deltaTime * speedRot / 2);
                    }
                    else if (frontR == false)
                    {
                        rb.velocity = transform.right * attB.speed / 10;

                        transform.Rotate(Vector3.forward * -Time.deltaTime * speedRot);
                    }
                    else if (R == false)
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
                if (frontR == false)
                {
                    if (front == false)
                    {
                        rb.velocity = transform.right * attB.speed / 10; // move
                        transform.Rotate(Vector3.forward * -Time.deltaTime * speedRot / 3); // rot
                    }
                    else
                    {
                        transform.Rotate(Vector3.forward * -Time.deltaTime * speedRot / 2); // rot
                    }
                    
                }
                else if(R == false)
                {
                    lastSide = 1;
                    transform.Rotate(Vector3.forward * -Time.deltaTime * speedRot);
                }
                else if (front == false)
                {
                    rb.velocity = transform.right * attB.speed / 5; // move
                }
                else if (frontL == false)
                {
                    transform.Rotate(Vector3.forward * Time.deltaTime * speedRot / 2);
                }
                else if (L == false)
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
                if (frontL == false)
                {
                    if (front == false)
                    {
                        rb.velocity = transform.right * attB.speed / 10; // move
                    }

                    transform.Rotate(Vector3.forward * Time.deltaTime * speedRot / 2); // rot

                }
                else if (L == false)
                {
                    lastSide = 0;
                    transform.Rotate(Vector3.forward * Time.deltaTime * speedRot);
                }
                else if (front == false)
                {
                    rb.velocity = transform.right * attB.speed / 5; // move
                }
                else if (frontR == false)
                {
                    transform.Rotate(Vector3.forward * -Time.deltaTime * speedRot / 2);
                }
                else if (R == false)
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
                    if (frontR == false)
                    {
                        if (front == false)
                        {
                            rb.velocity = transform.right * attB.speed / 10; // move
                        }
                        
                        transform.Rotate(Vector3.forward * -Time.deltaTime * speedRot / 2); // rot
                    }
                    else if (R == false)
                    {
                        lastSide = 1;
                        transform.Rotate(Vector3.forward * -Time.deltaTime * speedRot);
                    }
                    else if (front == false)
                    {
                        rb.velocity = transform.right * attB.speed / 5; // move
                    }
                    else if (frontL == false)
                    {
                        transform.Rotate(Vector3.forward * Time.deltaTime * speedRot / 2);
                    }
                    else if (L == false)
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
                    if (frontL == false)
                    {
                        if (front == false)
                        {
                            rb.velocity = transform.right * attB.speed / 10; // move
                            transform.Rotate(Vector3.forward * Time.deltaTime * speedRot / 3); // rot
                        }
                        else
                        {
                            transform.Rotate(Vector3.forward * Time.deltaTime * speedRot / 2); // rot
                        }

                    }
                    else if (L == false)
                    {
                        lastSide = 0;
                        transform.Rotate(Vector3.forward * Time.deltaTime * speedRot);
                    }
                    else if (front == false)
                    {
                        rb.velocity = transform.right * attB.speed / 5; // move
                    }
                    else if (frontR == false)
                    {
                        transform.Rotate(Vector3.forward * -Time.deltaTime * speedRot / 2);
                    }
                    else if (R == false)
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
}
