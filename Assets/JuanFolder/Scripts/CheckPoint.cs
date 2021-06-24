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
        if (other.gameObject.tag.Equals("Car") && !CPBuffer.Contains(other.gameObject) && hasPassedPreviousCP(other.gameObject))
        {
            CPBuffer.Add(other.gameObject);
            other.gameObject.GetComponent<Gen>().addCP();

            if (nextCP.id == 0)
            {
                nextLapSignal(other.gameObject);
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
        if (id == 0 && CPBuffer.Contains(car))
        {
            return true;
        }

        return (prevCP.hasPassedPreviousCP(car) && CPBuffer.Contains(car));
    }
}
