using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoystickAimAndFire : MonoBehaviour
{
    Touch touchJoystic;
    Vector2 startPositionTouch;
    RectTransform centerJoystick;

    Animator arcAnim;

    CanvasGroup alphaController;

    CamController controllerCam;

    [Header("Configurations Joystic")]
    [Header("Element 0 Canceled / 1 front / 2 left / 3 right / 4 back / 5 neutral ")]

    [Space(10)]
    public float reguleDistance = 200;
    
    [Space(10)]
    public Image icon;
    public Sprite[] iconsSprites;

    [Space(10)]
    public Image iconBackGround;
    public Sprite[] iconsSpritesBg;

    [Space (10)]
    [Header("Configurations path aim")]
    public Transform RotationVerifySides;
    public AimDirectionAndFire aimDirectionAndFire;


    void Start()
    {
        touchJoystic = new Touch { fingerId = -1 };

        centerJoystick = gameObject.transform.GetChild(2).GetComponent<RectTransform>();
        arcAnim = gameObject.transform.GetChild(1).GetComponent<Animator>();

        startPositionTouch = centerJoystick.position;

        alphaController = gameObject.GetComponent<CanvasGroup>();

        controllerCam = GameObject.FindGameObjectWithTag("Player").GetComponent<CamController>();
    }


    void Update()
    {
        if(Input.touchCount > 0)
        {
            for(int numberTouch = 0; numberTouch < Input.touchCount; numberTouch++ )
            {

                if(touchJoystic.fingerId == -1)
                {
                    // Register touch
                    if (Input.GetTouch(numberTouch).position.x > Screen.width / 2 && Input.GetTouch(numberTouch).position.y < Screen.height / 2)
                    {
                        touchJoystic = Input.GetTouch(numberTouch);

                        alphaController.alpha = 1;
                        startPositionTouch = touchJoystic.position;
                        transform.position = touchJoystic.position;
                    }
                }
                else 
                { 
                    if (Input.GetTouch(numberTouch).fingerId == touchJoystic.fingerId)
                    {
                        touchJoystic = Input.GetTouch(numberTouch);
                    }
                }
            }

            if (touchJoystic.fingerId != -1)
            {
                // Drop
                if (touchJoystic.phase == TouchPhase.Canceled || touchJoystic.phase == TouchPhase.Ended)
                {
                    touchJoystic = new Touch { fingerId = -1 };

                    icon.sprite = iconsSprites[0];
                    iconBackGround.sprite = iconsSpritesBg[0];

                    if(aimDirectionAndFire.triggerFire == true)
                    {
                        aimDirectionAndFire.StartCoroutine("Fire");
                        aimDirectionAndFire.triggerFire = false;
                    }

                    aimDirectionAndFire.lockAim = 0;

                    arcAnim.SetInteger("InsideOutside", 0);

                    controllerCam.SetNewLookAt(0);
                }
                else // Movement
                {

                    Vector2 direction = touchJoystic.position - startPositionTouch;
                    centerJoystick.position = startPositionTouch + Vector2.ClampMagnitude(direction, reguleDistance);

                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                    Vector2 distanceMoveJoystick = (Vector2)centerJoystick.position / reguleDistance - (Vector2)gameObject.transform.position / reguleDistance;

                    RotationVerifySides.transform.rotation = Quaternion.Slerp(RotationVerifySides.transform.rotation, Quaternion.Euler(0, 0, angle), 100 * Time.deltaTime);

                    if (distanceMoveJoystick.magnitude < 0.75f) // verify distance Joystick
                    {
                        iconBackGround.sprite = iconsSpritesBg[0];

                        aimDirectionAndFire.triggerFire = false;
                        aimDirectionAndFire.lockAim = 5;

                        arcAnim.SetInteger("InsideOutside", 1);
                    }
                    else if (aimDirectionAndFire.inAtack == false)
                    {
                        iconBackGround.sprite = iconsSpritesBg[aimDirectionAndFire.sideAim];

                        aimDirectionAndFire.triggerFire = true;

                        arcAnim.SetInteger("InsideOutside", 2);
                    }

                    icon.sprite = iconsSprites[aimDirectionAndFire.sideAim];
                    controllerCam.SetNewLookAt(1);

                }
            }
        }

        //unselected
        if (alphaController.alpha > 0 && touchJoystic.fingerId == -1)
        {
            centerJoystick.position = Vector2.MoveTowards(centerJoystick.position, startPositionTouch, 10);
            alphaController.alpha -= Time.deltaTime;
        }
    }
}