using UnityEngine;

public class FinishFlag : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] int StrawberryNeed;
    
    
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            
            if (collision.gameObject.GetComponent<PlayerController>().GetStrawberry() >= StrawberryNeed)
            {
                SoundManager.Instance.PlaySFX("Win");

                gameManager.isWin = true;
            }
            else
            {
                Debug.Log("need strawberry");
            }
            
        }
    }
}
