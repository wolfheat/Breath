using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using static UnityEngine.InputSystem.InputAction;

public class Player : MonoBehaviour
{
    [SerializeField] UIController uiController;
    [SerializeField] Transform tilt;
    [SerializeField] Rigidbody rb;
    // Free movement of camera with sensitivity

    // Player moves acording to velocity and acceleration
    // Max speed
    float maxSpeed = 5f;
    float cruiseSpeed = 2f;
    Vector3 velocity = new Vector3();
    Vector3 boosterAcceleration = new Vector3();
    float accelerationSpeed = 5f;
    float dampening = 0.2f;
    float stopDampening = 6f;
    private Vector2 mouseStoredPosition;
    private const float StopingSpeedLimit = 0.2f;
    private const float LookSensitivity = 15f;

    private void Start()
    {
        Debug.Log("Created Player");
        // set up input actions
        Inputs.Instance.Controls.Player.Click.performed += Click;
        Inputs.Instance.Controls.Player.RClick.performed += RClickPerformed;

    }

    private void OnTriggerEnter(Collider other)
    {
        //Entering Gravity area
        if(other.gameObject.GetComponent<GravityArea>() != null)
        {
            rb.useGravity = true;
        }else if (other.gameObject.GetComponent<DoorTrigger>() != null)
        {
            other.gameObject.GetComponent<DoorTrigger>().controller.OpenDoors(true);   
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        //Entering Gravity area
        if (other.gameObject.GetComponent<GravityArea>() != null)
        {
            rb.useGravity = false;
        }
        else if (other.gameObject.GetComponent<DoorTrigger>() != null)
        {
            other.gameObject.GetComponent<DoorTrigger>().controller.OpenDoors(false);
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
        boosterAcceleration = new();
        boosterAcceleration = move[0] * transform.right * accelerationSpeed;
        boosterAcceleration += move[1] * tilt.forward * accelerationSpeed;

        rb.AddForce(boosterAcceleration * Time.deltaTime,ForceMode.VelocityChange);
        // Limit velocity
        if (rb.velocity.magnitude > maxSpeed)
            rb.velocity = rb.velocity.normalized * maxSpeed;

        uiController.SetSpeed(rb.velocity);

        // If Not accelerating and above cruise speed dampen the speed        
        if (velocity.magnitude > cruiseSpeed)
        {

            // TODO How to make dampening independent on frame speed
            // Make different dampening in space and station
            if(boosterAcceleration.magnitude == 0)
                rb.velocity *= Mathf.Pow(dampening, Time.deltaTime);
            else
            {
                // Limit velocity thats not forward
                Vector3 perpendicularSpeed = Vector3.Dot(rb.velocity.normalized, boosterAcceleration.normalized) * boosterAcceleration.normalized;

                rb.velocity -= perpendicularSpeed* stopDampening*3 * Time.deltaTime;
            }
        }

        if (Inputs.Instance.Controls.Player.Space.ReadValue<float>() != 0)
            StopInPlace();


        // Limit angular rotations
        StopRotations();
    }

    private void StopRotations()
    {
        rb.angularVelocity = Vector3.zero;
        // Set all but Y direction to 0
        rb.transform.rotation = Quaternion.Euler(0, rb.transform.rotation.eulerAngles.y, 0);

    }

    private void StopInPlace()
    {
        if (rb.velocity.magnitude > StopingSpeedLimit)
        {
            float newVel = rb.velocity.magnitude - stopDampening * Time.deltaTime;
            rb.velocity = rb.velocity.normalized*newVel;
        }
        else
            rb.velocity = Vector3.zero;
    }

    [DllImport("user32.dll")]
    public static extern bool SetCursorPos(int X, int Y);

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
    
    public void Click(CallbackContext context)
    {
        Debug.Log("CLICK");
    }

    public void Look()
    {
        if (!Inputs.Instance.Controls.Player.RClick.IsPressed())
        {
            if (Cursor.visible == true) return;

            Cursor.visible = true;  
            Cursor.lockState = CursorLockMode.None;

            RegainCursorPosition();

            return;
        }

        // Hide cursor if changing view

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // When holding right button rotate player by mouse movement
        Vector2 mouseMove = Inputs.Instance.Controls.Player.Look.ReadValue<Vector2>();
        if (mouseMove.magnitude != 0)
        {            
            // Check if target is outside boundaries
            rb.transform.Rotate(0,  mouseMove[0] * LookSensitivity * Time.deltaTime, 0, Space.Self);
            // Method to limit looking directly down or up or passing these looking points

            // Still issue with snapping to up or down look for some reason
            Quaternion oldRot = tilt.localRotation;
            float oldRotX = (oldRot.eulerAngles.x+180)%360; // Angle set between 0-180
            float rotationAngle = -mouseMove[1] * LookSensitivity * Time.deltaTime;
            float resultAngle = (oldRotX+rotationAngle);
            resultAngle = Mathf.Clamp(resultAngle, 95, 265)-180;
            tilt.localRotation = Quaternion.Euler(resultAngle,0,0);
        }        
    }

    internal void ThrowPlayer(DoorThrower doorThrower)
    {
        Debug.Log("Request To throw player");
        StartCoroutine(ThrowPlayerCO(doorThrower));
    }

    private IEnumerator ThrowPlayerCO(DoorThrower thrower)
    {
        // Move player towards trigger, when close enough throw against target
        rb.velocity = (thrower.transform.position - rb.transform.position).normalized*maxSpeed;
        float lastDistance = (thrower.transform.position - rb.transform.position).magnitude; 
        yield return null;
        float currentDistance = (thrower.transform.position - rb.transform.position).magnitude;
        while (currentDistance < lastDistance)
        {
            lastDistance = (thrower.transform.position - rb.transform.position).magnitude;
            yield return null;
            currentDistance = (thrower.transform.position - rb.transform.position).magnitude;
        }
        // Now at thrower
        rb.velocity = (thrower.target.transform.position - rb.transform.position).normalized*maxSpeed;
        yield return null;
    }
}
