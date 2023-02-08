using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;

public class CarAIHandler : MonoBehaviour
{
    public enum AIMode { followPlayer, followMouse };

    [Header("AI settings")]
    public AIMode aiMode;
    public float maxSpeed = 16;
    public bool isAvoidingCars = true;
    [Range(0.0f, 1.0f)]
    public float skillLevel = 1.0f;
    private Transform player;
    public float fireRate = 1f;
    public float nextFireTime;

    public float speed;

    public float detectionRange = 5f;
    public float rotationSpeed = 3f;
    public LayerMask obstacleLayer;
    private Rigidbody2D rb;

    //Local variables
    Vector3 targetPosition = Vector3.zero;
    Transform targetTransform = null;
    float orignalMaximumSpeed = 0;

    //Stuck handling
    bool isRunningStuckCheck = false;
    int stuckCheckCounter = 0;
    List<Vector2> temporaryWaypoints = new List<Vector2>();
    float angleToTarget = 0;

    //Avoidance
    Vector2 avoidanceVectorLerped = Vector3.zero;
    Vector2 avoidanceVector = Vector2.zero;
    Vector2 obstacleDetectedPosition = Vector3.zero;
    Vector2 avoidanceDirection = Vector2.zero;
    float avoidanceDistance = 0f;
    int maxRays = 3;
    float raySpacing = 0.25f;

    NavMeshAgent agentti;
    Vector3 määränpää;

    //Colliders
    PolygonCollider2D polygonCollider2D;

    //Components
    TopDownCarController topDownCarController;

    public float shootingRange;
    public GameObject bullet;
    public GameObject bulletParent;
    

