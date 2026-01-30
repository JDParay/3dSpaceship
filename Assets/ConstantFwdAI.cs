using UnityEngine;

public class ConstantFwdAI : MonoBehaviour
{
    public float speed = 5f;
    public float strafeSpeed = 8f;
    public float panicMultiplier = 2.0f;
    public float rayDistance = 10f;
    public float sideRayAngle = 25f;
    public float dodgeDuration = 0.5f; 
    public LayerMask obstacleLayer;

    private float currentDodgeTime = 0f;
    private Vector3 currentStrafeDir = Vector3.zero;
    private bool isPanicking = false;
    public GameTimer gameManager;

    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;

        CheckForObstacles();

        if (currentDodgeTime > 0)
        {
            float finalStrafeSpeed = isPanicking ? (strafeSpeed * panicMultiplier) : strafeSpeed;
            transform.position += currentStrafeDir * finalStrafeSpeed * Time.deltaTime;
            currentDodgeTime -= Time.deltaTime;
        }
    }

    void CheckForObstacles()
    {
        float leftDist = CastDistance(Quaternion.Euler(0, -sideRayAngle, 0) * transform.forward);
        float rightDist = CastDistance(Quaternion.Euler(0, sideRayAngle, 0) * transform.forward);
        float fwdDist = CastDistance(transform.forward);

        float dangerZone = rayDistance * 0.4f; // Very close!
        float detectionZone = rayDistance * 0.9f;

        if (fwdDist < detectionZone || leftDist < rayDistance * 0.6f || rightDist < rayDistance * 0.6f)
        {
            currentStrafeDir = (leftDist > rightDist) ? -transform.right : transform.right;
            currentDodgeTime = dodgeDuration;

            isPanicking = (fwdDist < dangerZone);
        }
        else if (currentDodgeTime <= 0)
        {
            isPanicking = false;
        }
    }

    float CastDistance(Vector3 dir)
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position, dir * (rayDistance * 0.4f), Color.red); 

        if (Physics.Raycast(transform.position, dir, out hit, rayDistance, obstacleLayer))
        {
            return hit.distance;
        }
        return rayDistance;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Asteroid"))
        {
            Debug.Log("Oopsie! Hit asteroid.");
            if (gameManager != null)
            {
                gameManager.ExplodeShip();
            }
            else
            {
                Time.timeScale = 0f;
            }
        }
    }
}