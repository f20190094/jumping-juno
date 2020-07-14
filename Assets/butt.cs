using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public class butt : MonoBehaviour
{
    // Start is called before the first frame update
    public Material[] material;
    Renderer rend;
    public pinochio pino;
    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.enabled = true;
        //meterialstart();
    }

    public void meterialstart()
    {
        rend.sharedMaterial = material[0];
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name=="agent pino")
        {
            rend.sharedMaterial = material[1];
            pino.buttonpressed = true;
        }
    }
     public int getmaterial()
    {
        if (rend.sharedMaterial == material[0])
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }
    private void Update()
    {
        if (pino.buttonpressed == false)
        {
            rend.sharedMaterial = material[0];
        }
    }
}