    //Awake is called when the script instance is being loaded.
    void Awake()
    {
        topDownCarController = GetComponent<TopDownCarController>();
        

        

        polygonCollider2D = GetComponentInChildren<PolygonCollider2D>();

        orignalMaximumSpeed = maxSpeed;
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agentti = GetComponent<NavMeshAgent>();
        määränpää = agentti.destination;


        SetMaxSpeedBasedOnSkillLevel(maxSpeed);

        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame and is frame dependent
    void FixedUpdate()
    {
        Vector2 inputVector = Vector2.zero;

        switch (aiMode)
        {
            case AIMode.followPlayer:
                FollowPlayer();
                break;


            case AIMode.followMouse:
                FollowMousePosition();
                break;
        }

        inputVector.x = TurnTowardTarget();
        inputVector.y = ApplyThrottleOrBrake(inputVector.x);

        //If the AI is applying throttle but not manging to get any speed then lets run our stuck check.
        if (topDownCarController.GetVelocityMagnitude() < 0.5f && Mathf.Abs(inputVector.y) > 0.01f && !isRunningStuckCheck)
            StartCoroutine(StuckCheckCO());

        //Handle special case where the car has reversed for a while then it should check if it is still stuck. If it is not then it will drive forward again.
        if (stuckCheckCounter >= 4 && !isRunningStuckCheck)
            StartCoroutine(StuckCheckCO());


        //Send the input to the car controller.
        topDownCarController.SetInputVector(inputVector);

        
    }


    //AI follows player
    void FollowPlayer()
    {
        if (targetTransform == null)
            targetTransform = player;

        if (targetTransform != null)
            targetPosition = targetTransform.position;
    }
    
    //AI follows the mouse position
    void FollowMousePosition()
    {
        //Take the mouse position in screen space and convert it to world space
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //Set the target position of for the AI. 
        targetPosition = worldPosition;
    }

    //Shooting

    private List<Transform> _obstacles = new List<Transform>();

    private void Update()
    {
        float distanceFromPlayer = Vector2.Distance(player.position, transform.position );

        this.transform.position += transform.up * Time.deltaTime * speed;

        if(Vector3.Distance(player.position, määränpää) > 1.0f)
        {
            määränpää = player.position;
            agentti.destination = määränpää;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("rakennukset"))
        {
            this.transform.Rotate(transform.rotation.x, transform.rotation.y, transform.rotation.z - 90);
        }
    }



    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, shootingRange);
    }





    float TurnTowardTarget()
    {
        Vector2 vectorToTarget = targetPosition - transform.position;
        vectorToTarget.Normalize();

    //Check for obstacles and steer around them
        if (isAvoidingCars)
        AvoidCars(vectorToTarget, out vectorToTarget);

        float averageDirection = 0f;
        if (_obstacles.Count > 0)
        {
            Vector2 averageObstacleDirection = Vector2.zero;
            foreach (var obstacle in _obstacles)
            {
                Vector2 vectorToObstacle = (Vector2)obstacle.position - (Vector2)transform.position;
                averageObstacleDirection += vectorToObstacle.normalized;
            }
            averageObstacleDirection /= _obstacles.Count;

            averageDirection = Vector2.SignedAngle(transform.up, averageObstacleDirection);
        }

        float steerInput = Mathf.Clamp(averageDirection * rotationSpeed, -1f, 1f);

        return steerInput;
    }

    bool IsObstacleInWay(Vector2 vectorToTarget, out Vector2 avoidanceVector)
    {
        //Shoot a ray in the direction of the target and check for obstacles
        RaycastHit2D hit = Physics2D.Raycast(transform.position, vectorToTarget, detectionRange, obstacleLayer);

        if (hit.collider != null)
        {
            //Obstacle detected, calculate an avoidance vector that steers around the obstacle
            Vector2 perpendicular = Vector2.Perpendicular(hit.normal).normalized;
            avoidanceVector = (vectorToTarget + perpendicular * 2).normalized;

            return true;
        }
        else
        {
            avoidanceVector = Vector2.zero;
            return false;
        }
    }

    float ApplyThrottleOrBrake(float inputX)
    {
        //If we are going too fast then do not accelerate further. 
        if (topDownCarController.GetVelocityMagnitude() > maxSpeed)
            return 0;

        //Apply throttle forward based on how much the car wants to turn. If it's a sharp turn this will cause the car to apply less speed forward. We store this as reduceSpeedDueToCornering so we can use it togehter with the skill level
        float reduceSpeedDueToCornering = Mathf.Abs(inputX) / 1.0f;

        //Apply throttle based on cornering and skill.
        float throttle = 1.05f - reduceSpeedDueToCornering * skillLevel;

        //Handle throttle differently when we are following temp waypoints
        if (temporaryWaypoints.Count() != 0)
        {
            //If the angle is larger to reach the target the it is better to reverse. 
            if (angleToTarget > 70)
                throttle = throttle * -1;
            else if (angleToTarget < -70)
                throttle = throttle * -1;
            //If we are still stuck after a number of attempts then just reverse. 
            else if (stuckCheckCounter > 3)
                throttle = throttle * -1;
        }

        //Apply throttle based on cornering and skill.
        return throttle;
    }

    void SetMaxSpeedBasedOnSkillLevel(float newSpeed)
    {
        maxSpeed = Mathf.Clamp(newSpeed, 0, orignalMaximumSpeed);

        float skillbasedMaxiumSpeed = Mathf.Clamp(skillLevel, 0.3f, 1.0f);
        maxSpeed = maxSpeed * skillbasedMaxiumSpeed;
    }


    //Finds the nearest point on a line. 
    Vector2 FindNearestPointOnLine(Vector2 lineStartPosition, Vector2 lineEndPosition, Vector2 point)
    {
        //Get heading as a vector
        Vector2 lineHeadingVector = (lineEndPosition - lineStartPosition);

        //Store the max distance
        float maxDistance = lineHeadingVector.magnitude;
        lineHeadingVector.Normalize();

        //Do projection from the start position to the point
        Vector2 lineVectorStartToPoint = point - lineStartPosition;
        float dotProduct = Vector2.Dot(lineVectorStartToPoint, lineHeadingVector);

        //Clamp the dot product to maxDistance
        dotProduct = Mathf.Clamp(dotProduct, 0f, maxDistance);

        return lineStartPosition + lineHeadingVector * dotProduct;
    }

    //Checks for cars ahead of the car.
    bool IsCarsInFrontOfAICar(out Vector3 position, out Vector3 otherCarRightVector)
    {
        //Disable the cars own collider to avoid having the AI car detect itself. 
        polygonCollider2D.enabled = false;

        //Perform the circle cast in front of the car with a slight offset forward and only in the Car layer
        RaycastHit2D raycastHit2d = Physics2D.CircleCast(transform.position + transform.up * 0.5f, 1.2f, transform.up, 12, 1 << LayerMask.NameToLayer("Car"));

        //Enable the colliders again so the car can collide and other cars can detect it.  
        polygonCollider2D.enabled = true;

        if (raycastHit2d.collider != null)
        {
            //Draw a red line showing how long the detection is, make it red since we have detected another car
            Debug.DrawRay(transform.position, transform.up * 12, Color.red);

            position = raycastHit2d.collider.transform.position;
            otherCarRightVector = raycastHit2d.collider.transform.right;
            return true;
        }
        else
        {
            //We didn't detect any other car so draw black line with the distance that we use to check for other cars. 
            Debug.DrawRay(transform.position, transform.up * 12, Color.black);
        }

        //No car was detected but we still need assign out values so lets just return zero. 
        position = Vector3.zero;
        otherCarRightVector = Vector3.zero;

        return false;
    }

    void AvoidCars(Vector2 vectorToTarget, out Vector2 newVectorToTarget)
    {
        if (IsCarsInFrontOfAICar(out Vector3 otherCarPosition, out Vector3 otherCarRightVector))
        {
            Vector2 avoidanceVector = Vector2.zero;

            //Calculate the reflecing vector if we would hit the other car. 
            avoidanceVector = Vector2.Reflect((otherCarPosition - transform.position).normalized, otherCarRightVector);

            float distanceToTarget = (targetPosition - transform.position).magnitude;

            //We want to be able to control how much desire the AI has to drive towards the waypoint vs avoiding the other cars. 
            //As we get closer to the waypoint the desire to reach the waypoint increases.
            float driveToTargetInfluence = 6.0f / distanceToTarget;

            //Ensure that we limit the value to between 30% and 100% as we always want the AI to desire to reach the waypoint.  
            driveToTargetInfluence = Mathf.Clamp(driveToTargetInfluence, 0.30f, 1.0f);

            //The desire to avoid the car is simply the inverse to reach the waypoint
            float avoidanceInfluence = 1.0f - driveToTargetInfluence;

            //Reduce jittering a little bit by using a lerp
            avoidanceVectorLerped = Vector2.Lerp(avoidanceVectorLerped, avoidanceVector, Time.fixedDeltaTime * 4);

            //Calculate a new vector to the target based on the avoidance vector and the desire to reach the waypoint
            newVectorToTarget = (vectorToTarget * driveToTargetInfluence + avoidanceVector * avoidanceInfluence);
            newVectorToTarget.Normalize();

            //Draw the vector which indicates the avoidance vector in green
            Debug.DrawRay(transform.position, avoidanceVector * 10, Color.green);

            //Draw the vector that the car will actually take in yellow. 
            Debug.DrawRay(transform.position, newVectorToTarget * 10, Color.yellow);

            //we are done so we can return now. 
            return;
        }

        //We need assign a default value if we didn't hit any cars before we exit the function. 
        newVectorToTarget = vectorToTarget;
    }

    IEnumerator StuckCheckCO()
    {
        Vector3 initialStuckPosition = transform.position;

        isRunningStuckCheck = true;

        yield return new WaitForSeconds(0.7f);

        //if we have not moved for a second then we are stuck
        if ((transform.position - initialStuckPosition).sqrMagnitude < 3)
        {
            //Get a path to the desired position
            

            //If there was no path found then it will be null so if that happens just make a new empty list.
            if (temporaryWaypoints == null)
                temporaryWaypoints = new List<Vector2>();

            stuckCheckCounter++;
        }
        else stuckCheckCounter = 0;

        isRunningStuckCheck = false;
    }
}