using UnityEngine;

public class BackGroundController : MonoBehaviour
{
    private float startingHorizontalPosition , spriteWidth, startingVerticalPosition;
    public GameObject Cam;
    public float parallaxEffectFactorX; //multiplier that determines the strength of the parallax effect
    public float parallaxEffectFactorY;
    private float distanceBackGroundShouldMoveX;
    private float distanceBackGroundShouldMoveY;
    private float cameraRelativeMovement;

    void Awake()
    {
        Cam = Camera.main.gameObject;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startingHorizontalPosition = transform.position.x;
        startingVerticalPosition = transform.position.y;
        spriteWidth = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        distanceBackGroundShouldMoveX = Cam.transform.position.x * parallaxEffectFactorX;
        distanceBackGroundShouldMoveY = Cam.transform.position.y * parallaxEffectFactorY;
        cameraRelativeMovement = Cam.transform.position.x * (1 - parallaxEffectFactorX);//determine when bg needs to loop
        
        transform.position = new Vector3(startingHorizontalPosition + distanceBackGroundShouldMoveX, startingVerticalPosition + distanceBackGroundShouldMoveY, transform.position.z);

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
