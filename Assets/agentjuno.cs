using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class agentjuno : Agent
{
    // Start is called before the first frame update
    Rigidbody rb;
    public Transform target;
    public float speed = 10f;
    public float jumpstrength = 5f;
    public float sidespeed = 5f;
    private bool cubeonground = true;
    public bool crossedloop = false;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin()
    {
        this.transform.localPosition = new Vector3(0.5f, 0.5f, 3.5f);
        this.transform.localEulerAngles = new Vector3(0, 180, 0);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(this.transform.localPosition);
        sensor.AddObservation(target.localPosition);
        sensor.AddObservation(target.localScale);
        sensor.AddObservation(this.transform.localEulerAngles);
    }
    /// <summary>
    /// 0 is for jump
    /// 1 is for side movement
    /// 2 is for forward movement
    /// </summary>
    /// <param name="vectorAction"></param>
    public override void OnActionReceived(float[] vectorAction)
    {
        if (vectorAction[0] == 1)
        {
            jump();
        }
        sidemove(vectorAction[1]);
        forwardmove(vectorAction[2]);

        if (crossedloop && !cubeonground)
        {
            SetReward(1.0f);
        }

        if (this.transform.localPosition.y < 0)
        {
            EndEpisode();
        }
    }
    void jump()
    {
        if (cubeonground)
        {
            rb.AddForce(new Vector3(0, jumpstrength, 0), ForceMode.Impulse);
            cubeonground = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "floor")
        {
            cubeonground = true;
            crossedloop = false;
        }
    }

    void sidemove(float numure)
    {
        if (numure == 2)
        {
            this.transform.Translate(sidespeed * Time.deltaTime, 0, 0, Space.Self);
        }
        else
        {
            this.transform.Translate(-numure*sidespeed * Time.deltaTime, 0, 0, Space.Self);
        }
    }
    void forwardmove(float numure)
    {
        if(numure == 2)
        {
            this.transform.Translate(0, 0, Time.deltaTime * sidespeed, Space.Self);
        }
        else
        {
            this.transform.Translate(0, 0, -numure * Time.deltaTime * sidespeed, Space.Self);
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "trigger")
        {
            crossedloop = true;
        }
    }

    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = Input.GetAxis("Jump");
        actionsOut[1] = Input.GetAxis("Horizontal");
        actionsOut[2] = Input.GetAxis("Vertical");
    }
}
