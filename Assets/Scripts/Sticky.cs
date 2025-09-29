using System.Collections;
using UnityEngine;

public class Sticky : MonoBehaviour
{
    public float respawnTime = 11f;

    public void CollectSticky()
    {
        StartCoroutine(RespawnSticky());
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;

    }

    IEnumerator RespawnSticky()
    {
        yield return new WaitForSeconds(respawnTime);
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
        gameObject.GetComponent<BoxCollider2D>().enabled = true;

    }
}
