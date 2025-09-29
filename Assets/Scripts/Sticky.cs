using System.Collections;
using UnityEngine;

public class Sticky : MonoBehaviour
{
    public float respawnTime = 11f;

    public void CollectSticky(float time)
    {
        StartCoroutine(RespawnSticky());
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.GetComponent<PolygonCollider2D>().enabled = false;

    }

    IEnumerator RespawnSticky()
    {
        yield return new WaitForSeconds(respawnTime);
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
        gameObject.GetComponent<PolygonCollider2D>().enabled = true;

    }
}
