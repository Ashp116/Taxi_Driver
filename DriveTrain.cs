using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriveTrain : MonoBehaviour
{
    [SerializeField] private WheelCollider FR;
    [SerializeField] private WheelCollider FL;
    [SerializeField] private WheelCollider BR;
    [SerializeField] private WheelCollider BL;

    [SerializeField] Transform FRTransform;
    [SerializeField] Transform FLTransform;
    [SerializeField] Transform BRTransform;
    [SerializeField] Transform BLTransform;
    
    [SerializeField] Renderer BrakeLights;
    [SerializeField] Material BrakeLightsOff;
    [SerializeField] Material BrakeLightsOn;
    
    [SerializeField] Transform CarMesh;

    public float acceleration = 500f;
    public float breakingForce = 300f;
    public float maxTurnAngle = 15f;
    
    private MainFrame _mainFrame;

    private float currentAcceleration = 0f;
    private float currentBreakForce = 0f;
    private float currentTurnAngle = 0f;
    // Start is called before the first frame update

    // Update is called once per frame
    void Start()
    {
        _mainFrame = gameObject.GetComponent<MainFrame>();
    }

    void FixedUpdate()
    {
        Material[] mats = BrakeLights.materials;
        

        if (Input.GetKey(KeyCode.Space))
        {
            currentBreakForce = breakingForce;
            mats[5] = BrakeLightsOn;
            BrakeLights.materials = mats;
        }
        else
        {
            currentBreakForce = 0f;
            mats[5] = BrakeLightsOff;
            BrakeLights.materials = mats;
        }
        
        if (Input.GetAxis("Vertical") == 1 && !Input.GetKey(KeyCode.Space) || Input.GetAxis("Vertical") == -1 && !Input.GetKey(KeyCode.Space))
        {
            currentBreakForce = 0f;
            currentAcceleration = acceleration * Input.GetAxis("Vertical");
        }
        else
        {
            currentBreakForce = breakingForce/2;
            currentAcceleration = acceleration * Input.GetAxis("Vertical");
            mats[5] = BrakeLightsOn;
            BrakeLights.materials = mats;
        }
        
        if(Input.GetKeyDown("r"))
        {
            var carMeshRotation = CarMesh.rotation;
            carMeshRotation.z = 0;
            carMeshRotation.x = 0;

            CarMesh.rotation = carMeshRotation;
            transform.Translate(0, 2, 0);
        }
        


        FR.motorTorque = currentAcceleration;
        FL.motorTorque = currentAcceleration;
        BR.motorTorque = currentAcceleration;
        BL.motorTorque = currentAcceleration;

        FR.brakeTorque = currentBreakForce;
        FL.brakeTorque = currentBreakForce;
        BR.brakeTorque = currentBreakForce;
        BL.brakeTorque = currentBreakForce;

        currentTurnAngle = maxTurnAngle * Input.GetAxis("Horizontal");
        FL.steerAngle = currentTurnAngle;
        FR.steerAngle = currentTurnAngle;
        
        UpdateWheel(FL, FLTransform);
        UpdateWheel(FR, FRTransform);
        UpdateWheel(BL, BLTransform);
        UpdateWheel(BR, BRTransform);
    }
    
    void UpdateWheel(WheelCollider wheel, Transform trans)
    {
        Vector3 position;
        Quaternion rotation;
        wheel.GetWorldPose(out position, out rotation);

        //trans.position = (wheel.transform.position);
        trans.rotation = rotation;
    }
}
