using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class SystemGame : MonoBehaviour
{
    public bool inGame;

    [Space(10)]
    public GameObject prefabPlayer;

    GameObject playerScene;
    [HideInInspector]
    public GameObject lifeBarPlayer;



    [HideInInspector]
    public GameObject[] enemysInstance;

    [HideInInspector]
    public List<GameObject> barsInstance;

    [Space(10)]
    [Header("Main Menu Actives")]
    public GameObject[] objectsMenu;

    [Space(5)]
    [Header("-------------------------------------------------------")]

    [Space(10)]
    [Header("In Game Actives")]
    public GameObject[] objectsConfig;

    [Space(5)]
    [Header("-------------------------------------------------------")]

    [Space(10)]
    [Header("Config Game Actives")]
    public GameObject[] objectsInGame;
    [Space(5)]
    [Header("---")]
    public GameObject JoystickMove;
    public GameObject JoystickAim;
    public GameObject canvasControllers;
    [Space(5)]
    [Header("---")]
    public Timer timerGame;
    public Slider sliderTimer;
    public GameObject timerCanvas;
    [Space(5)]
    [Header("---")]
    public SpawnEnemy spawnEnemys;
    public Slider sliderTimeSpawns;
    public Slider sliderAmountSpawns;

    [Space(5)]
    [Header("-------------------------------------------------------")]

    [Space(10)]
    [Header("End Game Actives")]
    public GameObject[] objectsEndGame;

    [Space(5)]
    [Header("-------------------------------------------------------")]

    [Space(10)]
    [Header("Controller Cam")]
    public CinemachineVirtualCamera controllerCam;
    public Transform targetFree;

    [Space(5)]
    [Header("-------------------------------------------------------")]

    [Header("Main Menu = 0, In Game = 1,  Config Game = 2, End Game = 3  ")]
    [Range(0, 3)]
    public int controllerGame;

    private void Start()
    {
        playerScene = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (controllerGame == 0)
        {
            if (controllerCam.m_Lens.OrthographicSize < 100)
            {
                controllerCam.m_Lens.OrthographicSize += Time.deltaTime * 30;

                if (controllerCam.m_Lens.OrthographicSize > 50)
                {
                    controllerCam.m_Follow = targetFree;
                    controllerCam.m_LookAt = targetFree;
                }
            }
            else
            {
                foreach (GameObject obj in objectsMenu)
                {
                    obj.SetActive(true);
                }
            }
            // Menu
        }
        else if (controllerGame == 1 && inGame == true)
        {
            playerScene = GameObject.FindGameObjectWithTag("Player");

            if (controllerCam.m_Lens.OrthographicSize > 4)
            {
                controllerCam.m_Lens.OrthographicSize -= Time.deltaTime * 30;

                if (controllerCam.m_Follow != playerScene)
                {
                    controllerCam.m_Follow = playerScene.transform;
                    controllerCam.m_LookAt = playerScene.transform;
                }
            }
            // Start Game
        }
        else if (controllerGame == 2)
        {
            // Pause Game
        }
        else if (controllerGame == 3)
        {
            // End Game
        }
    }

    public void ButtonMainMenu()
    {
        enemysInstance = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject go in enemysInstance)
        {
            Destroy(go);
        }

        foreach (GameObject obj in objectsConfig)
        {
            obj.SetActive(false);
        }

        foreach (GameObject obj in objectsInGame)
        {
            obj.SetActive(false);
        }

        foreach (GameObject obj in objectsEndGame)
        {
            obj.SetActive(false);
        }

        spawnEnemys.StopAllCoroutines();
        controllerGame = 0;

        timerGame.enabled = false;

        spawnEnemys.enabled = false;
        canvasControllers.SetActive(false);
        JoystickMove.SetActive(false);
        JoystickAim.SetActive(false);
    }

    public void ButtonConfig()
    {
        foreach (GameObject obj in objectsInGame)
        {
            obj.SetActive(false);
        }

        foreach (GameObject obj in objectsEndGame)
        {
            obj.SetActive(false);
        }

        foreach (GameObject obj in objectsMenu)
        {
            obj.SetActive(false);
        }

        foreach (GameObject obj in objectsConfig)
        {
            obj.SetActive(true);
        }

        controllerGame = 2;
    }

    public void ButtonInGame()
    {

        enemysInstance = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject go in enemysInstance)
        {
            Destroy(go);
        }

        inGame = true;

        if (playerScene != null)
        {
            Destroy(playerScene);
            Destroy(lifeBarPlayer);
            playerScene = Instantiate(prefabPlayer, Vector3.zero, new Quaternion(0, 0, 0, 0));
        }
        else
        {
            playerScene = Instantiate(prefabPlayer, Vector3.zero, new Quaternion(0, 0, 0, 0));
        }

        foreach (GameObject obj in objectsEndGame)
        {
            obj.SetActive(false);
        }

        foreach (GameObject obj in objectsMenu)
        {
            obj.SetActive(false);
        }

        foreach (GameObject obj in objectsConfig)
        {
            obj.SetActive(false);
        }

        foreach (GameObject obj in objectsInGame)
        {
            obj.SetActive(true);
        }

        controllerGame = 1;

        // ------------------------
        
        timerGame.timer = sliderTimer.value;

        // ------------------------


        spawnEnemys.maxSpawn = (int)sliderAmountSpawns.value;
        spawnEnemys.maxTimeSpawn = (int)sliderTimeSpawns.value;
        spawnEnemys.minTimeSpawn = (int)sliderTimeSpawns.value;

        StartCoroutine("WaitInGame");
    }

    IEnumerator WaitInGame()
    {
        //Destroy(JoystickAim.GetComponent<JoystickAimAndFire>());
        JoystickAimAndFire scriptJoystickAim = JoystickAim.GetComponent<JoystickAimAndFire>();
        scriptJoystickAim.RotationVerifySides = playerScene.transform.Find("SidesTrigger/RotationVerifySides");
        scriptJoystickAim.aimDirectionAndFire = playerScene.transform.Find("SidesTrigger/RotationVerifySides/MainAim").GetComponent<AimDirectionAndFire>();

        yield return new WaitForSeconds(5);

        timerGame.enabled = true;
        spawnEnemys.enabled = true;
        canvasControllers.SetActive(true);
        JoystickMove.SetActive(true);
        JoystickAim.SetActive(true);

        JoystickMove.GetComponent<JoystickMove>().Initialize();
        spawnEnemys.GetComponent<SpawnEnemy>().Initialize();
        scriptJoystickAim.Initialize();

    }

    public void ButtonEndGame()
    {
        inGame = false;

        timerGame.enabled = false;
        spawnEnemys.enabled = false;
        canvasControllers.SetActive(false);
        JoystickMove.SetActive(false);
        JoystickAim.SetActive(false);

        foreach (GameObject obj in objectsMenu)
        {
            obj.SetActive(false);
        }

        foreach (GameObject obj in objectsConfig)
        {
            obj.SetActive(false);
        }

        foreach (GameObject obj in objectsInGame)
        {
            obj.SetActive(false);
        }

        foreach (GameObject obj in objectsEndGame)
        {
            obj.SetActive(true);
        }

        controllerGame = 3;

        enemysInstance = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject go in enemysInstance)
        {
            Destroy(go);
        }

        GameObject.FindGameObjectWithTag("EventSystem").GetComponent<Counter>().CalculeFinalGame();
    }
}
