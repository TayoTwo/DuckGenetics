using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Duck : MonoBehaviour
{
    public float fitness;
    //Run Speed, Turn Speed, Vision Angle
    public float[] genome;
    public float eyeRange;
    float runSpeed, turnSpeed, visionAngle;
    public LayerMask duckMask;
    public MeshRenderer[] bodyParts;
    Rigidbody rb;

    void Awake(){

        duckMask = ~duckMask;
        rb = GetComponent<Rigidbody>();
        OnSpawn(genome);

    }

    // Start is called before the first frame update
    public void OnSpawn(float[] g){

        genome = g;

        runSpeed = genome[0];
        turnSpeed = genome[1];
        visionAngle = genome[2];

        MeshRenderer[] meshRenderers = bodyParts;

        foreach(MeshRenderer mesh in meshRenderers){

            Color c = new Color((g[0]/2) + 0.5f,(g[1]/2) + 0.5f,(g[2]/2) + 0.5f);

            mesh.material.color = c;

        }
        
    }

    void Run(){

        rb.AddForce(transform.forward * runSpeed,ForceMode.Acceleration);

    }

    void Turn(){

        RaycastHit leftEye;
        RaycastHit rightEye;

        Vector3 leftDir = transform.forward + (-transform.right * visionAngle);
        Vector3 righDir = transform.forward + (transform.right * visionAngle);

        leftDir.Normalize();
        righDir.Normalize();

        float turnDir = 0;

        if(Physics.Raycast(transform.position,leftDir,out leftEye,eyeRange,duckMask)){

            string tag = leftEye.transform.root.tag;
            float angleBetween = Vector3.Angle(transform.forward,leftEye.transform.position);

            if(tag == "Plant"){

                turnDir -= angleBetween / 90;

            } else if(tag == "Stage"){

                turnDir += angleBetween / 90;

            }

            //Debug.Log("LEFT EYE TRIGGERED " + angleBetween);

        }

        if(Physics.Raycast(transform.position,righDir,out rightEye,eyeRange,duckMask)){

            string tag = rightEye.transform.root.tag;
            float angleBetween = Vector3.Angle(transform.forward,rightEye.transform.position);

            if(tag == "Plant"){

                turnDir += angleBetween / 90;

            } else if(tag == "Stage"){

                turnDir -= angleBetween / 90;

            }

            //Debug.Log("RIGHT EYE TRIGGERED " + angleBetween);

        }

        if(turnDir != 0){

            rb.AddTorque(Vector3.up * turnDir * turnSpeed);

        } else {

            rb.angularVelocity = Vector3.zero;

        }


    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Run();
        Turn();
        
    }

    void OnDrawGizmos(){

        Vector3 leftDir = transform.forward + (-transform.right * visionAngle);
        Vector3 righDir = transform.forward + (transform.right * visionAngle);
        
        leftDir.Normalize();
        righDir.Normalize();

        leftDir *= eyeRange;
        righDir *= eyeRange;

        Debug.DrawLine(transform.position,transform.position + leftDir);
        Debug.DrawLine(transform.position,transform.position + righDir);

    }

    void OnTriggerEnter(Collider c){

        if(c.tag == "Plant"){

            fitness++;
            Destroy(c.gameObject);

        }

    }

}
