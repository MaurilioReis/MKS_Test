using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionArea : MonoBehaviour
{
    public float damage;

    public float forceImpulse;

    public float timeDestroy = 0.4f;

    private void Start()
    {
        StartCoroutine("autoDestroyer");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Dead")
        {
            if (collision.gameObject.GetComponent<Rigidbody2D>())
            {
                Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
                Vector2 xy = transform.position - rb.transform.position;
                rb.AddForce(-xy * forceImpulse);
            }

            AttributesBase scriptAttributes = collision.GetComponent<AttributesBase>();
            if (scriptAttributes != null)
            {
                scriptAttributes.applyDmg(damage);
            }
        }
        else if (collision.gameObject.tag == "Fire")
        {
            GameObject set = collision.gameObject;
            collision.GetComponent<FireParameters>().StartColision(set);
        }
    }

    IEnumerator autoDestroyer()
    {
        yield return new WaitForSeconds(timeDestroy);
        Destroy(gameObject);
    }
}
