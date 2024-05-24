using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Content.Interaction;
using UnityEngine.XR.Interaction.Toolkit;

public class CarControl : MonoBehaviour
{
    [SerializeField]
    private XRKnob steeringWheel;

    [SerializeField]
    private Transform leftFrontTire;

    [SerializeField]
    private Transform rightFrontTire;

    [SerializeField]
    [Tooltip("Maximum steering angle for the tires in degrees")]
    private float maxSteeringAngle = 30f;

    [SerializeField]
    [Tooltip("The transform of the XR Origin (or the object to move)")]
    private Transform xrOrigin;

    [SerializeField]
    [Tooltip("Movement speed of the XR Origin")]
    private float movementSpeed = 5f;

    [SerializeField]
    [Tooltip("Turning speed of the XR Origin")]
    private float turningSpeed = 10f;

    [SerializeField]
    private Rigidbody carRigidbody;

    [SerializeField]
    [Tooltip("Braking force to apply when the brake button is pressed")]
    private float brakeForce = 50f;

    private InputAction rightTriggerAction;
    private InputAction leftTriggerAction;
    private InputAction brakeAction;

    private void Start()
    {
        // Initialize Input Actions
        rightTriggerAction = new InputAction("RightTrigger", binding: "<XRController>{RightHand}/trigger");
        rightTriggerAction.Enable();

        leftTriggerAction = new InputAction("LeftTrigger", binding: "<XRController>{LeftHand}/trigger");
        leftTriggerAction.Enable();

        brakeAction = new InputAction("Brake", binding: "<XRController>{RightHand}/buttonSouth"); // X button on most XR controllers
        brakeAction.Enable();
    }

    private void Update()
    {
        UpdateTireSteering();
    }

    private void FixedUpdate()
    {
        HandleMovement();
        HandleSteering();
        HandleBraking();
    }

    private void UpdateTireSteering()
    {
        // Get the current value from the steering wheel knob
        float steeringValue = steeringWheel.value;

        // Calculate the steering angle based on the steering wheel value
        float steeringAngle = Mathf.Lerp(-maxSteeringAngle, maxSteeringAngle, steeringValue);

        // Apply the steering angle to the front tires
        leftFrontTire.localEulerAngles = new Vector3(leftFrontTire.localEulerAngles.x, steeringAngle, leftFrontTire.localEulerAngles.z);
        rightFrontTire.localEulerAngles = new Vector3(rightFrontTire.localEulerAngles.x, steeringAngle, rightFrontTire.localEulerAngles.z);
    }

    private void HandleMovement()
    {
        // Get trigger values
        float triggerValue = rightTriggerAction.ReadValue<float>();
        float ltriggerValue = leftTriggerAction.ReadValue<float>();

        // Move the car forward or backward based on the trigger input
        if (triggerValue > 0.1f)
        {
            Vector3 forwardForce = xrOrigin.forward * movementSpeed * triggerValue;
            carRigidbody.AddForce(forwardForce, ForceMode.Force);
        }
        else if (ltriggerValue > 0.1f)
        {
            Vector3 backwardForce = xrOrigin.forward * movementSpeed * ltriggerValue * -1;
            carRigidbody.AddForce(backwardForce, ForceMode.Force);
        }
    }

    private void HandleSteering()
    {
        // Get the current value from the steering wheel knob
        float steeringValue = steeringWheel.value;

        // Calculate the steering angle based on the steering wheel value
        float steeringAngle = Mathf.Lerp(-maxSteeringAngle, maxSteeringAngle, steeringValue);

        // Calculate the desired turning direction
        float turnTorque = steeringAngle * turningSpeed * Time.fixedDeltaTime;

        // Apply torque to the Rigidbody to turn the car smoothly
        carRigidbody.AddTorque(Vector3.up * turnTorque, ForceMode.Acceleration);
    }

    private void HandleBraking()
    {
        // Check if the brake button is pressed
        if (brakeAction.ReadValue<float>() > 0.1f)
        {
            // Apply a braking force opposite to the current velocity
            Vector3 brakingForce = -carRigidbody.velocity * brakeForce * Time.fixedDeltaTime;
            carRigidbody.AddForce(brakingForce, ForceMode.Force);

            // Apply a damping force to reduce angular velocity
            Vector3 angularDampingForce = -carRigidbody.angularVelocity * brakeForce * Time.fixedDeltaTime;
            carRigidbody.AddTorque(angularDampingForce, ForceMode.Force);

            // Optional: If you want to stop immediately without sliding, you can set velocities to zero directly
            // carRigidbody.velocity = Vector3.zero;
            // carRigidbody.angularVelocity = Vector3.zero;
        }
    }

    private void OnDisable()
    {
        rightTriggerAction.Disable();
        leftTriggerAction.Disable();
        brakeAction.Disable();
    }
    public void Neutral()
    {
        steeringWheel.value = 0.5f;
    }
}
