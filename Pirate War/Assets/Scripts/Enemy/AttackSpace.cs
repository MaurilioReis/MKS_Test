using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSpace : MonoBehaviour
{
    public List<GameObject> listEnemys;
    public int limitList = 5;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (listEnemys.Count < limitList)
        {
            if (collision.gameObject.tag == "Enemy")
            {
                collision.gameObject.GetComponent<EnemyMoveAi>().freeToAtack = true;
                listEnemys.Add(collision.gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<EnemyMoveAi>().freeToAtack = false;
            listEnemys.Remove(collision.gameObject);
        }
    }
}
