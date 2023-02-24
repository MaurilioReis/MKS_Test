using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSpace : MonoBehaviour
{
    public List<GameObject> listEnemys;
    public List<GameObject> waitList;

    public float areaSize = 4;
    CircleCollider2D triggerArea;

    [Header("Chooses how many enemies attack simultaneously")]
    [Range(1,100)]
    public int limitList = 5;

    private void Start()
    {
        triggerArea = gameObject.GetComponent<CircleCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (listEnemys.Count < limitList)
            {
                EnemyMoveAi currentScript = collision.gameObject.GetComponent<EnemyMoveAi>();
                currentScript.freeToAtack = true;
                currentScript.keepDistance = false;
                listEnemys.Add(collision.gameObject);
            }
            else
            {
                EnemyMoveAi currentScript = collision.gameObject.GetComponent<EnemyMoveAi>();
                currentScript.waitingToAtack = true;
                currentScript.distanceArea = areaSize;
                currentScript.keepDistance = false;
                waitList.Add(collision.gameObject);

            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy") // remove enemy from listEnemys
        {
            collision.gameObject.GetComponent<EnemyMoveAi>().freeToAtack = false;
            listEnemys.Remove(collision.gameObject);

            if (listEnemys.Count < limitList && waitList.Count > 0) // choose the enemy to put on the listEnemys
            {
                EnemyMoveAi currentScript = waitList[0].GetComponent<EnemyMoveAi>();
                currentScript.waitingToAtack = false;
                currentScript.freeToAtack = true;
                currentScript.keepDistance = false;
                listEnemys.Add(waitList[0]);
                waitList.Remove(waitList[0]);
            }
            else // remove waitList
            {
                collision.gameObject.GetComponent<EnemyMoveAi>().waitingToAtack = false;
                collision.gameObject.GetComponent<EnemyMoveAi>().keepDistance = false;
                waitList.Remove(collision.gameObject);
            }
        }
    }

    private void Update()
    {
        triggerArea.radius = areaSize;

        if (listEnemys.Count > limitList) // choose the enemy to put on the WaitList
        {
            EnemyMoveAi currentScript = listEnemys[listEnemys.Count - 1].GetComponent<EnemyMoveAi>();
            currentScript.waitingToAtack = true;
            currentScript.distanceArea = areaSize;
            currentScript.keepDistance = false;
            waitList.Add(listEnemys[listEnemys.Count-1]);
            listEnemys.Remove(listEnemys[listEnemys.Count - 1]);
        }
        else if (listEnemys.Count < limitList && waitList.Count > 0) // choose the enemy to put on the listEnemys
        {
            EnemyMoveAi currentScript = waitList[waitList.Count-1].GetComponent<EnemyMoveAi>();
            currentScript.waitingToAtack = false;
            currentScript.freeToAtack = true;
            currentScript.keepDistance = false;
            listEnemys.Add(waitList[waitList.Count-1]);
            waitList.Remove(waitList[waitList.Count-1]);
        }
    }

    public void AutoRemovedFromLists (GameObject go)
    {
        waitList.Remove(go);
        listEnemys.Remove(go);
    }
}
