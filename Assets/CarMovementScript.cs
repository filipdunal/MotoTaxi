using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovementScript : MonoBehaviour
{
    [Header("Engine and braking physics")]
    public float motorForce;
    public float brakeForce;
    [Tooltip("In kilometeres per hour")] public float maxSpeed;
    public AnimationCurve torqueCurve;
    float acceleratingAxis = 0;
    float desireAcceleratingAxis = 0;
    public float accelerateButtonGravity = 1f;

    [Header("Steering physics")]
    public float steeringForce;
    public float leanMeshForce;
    public float leanMeshSmooth;
    public AnimationCurve turningCurve;
    public AnimationCurve leanCurve;
    float turningAxis = 0;
    float desireTurningAxis = 0;

    [Header("Input")]
    public float turningButtonsGravity = 1f;
    public float turningButtonsBackwardGravity = 1f;
    public float gravityDeadzone = 0.1f;
    public bool keyboardSteering;

    [Header("Physic part references")]
    public WheelCollider WheelColFront;
    public WheelCollider WheelColRear;
    public Transform handlebarTurnAxis;

    [Header("Mesh part references")]
    public Transform frontWheelMesh;
    public Transform rearWheelMesh;
    public Transform handlebarMesh;
    public Transform wholeMesh;
    public Transform localMesh;
    Rigidbody rb;

    [Header("Lights")]
    public Renderer frontLightRenderer;
    public Transform frontLight;
    public Renderer tailLightRenderer;

    Vector3 meshPositionOffset;
    Quaternion meshRotationOffset;

    Vector3 tempPosition;
    Quaternion tempRotation;
    
    private void Start()
    {
        meshPositionOffset = transform.position - wholeMesh.position;
        meshRotationOffset = transform.rotation * Quaternion.Inverse(wholeMesh.rotation);

        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0f, -0.5f, 0f);
    }
    
    void SwitchBrakeLight(bool condition)
    {
    }
    public void InputTurning(float axis)
    {
        desireTurningAxis = axis;
    }
    public void InputAccelerating(float axis)
    {
        desireAcceleratingAxis = axis;
    }

    public void InputBrake(bool condition)
    {
        if(condition)
        {
            WheelColFront.brakeTorque = brakeForce;
            WheelColRear.brakeTorque = brakeForce;
        }
        else
        {
            WheelColFront.brakeTorque = 0f;
            WheelColRear.brakeTorque = 0f;
        }
        
    }
    public void ResetPosition()
    {
        transform.position = new Vector3(3f, 3f, 3f);
        rb.velocity = Vector3.zero;
    }
    private void Update()
    {
        #region KEYBOARD STEERING
        if(keyboardSteering)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                InputAccelerating(1);
            }
            else
            {
                InputAccelerating(0);
            }
            if (Input.GetKey(KeyCode.Space))
            {
                InputBrake(true);
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                InputBrake(false);
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                InputTurning(-1);
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                InputTurning(1);
            }
            else
            {
                InputTurning(0);
            }
        }
        #endregion

        #region MOTOR
        acceleratingAxis = Mathf.Lerp(acceleratingAxis, desireAcceleratingAxis, Time.deltaTime * accelerateButtonGravity);
        float currentMotorForce = acceleratingAxis * motorForce*torqueCurve.Evaluate(GetSpeed(0,true));
        WheelColRear.motorTorque=currentMotorForce;
        #endregion

        #region TURNING
        //turningAxis = Mathf.Lerp(turningAxis, desireTurningAxis, turningButtonsGravity * Time.deltaTime);
        
        if(desireTurningAxis>turningAxis)
        {
            turningAxis = turningAxis + turningButtonsGravity * Time.deltaTime;
        }
        else if(desireTurningAxis<turningAxis)
        {
            turningAxis = turningAxis - turningButtonsGravity * Time.deltaTime;
        }
        if(Mathf.Abs(turningAxis-desireTurningAxis)<gravityDeadzone)
        {
            turningAxis = desireTurningAxis;
        }
        float currentSteeringForce = turningAxis * steeringForce;//* turningCurve.Evaluate(GetSpeed(0, true));
        WheelColFront.steerAngle = currentSteeringForce;
        #endregion
        
        //MESH THINGS
        #region WHEELS AND HANDLEBAR MESHES ROTATION
        handlebarTurnAxis.localEulerAngles = new Vector3(0f, currentSteeringForce, 0f);
        handlebarMesh.localEulerAngles = new Vector3(0f, 0f, currentSteeringForce);

        rearWheelMesh.Rotate(new Vector3(0f,-WheelColRear.rpm*60f/1000f,0f));
        frontWheelMesh.Rotate(new Vector3(0f,-WheelColFront.rpm * 60f / 1000f, 0f));

        frontWheelMesh.localEulerAngles = new Vector3(frontWheelMesh.localEulerAngles.x, (WheelColFront.steerAngle - frontWheelMesh.localEulerAngles.z)+90f, frontWheelMesh.localEulerAngles.z);
        #endregion

        #region WHOLE MESH POSITION
        wholeMesh.position = transform.position - meshPositionOffset;
        wholeMesh.rotation = transform.rotation * meshRotationOffset;
        WheelColFront.GetWorldPose(out tempPosition, out tempRotation);
        frontWheelMesh.position = tempPosition;
        //frontWheelMesh.rotation = tempRotation*Quaternion.Euler(0f,0f,90f);
        WheelColRear.GetWorldPose(out tempPosition, out tempRotation);
        rearWheelMesh.position = tempPosition;
        //rearWheelMesh.rotation = tempRotation*Quaternion.Euler(0f,0f,90f);
        #endregion

        #region LOCAL MESH LEAN
        localMesh.localRotation = Quaternion.Lerp(localMesh.localRotation, Quaternion.Euler(0f, 0f, -leanMeshForce * turningAxis*leanCurve.Evaluate(GetSpeed(0,true))), Time.deltaTime*leanMeshSmooth);
        #endregion

        //Vector3 eul = transform.rotation.eulerAngles;
        //transform.eulerAngles= new Vector3(eul.x, eul.y, 0f);
    }

    public float GetSpeed(int decimalPoints, bool returnRelativeToMaxSpeed=false)
    {
        float speed = (float)(rb.velocity.magnitude * 3.6);
        speed = (float)System.Math.Round(speed, decimalPoints);
        if(returnRelativeToMaxSpeed)
        {
            return speed / maxSpeed;
        }
        return speed;
    }
    
    //SILA OPORU??? velocity = velocity * ( 1 - deltaTime * drag);
}
