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
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin()
    {
        this.transform.localPosition = new Vector3(-0.5f, 0.5f, 3.5f);
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
        }
    }

    void sidemove(float numure)
    {
        numure--;
        this.transform.Translate(sidespeed * numure*Time.deltaTime, 0, 0, Space.Self);
    }
    void forwardmove(float numure)
    {
        this.transform.Translate(0, 0, numure * Time.deltaTime * sidespeed, Space.Self);
    }
}
