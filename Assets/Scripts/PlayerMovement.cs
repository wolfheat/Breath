using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] UIController uiController;
    [SerializeField] Transform tilt;
    // Player moves acording to velocity and acceleration
    private Vector2 mouseStoredPosition;
// Max speed
    float maxSpeed = 5f;
    float cruiseSpeed = 2f;
    Vector3 velocity = new Vector3();
    Vector3 boosterAcceleration = new Vector3();
    float accelerationSpeed = 5f;
    float dampening = 0.2f;
    float stopDampening = 6f;
    float driftDampening = 25f;
    private const float StopingSpeedLimit = 0.1f; // go slower than this and you imidiately stop
    private const float DistanceLimit = 0.1f; // go slower than this and you imidiately stop
    private const float MaxDistanceLimit = 2.5f; // safe messure if moving to far away from throw point
    private const float LookSensitivity = 0.15f;
    private const float RotationLowerLimit = 89;
    private const float RotationUpperLimit = 271;

    private Coroutine throwCoroutine;

    public void Start()
    {
        Inputs.Instance.Controls.Player.RClick.performed += RClickPerformed;
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //Entering Gravity area
        if (other.gameObject.GetComponent<GravityArea>() != null)
        {
            rb.useGravity = true;
            SoundMaster.Instance.ChangeMusicTrack(MusicTrack.Indoor);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //Entering Gravity area
        if (other.gameObject.GetComponent<GravityArea>() != null)
        {
            rb.useGravity = false;
            SoundMaster.Instance.ChangeMusicTrack(MusicTrack.OutDoor);
        }
    }

    private void Update()
    {
        Move();
        Look();
    }

    private void Move()
    {
        Vector2 move = Inputs.Instance.Controls.Player.Move.ReadValue<Vector2>();
        
        if(throwCoroutine != null)
        {
            if (move.magnitude != 0) Debug.Log("Can not move while being thrown");
            return;
        }

        boosterAcceleration = new();
        // SIDEWAY MOVEMENT
        boosterAcceleration = move[0] * transform.right * accelerationSpeed;
        // SPEED BOOSTER MOVEMENT
        boosterAcceleration += move[1] * tilt.forward * accelerationSpeed;

        rb.AddForce(boosterAcceleration * Time.deltaTime, ForceMode.VelocityChange);
        // Limit velocity
        if (rb.velocity.magnitude > maxSpeed)
            rb.velocity = rb.velocity.normalized * maxSpeed;

        uiController.SetSpeed(rb.velocity);

        // If Not accelerating and above cruise speed dampen the speed down to cruise speed       
        if (velocity.magnitude > cruiseSpeed)
        {
            // TODO How to make dampening independent on frame speed
            // Make different dampening in space and inside a spacestation
            if (boosterAcceleration.magnitude == 0)
            {
                float before = rb.velocity.magnitude;
                rb.velocity *= Mathf.Pow(dampening, Time.deltaTime);
                if(rb.velocity.magnitude != before)
                    Debug.Log("Dampening velocity from "+before+" to "+rb.velocity.magnitude);
                
            }
            else
            {
                // Limit sideway velocity when using booster
                
                Vector3 perpendicularSpeed = Vector3.Dot(rb.velocity.normalized, boosterAcceleration.normalized) * boosterAcceleration.normalized;

                rb.velocity -= perpendicularSpeed * driftDampening * Time.deltaTime;
                
            }
        }

        if (Inputs.Instance.Controls.Player.Space.ReadValue<float>() != 0)
            StopInPlace();

        // Limit angular rotations
        StopRotations();
    }

    private void StopRotations()
    {
        Vector3 startRot = rb.transform.rotation.eulerAngles;
        rb.angularVelocity = Vector3.zero;
        // Set all but Y direction to 0
        rb.transform.rotation = Quaternion.Euler(0, rb.transform.rotation.eulerAngles.y, 0);
        Vector3 endRot = rb.transform.rotation.eulerAngles;
        if (Mathf.Abs(startRot.x - endRot.x) > 2f)
        {
            Debug.Log("Stop Rotations Changed rotations from " + startRot + " to " + endRot);
        }

    }

    private void StopInPlace()
    {
        if (rb.velocity.magnitude > StopingSpeedLimit)
        {
            float newVel = rb.velocity.magnitude - stopDampening * Time.deltaTime;
            rb.velocity = rb.velocity.normalized * newVel;
        }
        else
            rb.velocity = Vector3.zero;
    }

    [DllImport("user32.dll")]
    public static extern bool SetCursorPos(int X, int Y);

    public void Look()
    {
        // Right button is not held?
        if (!Inputs.Instance.Controls.Player.RClick.IsPressed())
        {
            if (Cursor.visible == true) return;

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            RegainCursorPosition();

            return;
        }

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

    internal void ThrowPlayer(DoorThrower doorThrower)
    {
        Debug.Log("Request To throw player");
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
        mouseStoredPosition = (Vector2)Input.mousePosition;
        uiController.ShowTempHairAt(mouseStoredPosition);
    }
    public void RegainCursorPosition()
    {
        Mouse.current.WarpCursorPosition(mouseStoredPosition);
        uiController.HideTempHair();
    }

}
