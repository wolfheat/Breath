using System;
using System.Collections;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using static UnityEngine.InputSystem.InputAction;

public class Player : MonoBehaviour
{
    [SerializeField] UIController uiController;
    [SerializeField] Inventory inventory;
    [SerializeField] PlayerPickupAreaController pickupController;
    [SerializeField] Collider itemCollider;
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
    private const float LookSensitivity = 0.15f;
    private const float RotationLowerLimit = 89;
    private const float RotationUpperLimit = 271;

    private void Start()
    {
        Debug.Log("Created Player");
        // set up input actions
        Inputs.Instance.Controls.Player.Click.performed += Click;
        Inputs.Instance.Controls.Player.RClick.performed += RClickPerformed;
        Inputs.Instance.Controls.Player.E.performed += EPickup;

    }

    private void OnTriggerEnter(Collider other)
    {
        //Entering Gravity area
        if(other.gameObject.GetComponent<GravityArea>() != null)
        {
            rb.useGravity = true;
            SoundMaster.Instance.ChangeMusicTrack(MusicTrack.Indoor);
        }
        else if (other.gameObject.GetComponent<DoorTrigger>() != null)
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
            SoundMaster.Instance.ChangeMusicTrack(MusicTrack.OutDoor);
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
        Vector3 startRot = rb.transform.rotation.eulerAngles;
        rb.angularVelocity = Vector3.zero;
        // Set all but Y direction to 0
        rb.transform.rotation = Quaternion.Euler(0, rb.transform.rotation.eulerAngles.y, 0);
        Vector3 endRot = rb.transform.rotation.eulerAngles;
        if (Mathf.Abs(startRot.x - endRot.x)>2f)
        {
            Debug.Log("Stop Rotations Changed rotations from "+startRot+" to "+endRot);
        }

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
    
    public void EPickup(CallbackContext context)
    {
        Debug.Log("E - Pick up nearby item or interact");
        // Interact with closest visible item 
        if(pickupController.ActiveItem != null)
        {
            inventory.AddItem(pickupController.ActiveItem);
            
            bool didPickUp = pickupController.PickUpActiveItem();
            Debug.Log("Did Pick Up = "+didPickUp);

            StartCoroutine(ResetItemCollider());
            // No active item here
        }

    }

    public void Click(CallbackContext context)
    {
        Debug.Log("CLICK");
        // Raycast from mouse into screen get first item thats interactable
    }

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
            rb.transform.Rotate(0,  mouseMove[0] * LookSensitivity, 0, Space.Self);
            
            // Looking up and down
            float oldAngle = tilt.localRotation.eulerAngles.x;
            float rotationAngle = (-mouseMove[1] * LookSensitivity);
            float resultAngle = oldAngle+rotationAngle;
            if (rotationAngle > 0 && oldAngle <= RotationLowerLimit+1 && oldAngle >= RotationLowerLimit-20f && resultAngle >= RotationLowerLimit)
                rotationAngle = RotationLowerLimit - oldAngle;
            else if (rotationAngle < 0 && oldAngle >= RotationUpperLimit-1 && oldAngle <= RotationUpperLimit + 20f && resultAngle<= RotationUpperLimit)
                rotationAngle = RotationUpperLimit - oldAngle;
            tilt.transform.Rotate(rotationAngle, 0,0,Space.Self);
        }        
    }

    internal void ThrowPlayer(DoorThrower doorThrower)
    {
        Debug.Log("Request To throw player");
        StartCoroutine(ThrowPlayerCO(doorThrower));
    }

    private IEnumerator ThrowPlayerCO(DoorThrower thrower)
    {
        // MAybe fixed update is needed cause of physics to have time to update correctly
        // Move player towards trigger, when close enough throw against target
        rb.velocity = (thrower.transform.position - rb.transform.position).normalized*maxSpeed;
        Debug.Log("Changing velocity towards thrower position");
        float lastDistance = (thrower.transform.position - rb.transform.position).magnitude;
        yield return null;
        float currentDistance = (thrower.transform.position - rb.transform.position).magnitude;
        Debug.Log("Distance change "+ lastDistance+" -> "+ currentDistance);

        // Still not correclty going towrds thow position fix

        while (currentDistance < lastDistance)
        {
            lastDistance = (thrower.transform.position - rb.transform.position).magnitude;
            yield return null;
            currentDistance = (thrower.transform.position - rb.transform.position).magnitude;
            Debug.Log("Distance change " + lastDistance + " -> " + currentDistance);
        }
        rb.transform.position = thrower.transform.position;
        Debug.Log("Close set to thrower position" + rb.transform.position);
        yield return new WaitForFixedUpdate();
        // Now at thrower
        rb.velocity = (thrower.target.transform.position - rb.transform.position).normalized*maxSpeed*3;
        yield return new WaitForFixedUpdate();
    }

    public IEnumerator ResetItemCollider()
    {
        itemCollider.enabled = false;
        yield return null;
        itemCollider.enabled = true;
    }
}
