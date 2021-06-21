using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public int id;
    public CheckPoint nextCP;
    public List<GameObject> CPBuffer;

    private void Start()
    {
        CPBuffer = new List<GameObject>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Car") && !CPBuffer.Contains(other.gameObject))
        {
            CPBuffer.Add(other.gameObject);
            other.gameObject.GetComponent<Gen>().addCP();

            if (nextCP.id < id)
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
}
