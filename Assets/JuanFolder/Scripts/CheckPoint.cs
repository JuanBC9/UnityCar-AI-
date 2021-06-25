using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public int id;
    public CheckPoint nextCP;
    public CheckPoint prevCP;
    public List<GameObject> CPBuffer;

    private void Start()
    {
        CPBuffer = new List<GameObject>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Car"))
        {
            if (!CPBuffer.Contains(other.gameObject) && hasPassedPreviousCP(other.gameObject))
            {
                CPBuffer.Add(other.gameObject);
                other.gameObject.GetComponent<MotorSphereScript>().controller.gen.addFitness(1);

                if (nextCP.id == 0)
                {
                    nextLapSignal(other.gameObject);
                }
            }
        }
    }

    private void nextLapSignal(GameObject car)
    {
        if (nextCP.CPBuffer.Contains(car))
        {
            nextCP.CPBuffer.Remove(car);
            nextCP.nextLapSignal(car);
        }
    }

    private bool hasPassedPreviousCP(GameObject car)
    {
        if (id == 0)
        {
            return true;
        }

        return prevCP.CPBuffer.Contains(car);
    }
}
