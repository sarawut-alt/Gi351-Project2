using UnityEngine;

public class BackGroundController : MonoBehaviour
{
    private float startingHorizontalPosition , spriteWidth;
    public GameObject Cam;
    public float parallaxEffectFactor; //multiplier that determines the strength of the parallax effect

    private float distanceBackGroundShouldMove;
    private float cameraRelativeMovement;

    void Awake()
    {
        Cam = Camera.main.gameObject;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startingHorizontalPosition = transform.position.x;
        spriteWidth = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        distanceBackGroundShouldMove = Cam.transform.position.x * parallaxEffectFactor; 
        cameraRelativeMovement = Cam.transform.position.x * (1 - parallaxEffectFactor);//determine when bg needs to loop
        
        transform.position = new Vector3(startingHorizontalPosition + distanceBackGroundShouldMove, Cam.transform.position.y, transform.position.z);

        if (cameraRelativeMovement > startingHorizontalPosition + spriteWidth)
        {
            startingHorizontalPosition += spriteWidth;
        }
        else if (cameraRelativeMovement < startingHorizontalPosition - spriteWidth)
        {
            startingHorizontalPosition -= spriteWidth;
        }
    }
}
