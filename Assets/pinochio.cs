using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using System;
using Unity.Mathematics;

public class pinochio : Agent
{
    // Start is called before the first frame update
    Rigidbody rb;
    public butt BU;
    public Transform target;
    public Transform ring;
    public Transform butto;
    public float speed = 10f;
    public float jumpstrength = 5f;
    public float sidespeed = 5f;
    public bool cubeonground = true;
    public bool crossedloop = false;
    public bool hitpad = false;
    public bool isjumping = false;
    public bool buttonpressed = false;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin()
    {
        this.transform.localPosition = new Vector3(0.5f, 0.5f, 3.5f);
        this.transform.localEulerAngles = new Vector3(0, 180, 0);
        ring.transform.localPosition = new Vector3(5.51f - UnityEngine.Random.value*6, 2.31f + UnityEngine.Random.value*3, 0);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(this.transform.localPosition);//3
        sensor.AddObservation(target.localPosition);//3
        sensor.AddObservation(target.localScale);//3
        sensor.AddObservation(this.transform.localEulerAngles);//3
        sensor.AddObservation(ring.transform.localPosition);//3
        sensor.AddObservation(BU.getmaterial());//1
        sensor.AddObservation(butto.transform.localPosition);//3

    }
    /// <summary>
    /// 0 is for jump
    /// 1 is for left movement
    ///
    /// 2 is for forward movement
    /// 
    /// </summary>
    /// <param name="vectorAction"></param>
    public override void OnActionReceived(float[] vectorAction)
    {
        /*
        if (vectorAction[0] == 1)
        {
            jump();
        }*/
        jump(Math.Abs(vectorAction[0]));

        forwardmove(vectorAction[2]);

        moveleft(vectorAction[1]);

        chekcbutton();

        rewardsequence();

        if (this.transform.localPosition.y < 0)
        {
            EndEpisode();
        }
    }

    private void rewardsequence()
    {
        if (crossedloop && hitpad && BU.getmaterial() == 0)
        {
            SetReward(1.0f);
            hitpad = false;
            buttonpressed = false;
        }
    }

    private void chekcbutton()
    {
        if (BU.getmaterial() == 1)
        {
            buttonpressed = false;
        }
        else if (BU.getmaterial() == 0)
        {
            buttonpressed = true;
        }
    }

    /*private void moveright(float v)
    {
        this.transform.Translate(sidespeed * Time.deltaTime, 0, 0, Space.Self);
    }*/

    private void moveleft(float v)
    {
        this.transform.Translate(v*sidespeed * Time.deltaTime, 0, 0, Space.Self);
    }

    void jump(float numure)
    {
        if (cubeonground && numure >0.5)
        {
            rb.AddForce(new Vector3(0, numure*jumpstrength, 0), ForceMode.Impulse);
            cubeonground = false;
            isjumping = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "floor")
        {
            cubeonground = true;
            crossedloop = false;
            hitpad = false;
            isjumping = false;
        }

        if (collision.gameObject.name == "landpad")
        {
            hitpad = true;
            isjumping = false;
            buttonpressed = false;
        }
    }
    /*void sidemove(float numure)
    {
        if (numure == 2)
        {
            this.transform.Translate(sidespeed * Time.deltaTime, 0, 0, Space.Self);
        }
        else
        {
            this.transform.Translate(-numure * sidespeed * Time.deltaTime, 0, 0, Space.Self);
        }
    }*/
    void forwardmove(float numure)
    {
        /*if (numure == 2)
        {
            this.transform.Translate(0, 0, Time.deltaTime * sidespeed, Space.Self);
        }
        else
        {
            this.transform.Translate(0, 0, -numure * Time.deltaTime * sidespeed, Space.Self);
        }*/
        this.transform.Translate(0, 0, numure*Time.deltaTime * sidespeed, Space.Self);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "trig")
        {
            crossedloop = true;
        }
    }

    public override void Heuristic(float[] actionsOut)//todo change this too acc to code
    {
        actionsOut[0] = Input.GetAxis("Jump");
        actionsOut[1] = Input.GetAxis("Horizontal");
        actionsOut[2] = Input.GetAxis("Vertical");
    }
}
