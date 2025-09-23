using UnityEngine;

public class FinishFlag : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            
            if (collision.gameObject.GetComponent<PlayerController>().haveStrawberry)
            {
                gameManager.isWin = true;
            }
            else
            {
                Debug.Log("need strawberry");
            }
            
        }
    }
}
