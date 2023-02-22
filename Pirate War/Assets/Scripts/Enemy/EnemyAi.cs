using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAi : MonoBehaviour
{

    [Space(15)]
    [Header(" --------------------------------- Dir orientation")]
    [Space(15)]

    bool front;
    public Transform frontDir;
    SpriteRenderer frontDirSprite;

    bool frontR;
    public Transform frontRDir;
    SpriteRenderer frontRDirSprite;

    bool frontL;
    public Transform frontLDir;
    SpriteRenderer frontLDirSprite;

    bool R;
    public Transform RDir;
    SpriteRenderer RDirSprite;

    bool L;
    public Transform LDir;
    SpriteRenderer LDirSprite;

    int lastSide;

    [Space(10)]
    public float range = 5;
    public LayerMask colMask;
    public LayerMask colMaskOnlyPlayer;
    bool frontPlayer;
    public float speedRot = 40;

    [Space(10)]
    public AimDirectionAndFire scriptAimDirection;
    public GameObject verifySide;
    bool loopVerifySide;

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

        loopVerifySide = true;

        StartCoroutine("LoopVerifySide");

        //scriptAimDirection = gameObject.GetComponent<AimDirectionAndFire>();
    }

    void Update()
    {
        float distance = Vector2.Distance(transform.position, player.position);

        // ----------------------------------------------------------------------------------------------

        RaycastHit2D hitPlayer = Physics2D.Raycast(transform.position, transform.right, float.PositiveInfinity, colMaskOnlyPlayer);
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


        RaycastHit2D hitPlayerSurrounding = Physics2D.Raycast(transform.position, transform.right, float.PositiveInfinity, colMaskOnlyPlayer);
        if (hitPlayerSurrounding) // Front Player
        {
            frontPlayer = true;
          //  Debug.DrawRay(transform.position, transform.right, rayCol, hitPlayerSurrounding.distance);
        }
        else
        {
            frontPlayer = false;
         //   Debug.DrawRay(transform.position, transform.right, rayFree, float.PositiveInfinity);
        }


        // ----------------------------------------------------------------------------------------------

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

        // ----------------------------------------------------------------------------------------------

        if(distance > 4) // all movimentations if hight distance to player
        {
            if(loopVerifySide == false)
            {
                loopVerifySide = true;
                StartCoroutine("LoopVerifySide");
            }

            if (scriptAimDirection.sideAim != 100) // front
            {
                if (scriptAimDirection.sideRL == "R") // Front R
                {
                    if (front == false)
                    {
                        rb.velocity = transform.right * attB.speed / 5;

                        if (frontL == true)
                        {
                            transform.Rotate(Vector3.forward * -Time.deltaTime * speedRot/2);
                        }
                        else if (frontR == true)
                        {
                            transform.Rotate(Vector3.forward * Time.deltaTime * speedRot/2);
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

            if (scriptAimDirection.sideAim == 3) // Right
            {

            }

            if (scriptAimDirection.sideAim == 2) // Left
            {

            }

            if (scriptAimDirection.sideAim == 4) // Back
            {
                //transform.Rotate(Vector3.forward * Time.deltaTime * speedRot);
            }

            if (front == true && frontR == true && frontL == true && R == true && L == true)
            {
               
            }

        }
        else if(loopVerifySide == true)
        {
            loopVerifySide = false;
            StopCoroutine("LoopVerifySide");
            verifySide.SetActive(true);
        }

        //if (frontPlayer == false) // rot right
        //{
        //    transform.Rotate(Vector3.forward * Time.deltaTime * speedRot);
        //}
    }

    IEnumerator LoopVerifySide()
    {
        verifySide.SetActive(true);
        yield return new WaitForSeconds(1);
        verifySide.SetActive(false);
        yield return new WaitForSeconds(1);

        StartCoroutine("LoopVerifySide");
    }
}
