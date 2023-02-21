using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAi : MonoBehaviour
{

    [Space(15)]
    [Header(" --------------------------------- Dir orientation")]
    [Space(15)]

    public bool front;
    public Transform frontDir;
    bool frontR;
    public Transform frontRDir;
    bool frontL;
    public Transform frontLDir;
    bool R;
    public Transform RDir;
    bool L;
    public Transform LDir;

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
        player = GameObject.FindGameObjectWithTag("Player").transform;

        attB = player.GetComponent<AttributesBase>();

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
            Debug.DrawRay(transform.position, transform.right, rayCol, hitPlayer.distance);
        }
        else
        {
            frontPlayer = false;
            Debug.DrawRay(transform.position, transform.right, rayFree, float.PositiveInfinity);
        }


        RaycastHit2D hitPlayerSurrounding = Physics2D.Raycast(transform.position, transform.right, float.PositiveInfinity, colMaskOnlyPlayer);
        if (hitPlayerSurrounding) // Front Player
        {
            frontPlayer = true;
            Debug.DrawRay(transform.position, transform.right, rayCol, hitPlayerSurrounding.distance);
        }
        else
        {
            frontPlayer = false;
            Debug.DrawRay(transform.position, transform.right, rayFree, float.PositiveInfinity);
        }


        // ----------------------------------------------------------------------------------------------

        RaycastHit2D hitFrontDir = Physics2D.Raycast(frontDir.position, frontDir.up, range, colMask);
        if(hitFrontDir) // front
        {
            front = true;
            Debug.DrawRay(frontDir.position, frontDir.up, rayCol, hitFrontDir.distance);
        }
        else
        {
            front = false;
            Debug.DrawRay(frontDir.position, frontDir.up, rayFree, range);
        }

        RaycastHit2D hitFrontRDir = Physics2D.Raycast(frontRDir.position, frontRDir.up, range, colMask);
        if (hitFrontRDir) // front R
        {
            frontR = true;
            Debug.DrawRay(frontRDir.position, frontRDir.up, rayCol, hitFrontRDir.distance);
        }
        else
        {
            frontR = false;
            Debug.DrawRay(frontRDir.position, frontRDir.up, rayFree, range);
        }

        RaycastHit2D hitFrontLDir = Physics2D.Raycast(frontLDir.position, frontLDir.up, range, colMask);
        if (hitFrontLDir) // front L
        {
            frontL = true;
            Debug.DrawRay(frontLDir.position, frontLDir.up, rayCol, hitFrontLDir.distance);
        }
        else
        {
            frontL = false;
            Debug.DrawRay(frontLDir.position, frontLDir.up, rayFree, range);
        }

        RaycastHit2D hitRDir = Physics2D.Raycast(RDir.position, RDir.up, range, colMask);
        if (hitRDir) // R
        {
            R = true;
            Debug.DrawRay(RDir.position, RDir.up, rayCol, hitRDir.distance);
        }
        else
        {
            R = false;
            Debug.DrawRay(RDir.position, RDir.up, rayFree, range);
        }

        RaycastHit2D hitLDir = Physics2D.Raycast(LDir.position, LDir.up, range, colMask);
        if (hitLDir) // L
        {
            L = true;
            Debug.DrawRay(LDir.position, LDir.up, rayCol, hitLDir.distance);
        }
        else
        {
            L = false;
            Debug.DrawRay(LDir.position, LDir.up, rayFree, range);
        }

        // ----------------------------------------------------------------------------------------------

        if(distance > 3) // all movimentations if hight distance to player
        {
            if(loopVerifySide == false)
            {
                loopVerifySide = true;
                StartCoroutine("LoopVerifySide");
            }

            if (scriptAimDirection.sideAim == 1) // front
            {
                if (front == false)
                {
                    rb.AddForce(transform.right * attB.speed, ForceMode2D.Force);

                    if (frontR == true)
                    {
                        transform.Rotate(Vector3.forward * Time.deltaTime * speedRot);
                    }
                    else if (frontL == true)
                    {
                        transform.Rotate(Vector3.forward * -Time.deltaTime * speedRot);
                    }
                }
                else if (frontR == false)
                {
                    rb.AddForce(transform.right * attB.speed, ForceMode2D.Force);
                    transform.Rotate(Vector3.forward * Time.deltaTime * speedRot);
                }
                else if (R == false)
                {
                    rb.AddForce(transform.right * attB.speed, ForceMode2D.Force);
                    transform.Rotate(Vector3.forward * Time.deltaTime * speedRot / 2);
                }
                else
                {
                    transform.Rotate(Vector3.forward * Time.deltaTime * speedRot);
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
                transform.Rotate(Vector3.forward * Time.deltaTime * speedRot);
            }

            if(front && frontR && frontL && R && L)
            {
                transform.Rotate(Vector3.forward * Time.deltaTime * speedRot);
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
