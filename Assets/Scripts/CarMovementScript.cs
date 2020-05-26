using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMovementScript : MonoBehaviour
{
    [Header("Engine and braking physics")]

    public float motorForce;
    public float brakeForce;
    [Tooltip("In kilometeres per hour")] public float maxSpeed;
    //public AnimationCurve torqueCurve;
    float acceleratingAxis = 0;
    float desireAcceleratingAxis = 0;
    [SerializeField] List<Gear> gears;
    [HideInInspector] public Gear actualGear;
    bool changingGear;
    public float changeGearTime=0f;
    [System.Serializable]
    public class Gear
    {
        public int number;
        public float gearRatio;
    }
    //float currentMotorForce;
    public float maxRPM;
    public float minRPM;
    public float tachometerMaxRPM;
    [HideInInspector] public float engineRPM;
    [HideInInspector] public float engineRPMsmooth;
    public float rpmLerp;
    public float neutralGearRPM;



    [Header("Sound")]

    public AnimationCurve engineSoundPitch;
    public AudioSource engineSoundSource;
    public float engineSoundPitchLerp;
    float desirePitch;



    [Header("Steering physics")]

    public float steeringForce;
    public float leanMeshForce;
    public float leanMeshSmooth;
    public AnimationCurve turningCurve;
    public AnimationCurve leanCurve;
    public float turningAxis = 0;
    float desireTurningAxis = 0;



    [Header("Input")]

    public float accelerateButtonGravity = 1f;
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
    Vector3 meshPositionOffset;
    Quaternion meshRotationOffset;
    Vector3 tempPosition;
    Quaternion tempRotation;
    Rigidbody rb;



    [Header("Lights")]

    public GameObject frontLight;
    public GameObject tailLight;
    public GameObject brakeLight;

    private void Awake()
    {
        //wholeMesh.parent = null; // Detach of mesh
        //GetComponent<Rigidbody>().ResetCenterOfMass();
        //GetComponent<Rigidbody>().ResetInertiaTensor();
    }
    private void Start()
    {
        meshPositionOffset = transform.position - wholeMesh.position;
        meshRotationOffset = transform.rotation * Quaternion.Inverse(wholeMesh.rotation);

        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0f, -0.2f, 0f);
        actualGear = gears[0];
        //StartCoroutine(ResetRotationZ());
    }
    
    public void SwitchLights(bool condition)
    {
        frontLight.SetActive(condition);
        tailLight.SetActive(condition);
    }
    public void SwitchLights()
    {
        frontLight.SetActive(!frontLight.activeInHierarchy);
        tailLight.SetActive(!tailLight.activeInHierarchy);
    }

    void SwitchBrakeLight(bool condition)
    {
        brakeLight.SetActive(condition);
    }

    public void InputTurning(float axis)
    {
        desireTurningAxis = axis;
    }
    public void InputAccelerating(float axis)
    {
        desireAcceleratingAxis = axis;
    }

    bool reversing;
    public void InputBrake(bool condition)
    {
        //Debug.Log("Brake hit");
        if (GetSpeed(0) < 10f && Mathf.Abs(acceleratingAxis) < 0.1f)
        {
            reversing = condition;
        }
        else
        {
            reversing = false;
        }

        if (condition)
        {
            if(reversing)
            {
                WheelColRear.motorTorque = -550f;
                WheelColRear.brakeTorque = 0f;
                WheelColFront.brakeTorque = 0f;
            }
            else
            {
                WheelColFront.brakeTorque = brakeForce;
                WheelColRear.brakeTorque = brakeForce;
            }
        }
        else
        {
            WheelColFront.brakeTorque = 0f;
            WheelColRear.brakeTorque = 0f;
        }
        SwitchBrakeLight(condition);
    }
    public void ResetPosition()
    {
        transform.position = new Vector3(3f, 3f, 3f);
        transform.rotation = Quaternion.identity;
        rb.velocity = Vector3.zero;
    }

    IEnumerator GearShiftCoroutine(int gearChange, bool forceNow)
    {
        if(!forceNow)
        {
            changingGear = true;
            yield return new WaitForSeconds(changeGearTime);
            changingGear = false;
        }
        int lookForGear = actualGear.number + gearChange;
        foreach(Gear g in gears)
        {
            if(g.number==lookForGear)
            {
                actualGear = g;
                yield break;
            }
        }
    }

    void GearShift(int gearChange, bool forceNow=false)
    {
        if(!changingGear)
        {
            StartCoroutine(GearShiftCoroutine(gearChange,forceNow));
        }
    }

    private void Update()
    {
        #region KEYBOARD STEERING
        if (keyboardSteering)
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
        if(changingGear)
        {
            desireAcceleratingAxis = 0f;
        }
        acceleratingAxis = Mathf.Lerp(acceleratingAxis, desireAcceleratingAxis, Time.deltaTime * accelerateButtonGravity);

        float currentMotorForce = acceleratingAxis * motorForce * actualGear.gearRatio;//*torqueCurve.Evaluate(GetSpeed(0,true));
        if (!reversing)
        {
            WheelColRear.motorTorque = currentMotorForce;
        }
        if (actualGear.number == 0)
        {
            engineRPM = neutralGearRPM;
            if (desireAcceleratingAxis > 0f)
            {
                GearShift(1,true);
            }
        }
        else
        {
            engineRPM = WheelColRear.rpm * actualGear.gearRatio;
        }

        //Debug.Log(engineRPM);
        if (engineRPM > maxRPM)
        {
            GearShift(1);
        }
        else if (engineRPM < minRPM)
        {
            if (actualGear.number == 1)
            {
                if (desireAcceleratingAxis == 0f)
                {
                    GearShift(-1);
                }
            }
            else
            {
                GearShift(-1);
            }

        }
        engineRPMsmooth = Mathf.Lerp(engineRPMsmooth, engineRPM, rpmLerp * Time.deltaTime);



        #endregion

        #region TURNING
        //turningAxis = Mathf.Lerp(turningAxis, desireTurningAxis, turningButtonsGravity * Time.deltaTime);

        if (desireTurningAxis > turningAxis)
        {
            if (Mathf.Abs(desireTurningAxis) < gravityDeadzone)
            {
                turningAxis = turningAxis + turningButtonsBackwardGravity * Time.deltaTime;
            }
            else
            {
                turningAxis = turningAxis + turningButtonsGravity * Time.deltaTime;
            }
            
        }
        else if (desireTurningAxis < turningAxis)
        {
            turningAxis = turningAxis - turningButtonsGravity * Time.deltaTime;
        }
        if (Mathf.Abs(turningAxis - desireTurningAxis) < gravityDeadzone)
        {
            turningAxis = desireTurningAxis;
        }
        float currentSteeringForce = turningAxis * steeringForce * turningCurve.Evaluate(GetSpeed(0, true));
        WheelColFront.steerAngle = currentSteeringForce;
        #endregion

        //MESH THINGS
        #region WHEELS AND HANDLEBAR MESHES ROTATION
        handlebarTurnAxis.localEulerAngles = new Vector3(0f, currentSteeringForce, 0f);
        handlebarMesh.localEulerAngles = new Vector3(0f, 0f, currentSteeringForce);

        rearWheelMesh.Rotate(new Vector3(0f, -WheelColRear.rpm * 60f / 1000f * Time.timeScale, 0f));
        frontWheelMesh.Rotate(new Vector3(0f, -WheelColFront.rpm * 60f / 1000f * Time.timeScale, 0f));

        frontWheelMesh.localEulerAngles = new Vector3(frontWheelMesh.localEulerAngles.x, (WheelColFront.steerAngle - frontWheelMesh.localEulerAngles.z) + 90f, frontWheelMesh.localEulerAngles.z);
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
        localMesh.localRotation = Quaternion.Lerp(localMesh.localRotation, Quaternion.Euler(0f, 0f, -leanMeshForce * turningAxis * leanCurve.Evaluate(GetSpeed(0, true))), Time.deltaTime * leanMeshSmooth);
        #endregion
        
        #region SOUND
        float speed = GetSpeed(0);
        if (Time.timeScale != 0f)
        {
            desirePitch = engineSoundPitch.Evaluate(engineRPMsmooth);
            engineSoundSource.pitch = Mathf.Lerp(engineSoundSource.pitch, desirePitch, Time.deltaTime * engineSoundPitchLerp);
        }
        #endregion
    }

    private void FixedUpdate()
    {
        transform.Rotate(0f, 0f, -transform.localEulerAngles.z);
    }
    IEnumerator ResetRotationZ()
    {
        while(true)
        {
            transform.Rotate(0f, 0f, -transform.localEulerAngles.z);
            yield return new WaitForSecondsRealtime(0.5f);
        }
        
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
