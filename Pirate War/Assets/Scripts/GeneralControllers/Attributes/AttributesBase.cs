using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Animations;
using TMPro;

public class AttributesBase : MonoBehaviour
{
    [Space(15)]
    [Header(" --------------------------------- Attributes Configs")]
    [Space(15)]

    [Range(0.1f, 1000)]
    public float maxLife;

    [Space(5)]

    [Range(0, 10)]
    public float speed = 5;

    [Space(5)]

    [Range(1, 10)]
    public float speedRotation = 2;

    [Space(5)]

    [Range(0.1f, 100)]
    public float damage;

    [Space(5)] // unused

    [Range(0.1f, 100)]
    [HideInInspector]
    public float resist;

    [Space(15)]
    [Header(" --------------------------------- Cooldown Configs")]
    [Space(15)]

    [Header("Cooldowns config")]
    [Header("Element 0 Canceled / 1 front / 2 left / 3 right / 4 back / 5 neutral")]
    [Range(0, 10)]
    public float[] cooldowns = new float[5];
    
    public float[] timerCooldown = new float[5];


    [Space(10)]
    [Header("Aim Cooldown")]

    public Image fillAim;
    public Image backgroundAim;
    public AimDirectionAndFire directionFire;
    public TMP_Text textCooldownFill;

    [Space(10)]
    [Header("Colors cooldown")]
    public Color colorLoading;
    public Color colorDefault;
    public Color colorComplete;

    [Space(10)]
    CamController camController;

    [Space(15)]
    [Header(" --------------------------------- LifeBar")]
    [Space(15)]

    [Header("Life Bar")]
    public GameObject prefabBar;
    GameObject instantiateLifeBar;

    [Space(10)]
    [Header("ValueDmg")]
    public GameObject numbersDmgFX;

    PositionConstraint constraint;
    ConstraintSource myConstraintSource;

    CanvasGroup alphaBar;

    Image fillBar;
    Image secondFillBar;
    float valueLifeBar;

    //Effects
    ParticleSystem particle;
    FadeLight LightEffectBar;

    [Space(15)]
    [Header(" --------------------------------- Edit Sprites")]
    [Space(15)]

    [Space(10)]
    [Header("Set Sprites Ship")]

    public bool autoGenerateSprites;

    TypeShips selectSpritesShip; // script in EventSystem

    [Space(10)]
    [Header("Ships")]
    public Sprite[] shipSprite;

    [Space(10)]
    [Header("Flags")]
    public Sprite[] sailLargeSprite;
    public Sprite[] sailSmallSprite;
    public Sprite[] flagSprite;

    [Space(10)]
    [Header("SPRITES SHIP")]
    [Space(7)]

    [Header("Please drop GameObjects with Sprite Renderer here")]

    public SpriteRenderer hull;
    public SpriteRenderer sailLarge;
    public SpriteRenderer sailSmall;
    public SpriteRenderer flag;

    [Space(15)]
    [Header(" --------------------------------- Dead")]
    [Space(15)]

    bool isDead = false;

    [Header("Active system submerge")]
    public Submerge[] scriptSubmerge;

    [Space(10)]
    [Header("objects to destroy when you die")]
    public GameObject[] destroyWhenYouDie;

    [Space(10)]
    [Header("objects instantiate when you die")]
    public GameObject[] instatiateWhenYouDie;

    void Start()
    {
        camController = GameObject.FindGameObjectWithTag("Player").GetComponent<CamController>();

        // ----------------------------------------------------------------------------------------------

        for (int i = 1; i < cooldowns.Length; i++)
        {
            timerCooldown[i] = cooldowns[i];
        }

        // ----------------------------------------------------------------------------------------------
        instantiateLifeBar = Instantiate(prefabBar) as GameObject;

        constraint = instantiateLifeBar.GetComponent<PositionConstraint>();
        myConstraintSource.sourceTransform = transform;
        myConstraintSource.weight = 1f;
        constraint.AddSource(myConstraintSource);

        alphaBar = instantiateLifeBar.GetComponent<CanvasGroup>();

        fillBar = instantiateLifeBar.transform.GetChild(2).GetComponent<Image>();
        secondFillBar = instantiateLifeBar.transform.GetChild(1).GetComponent<Image>();
        valueLifeBar = maxLife;

        particle = instantiateLifeBar.GetComponent<ParticleSystem>();
        LightEffectBar = instantiateLifeBar.GetComponent<FadeLight>();

        if (gameObject.tag == "Player")
        {
            fillBar.color = Color.green;
        }
        else
        {
            fillBar.color = Color.red;
        }

        // ----------------------------------------------------------------------------------------------

        selectSpritesShip = GameObject.FindGameObjectWithTag("EventSystem").GetComponent<TypeShips>();
        if (autoGenerateSprites)
        {
            selectSpritesShip.GenerateShip();

            shipSprite = selectSpritesShip.currentSelectedShip;
            sailLargeSprite = selectSpritesShip.currentSelectedBigSail;
            sailSmallSprite = selectSpritesShip.currentSelectedSmallSail;
            flagSprite = selectSpritesShip.currentSelectedFlag;

            hull.sprite = shipSprite[3];
            sailLarge.sprite = sailLargeSprite[3];
            sailSmall.sprite = sailSmallSprite[3];
            flag.sprite = flagSprite[3];
        }


    }

