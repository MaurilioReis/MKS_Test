using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSpace : MonoBehaviour
{
    public List<GameObject> listEnemys;
    public List<GameObject> waitList;

    [Header("Chooses how many enemies attack simultaneously")]
    public int limitList = 5;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (listEnemys.Count < limitList)
            {
                collision.gameObject.GetComponent<EnemyMoveAi>().freeToAtack = true;
                collision.gameObject.GetComponent<EnemyMoveAi>().keepDistance = false;
                listEnemys.Add(collision.gameObject);
            }
            else
            {
                collision.gameObject.GetComponent<EnemyMoveAi>().waitingToAtack = true;
                waitList.Add(collision.gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy") // remove enemy atack
        {
            collision.gameObject.GetComponent<EnemyMoveAi>().freeToAtack = false;
            listEnemys.Remove(collision.gameObject);

            if (listEnemys.Count < limitList && waitList.Count > 0) // alterne enemy atack
            {
                waitList[0].GetComponent<EnemyMoveAi>().waitingToAtack = false;
                waitList[0].GetComponent<EnemyMoveAi>().freeToAtack = true;
                waitList[0].GetComponent<EnemyMoveAi>().keepDistance = false;
                listEnemys.Add(waitList[0]);
                waitList.Remove(waitList[0]);
            }
            else // remove waitList
            {
                collision.gameObject.GetComponent<EnemyMoveAi>().waitingToAtack = false;
                waitList.Remove(collision.gameObject);
            }
        }
    }
}
