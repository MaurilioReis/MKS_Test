using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Animations;
using TMPro;

public class AttributesBase : MonoBehaviour
{
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

    [Space(5)]

    [Range(0.1f, 100)]
    public float resist;

    [Space(5)]

    [Header("Cooldowns config")]
    [Header("Element 0 Canceled / 1 front / 2 left / 3 right / 4 back / 5 neutral")]
    [Range(0, 10)]
    public float[] cooldowns = new float[5];

    [Space(10)]
    CamController camController;

    [Space(10)]
    [Header("Life Bar")]
    public GameObject prefabBar;
    GameObject instantiateLifeBar;


    [Space(10)]
    [Header("ValueDmg")]
    public GameObject numbersDmg;

    PositionConstraint constraint;
    ConstraintSource myConstraintSource;

    CanvasGroup alphaBar;

    Image fillBar;
    Image secondFillBar;
    float valueLifeBar;

    ParticleSystem particle;
    FadeLight LightEffectBar;

    void Start()
    {
        camController = GameObject.FindGameObjectWithTag("Player").GetComponent<CamController>();

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
    }

    public void applyDmg(float valueDmg)
    {
        valueLifeBar -= valueDmg;
        fillBar.fillAmount = valueLifeBar / maxLife;

        Vector3 randomX = new Vector3(Random.Range(instantiateLifeBar.transform.position.x - 0.3f, instantiateLifeBar.transform.position.x + 0.3f), instantiateLifeBar.transform.position.y, instantiateLifeBar.transform.position.z);
        GameObject instValueDmg = Instantiate(numbersDmg, randomX, instantiateLifeBar.transform.rotation) as GameObject;
        instValueDmg.GetComponentInChildren<TMP_Text>().text = "-" + valueDmg;

        alphaBar.alpha = 1;
        camController.ShakeCam();
        particle.Play();
        LightEffectBar.enabled = true;
        instantiateLifeBar.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);

        // Debug.Log("life: " + valueLifeBar + " / Receive: " + valueDmg + " / Current life: " + fillBar.fillAmount * 500);
    }

    private void FixedUpdate()
    {
        if (alphaBar.alpha > 0.3f)
        {
            alphaBar.alpha -= Time.deltaTime * 0.3f;
        }

        if (fillBar.fillAmount < secondFillBar.fillAmount)
        {
            secondFillBar.fillAmount -= Time.deltaTime * 0.1f;
        }

        if(instantiateLifeBar.transform.localScale.x > 1)
        {
            instantiateLifeBar.transform.localScale -= new Vector3(Time.deltaTime, Time.deltaTime, Time.deltaTime);
        }
        else if (instantiateLifeBar.transform.localScale.x < 1)
        {
            instantiateLifeBar.transform.localScale = new Vector3(1, 1, 1);
        }
    }
}
