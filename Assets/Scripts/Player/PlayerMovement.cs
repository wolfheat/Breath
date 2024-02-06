using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.InputSystem;
using Wolfheat.StartMenu;
using static UnityEngine.InputSystem.InputAction;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] PlayerStats playerStats;
    [SerializeField] UIController uiController;
    [SerializeField] Transform tilt;
    [SerializeField] private PickableItem genericPrefab;

    public bool InGravity { get { return rb.useGravity; } }
    // Player moves acording to velocity and acceleration
    private Vector2 mouseStoredPosition;
// Max speed
    float maxSpeed = 5f;
    Vector3 lastSafePoint = new Vector3();
    Vector3 boosterAcceleration = new Vector3();
    float boosterAccelerationSpeed = 5f;
    float walkingAccelerationSpeed = 1f;
    float dampening = 0.2f;
    float stopDampening = 6f;
    private const float StopingSpeedLimit = 0.1f; // go slower than this and you imidiately stop
    private const float DistanceLimit = 0.1f; // go slower than this and you imidiately stop
    private const float MaxDistanceLimit = 2.5f; // safe messure if moving to far away from throw point
    private const float LookSensitivity = 0.15f;
    private const float RotationLowerLimit = 89;
    private const float RotationUpperLimit = 271;
    private const float WalkSpeed = 1.5f;
    private const float WalkSpeedMinimum = 0.3f;
        
    private Coroutine throwCoroutine;

    public void OnEnable()
    {

        Inputs.Instance.Controls.Player.RClick.performed += RClickPerformed;
        lastSafePoint = rb.transform.position;
    }
    public void OnDisable()
    {
        Inputs.Instance.Controls.Player.RClick.performed -= RClickPerformed;
    }

    private void OnTriggerEnter(Collider other)
    {
        //Entering Gravity area
        if (other.gameObject.GetComponent<GravityArea>() != null)
        {
            rb.useGravity = true;
            SoundMaster.Instance.PlayMusic(MusicName.IndoorMusic);
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        //Entering Gravity area
        if (other.gameObject.GetComponent<GravityArea>() != null)
        {
            rb.useGravity = false;
            SoundMaster.Instance.PlayMusic(MusicName.OutDoorMusic);
        }
    }

    private void Update()
    {
        Move();
        Look();
    }

    private void Move()
    {
        if (playerStats.IsDead || UIController.CraftingActive)
        {
            Stop();
            return;
        }

        Vector2 move = Inputs.Instance.Controls.Player.Move.ReadValue<Vector2>();
        
        if(throwCoroutine != null)
        {
            //if (move.magnitude != 0) Debug.Log("Can not move while being thrown");
            return;
        }

        // SIDEWAY MOVEMENT
        boosterAcceleration = move[0] * transform.right;
        // SPEED BOOSTER MOVEMENT
        boosterAcceleration += move[1] * tilt.forward;

        

        if (rb.useGravity)
        {
            //remove up and down
            boosterAcceleration = new Vector3(boosterAcceleration.x, 0, boosterAcceleration.z);
            
            // Movement in Gravity            
            Vector3 planeParts = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            Vector3 result = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            
            rb.AddForce(boosterAcceleration.normalized * walkingAccelerationSpeed, ForceMode.VelocityChange);

            planeParts = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            if (planeParts.magnitude > WalkSpeed)
            {
                rb.velocity = planeParts.normalized * WalkSpeed + rb.velocity.y * Vector3.up;
            }

            /*
            if (boosterAcceleration.magnitude>0)
                rb.velocity = boosterAcceleration.normalized * minSpeed + rb.velocity.y * Vector3.up;
            else
                rb.velocity = rb.velocity.y * Vector3.up;
        }
        else if (planeParts.magnitude > maxSpeed)
            rb.velocity = planeParts.normalized * maxSpeed+rb.velocity.y*Vector3.up;
        */
            DampenSpeedInDoors();

            // STEP SOUND
            if (planeParts.magnitude > WalkSpeedMinimum)
                SoundMaster.Instance.PlayStepSound();

        }
        else
        {
            float upDown = Inputs.Instance.Controls.Player.UpDown.ReadValue<float>();
            boosterAcceleration += upDown * transform.up;

            // Movement in NON-Gravity
            rb.AddForce(boosterAcceleration.normalized * boosterAccelerationSpeed * Time.deltaTime, ForceMode.VelocityChange);
            // Limit velocity
            if (rb.velocity.magnitude > playerStats.MaxSpeed)
                rb.velocity = rb.velocity.normalized * playerStats.MaxSpeed;

            // CRUISE SPEED LIMITATION       
            if (rb.velocity.magnitude > playerStats.MaxSpeed/2f)
                LimitSpeedToCruiseSpeed(); 

            // PLAYER STOP IN PLACE
            if (Inputs.Instance.Controls.Player.LeftAlt.ReadValue<float>() != 0)
            StopInPlace();
        }

        // Show speed in HUD
        uiController.SetSpeed(rb.velocity);

        // Limit angular rotations
        StopRotations();
    }

    private bool StandingOnCollider()
    {
        bool hitCollider = Physics.Raycast(transform.position, Vector3.down, 1f);
        return hitCollider;
    }

    private void DampenSpeedInDoors()
    {
        rb.velocity *= Mathf.Pow(dampening, Time.deltaTime);
    }
    
    private void LimitSpeedToCruiseSpeed()
    {
        // Make different dampening in space and inside a spacestation
        if (boosterAcceleration.magnitude == 0)
        {
            rb.velocity *= Mathf.Pow(dampening, Time.deltaTime);
            return;
        }

        // Limit sideway velocity when using booster - Experimental
        /*
        Vector3 perpendicularSpeed = Vector3.Dot(rb.velocity.normalized, boosterAcceleration.normalized) * boosterAcceleration.normalized;
        rb.velocity -= perpendicularSpeed * driftDampening * Time.deltaTime;        
        */
    }

    private void StopRotations()
    {
        // Hinder player from falling down
        Vector3 startRot = rb.transform.rotation.eulerAngles;
        // Stop current rotation
        rb.angularVelocity = Vector3.zero;
        // Reset player to upright position
        rb.transform.rotation = Quaternion.Euler(0, rb.transform.rotation.eulerAngles.y, 0);
    }

    private void StopInPlace()
    {
        if (rb.velocity.magnitude <= StopingSpeedLimit)
        {
            rb.velocity = Vector3.zero;
            return;
        }
        // Slow Down player
        float newVel = rb.velocity.magnitude - stopDampening * Time.deltaTime;
        rb.velocity = rb.velocity.normalized * newVel;
        
    }
    
    private void Stop()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    [DllImport("user32.dll")]
    public static extern bool SetCursorPos(int X, int Y);

    public void Look()
    {
        // Right button is not held or player is dead = regain normal cursor
        if (!Inputs.Instance.Controls.Player.RClick.IsPressed() || playerStats.IsDead)
        {
            if (Cursor.visible == true) return;

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            RegainCursorPosition();
            return;
        }

        // Disables Right button to work when in UI
        if (UIController.InventoryActive || UIController.CraftingActive)
            return;

        // Right button is held

        // Hide cursor if changing view
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // When holding right button rotate player by mouse movement
        Vector2 mouseMove = Inputs.Instance.Controls.Player.Look.ReadValue<Vector2>();
        if (mouseMove.magnitude != 0)
        {
            // ISSUE Lower part of screen goes from 270-360 upper part 0-90. Issue to limit looking up and down past boundaries
            // 0.675 -> -0.675
            // 270 -> 90

            // Looking to the sides
            rb.transform.Rotate(0, mouseMove[0] * LookSensitivity, 0, Space.Self);

            // Looking up and down
            float oldAngle = tilt.localRotation.eulerAngles.x;
            float rotationAngle = (-mouseMove[1] * LookSensitivity);
            
            float resultAngle = oldAngle + rotationAngle;
            if (rotationAngle > 0 && oldAngle <= RotationLowerLimit + 1 && oldAngle >= RotationLowerLimit - 20f && resultAngle >= RotationLowerLimit)
            {
                Debug.Log("Changing valid rotationangle");
                rotationAngle = RotationLowerLimit - oldAngle;
            }
            else if (rotationAngle < 0 && oldAngle >= RotationUpperLimit - 1 && oldAngle <= RotationUpperLimit + 20f && resultAngle <= RotationUpperLimit)
            {
                Debug.Log("Changing to max rotationangle");

                rotationAngle = RotationUpperLimit - oldAngle;
            }
            tilt.transform.Rotate(rotationAngle, 0, 0, Space.Self);
            uiController.SetTilt(tilt.transform.localRotation.eulerAngles.x);
            uiController.SetPlayerTilt(rb.transform.localRotation.eulerAngles);
            // At 334 degrees

        }
    }

    public void ThrowPlayer(DoorThrower doorThrower)
    {
        lastSafePoint = doorThrower.transform.position;
        Debug.Log("Request To throw player: safePoint set to "+lastSafePoint);
        if(throwCoroutine!=null) StopCoroutine(throwCoroutine);
        throwCoroutine = StartCoroutine(ThrowPlayerCO(doorThrower));
    }

    private IEnumerator ThrowPlayerCO(DoorThrower thrower)
    {
        Debug.Log("Throw coroutine STARTED");
        // Move player towards trigger, when close enough throw against target

        Vector3 direction = (thrower.transform.position - rb.transform.position).normalized;
        rb.velocity = direction * maxSpeed;
        // Have to disable gravity or velocity wont take player to target
        //rb.useGravity = false;

        float currentDistance = (thrower.transform.position - rb.transform.position).magnitude;

        while (currentDistance > DistanceLimit && currentDistance < MaxDistanceLimit)
        {
            yield return null;
            rb.velocity = direction * maxSpeed;
            currentDistance = (thrower.transform.position - rb.transform.position).magnitude;
        }
        
        rb.transform.position = thrower.transform.position;
        //Debug.Log("Player is at thrower position, send him towards target:" + thrower.target.name);

        yield return null;
        // Now at thrower
        rb.velocity = (thrower.target.transform.position - rb.transform.position).normalized * maxSpeed * 3;
        yield return null;

        Debug.Log("Throw coroutine COMPLETE");
        throwCoroutine = null;
    }

    // Input handling
    public void RClickPerformed(CallbackContext context)
    {
        if (UIController.InventoryActive || UIController.CraftingActive) 
            return;

        mouseStoredPosition = (Vector2)Input.mousePosition;
        uiController.ShowTempHairAt(mouseStoredPosition);
    }

    public void RegainCursorPosition()
    {
        Mouse.current.WarpCursorPosition(mouseStoredPosition);
        uiController.HideTempHair();
    }

    public void SetToSafePoint()
    {
        Debug.Log("Setting player to safepoint: " + lastSafePoint);

        rb.transform.position = lastSafePoint;
    }

    public void CreateItemBox(ItemData data)
    {
        Debug.Log("Creating box in front of player");
        PickableItem box = Instantiate(genericPrefab, transform.position + transform.forward*2, Quaternion.identity);
        box.Data = data;
    }
}