    // ----------------------------------------------------------------------------------------------

    public void applyDmg(float valueDmg)
    {
        if(isDead == false)
        {
            // --------------------- Sub dmg to life
            valueLifeBar -= valueDmg;
            fillBar.fillAmount = valueLifeBar / maxLife;

            // --------------------- Effect drop txt valueDmg
            Vector3 randomX = new Vector3(Random.Range(instantiateLifeBar.transform.position.x - 0.3f, instantiateLifeBar.transform.position.x + 0.3f), instantiateLifeBar.transform.position.y, instantiateLifeBar.transform.position.z);
            GameObject instValueDmg = Instantiate(numbersDmgFX, randomX, instantiateLifeBar.transform.rotation) as GameObject;
            instValueDmg.GetComponentInChildren<TMP_Text>().text = "-" + valueDmg;

            // --------------------- Visual effects
            alphaBar.alpha = 1;
            camController.ShakeCam();
            particle.Play();
            LightEffectBar.enabled = true;
            instantiateLifeBar.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);

            // --------------------- Ship deteoring
            float stageLife = fillBar.fillAmount * 3 + 0.99f;
            //Debug.Log(stageLife);
            hull.sprite = shipSprite[(int)stageLife];
            sailLarge.sprite = sailLargeSprite[(int)stageLife];
            sailSmall.sprite = sailSmallSprite[(int)stageLife];
            flag.sprite = flagSprite[(int)stageLife];

            // Debug.Log("life: " + valueLifeBar + " / Receive: " + valueDmg + " / Current life: " + fillBar.fillAmount * maxLife);
        }
    }

    // ----------------------------------------------------------------------------------------------

    private void FixedUpdate()
    {
        // --------------------- Lifebar

        if (instantiateLifeBar != null) 
        {
            if (alphaBar.alpha > 0.3f)
            {
                alphaBar.alpha -= Time.deltaTime * 0.3f;
            }

            if (fillBar.fillAmount < secondFillBar.fillAmount)
            {
                secondFillBar.fillAmount -= Time.deltaTime * 0.1f;
            }

            if (instantiateLifeBar.transform.localScale.x > 1)
            {
                instantiateLifeBar.transform.localScale -= new Vector3(Time.deltaTime, Time.deltaTime, Time.deltaTime);
            }
            else if (instantiateLifeBar.transform.localScale.x < 1)
            {
                instantiateLifeBar.transform.localScale = new Vector3(1, 1, 1);
            }
        }

        // --------------------- Cooldown

        if (timerCooldown[1] < cooldowns[1])
        {
            timerCooldown[1] += Time.deltaTime;
            backgroundAim.color = Color.red;

            textCooldownFill.color = Color.red;
        }
        else
        {
            timerCooldown[1] = cooldowns[1];
            backgroundAim.color = Color.white;

            textCooldownFill.color = Color.green;
        }

        if (timerCooldown[2] < cooldowns[2])
        {
            timerCooldown[2] += Time.deltaTime;
            backgroundAim.color = Color.red;

            textCooldownFill.color = Color.red;
        }
        else
        { 
            timerCooldown[2] = cooldowns[2];
            backgroundAim.color = Color.white;

            textCooldownFill.color = Color.green;
        }

        if (timerCooldown[3] < cooldowns[3])
        {
            timerCooldown[3] += Time.deltaTime;
            backgroundAim.color = Color.red;

            textCooldownFill.color = Color.red;
        }
        else 
        { 
            timerCooldown[3] = cooldowns[3];
            backgroundAim.color = Color.white;

            textCooldownFill.color = Color.green;
        }

        if (timerCooldown[4] < cooldowns[4])
        {
            timerCooldown[4] += Time.deltaTime;
            backgroundAim.color = Color.red;

            textCooldownFill.color = Color.red;
        } 
        else 
        {
            timerCooldown[4] = cooldowns[4];
            backgroundAim.color = Color.white;

            textCooldownFill.color = Color.green;
        }

        // --------------------- Dead
        if (fillBar.fillAmount <= 0 && isDead == false)         
        {
            isDead = true;
            StartCoroutine("Dead");
        }

        // --------------------- Cooldown

        fillAim.fillAmount = timerCooldown[directionFire.sideAim] / cooldowns[directionFire.sideAim];
        float x = fillAim.fillAmount * 100;
        textCooldownFill.text = (int)x + "%";

        if (timerCooldown[directionFire.sideAim] == cooldowns[directionFire.sideAim])
        {

        }
    }

    IEnumerator Dead()
    {
        if (instatiateWhenYouDie.Length != 0)
            foreach (GameObject go in instatiateWhenYouDie)
            {
                GameObject instance = Instantiate(go, transform.position, transform.rotation) as GameObject;
            }

        if (destroyWhenYouDie.Length != 0)
            foreach (GameObject go in destroyWhenYouDie)
            {
                Destroy(go);
            }

        if (scriptSubmerge.Length != 0)
            foreach (Submerge script in scriptSubmerge)
            {
                script.enabled = true;
            }

        yield return new WaitForSeconds(4);

        Destroy(instantiateLifeBar);
    }
}
