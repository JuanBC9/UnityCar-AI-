using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensors : MonoBehaviour
{
    public float maxSensorDist = 50;

    public float[] sensorData;

    public Vector3[] sensorVectors;

    RaycastHit raycastHit;

    private void Start()
    {
        sensorData = new float[5];
        sensorVectors = new Vector3[5];
        UpdateVectors();
    }

    private void FixedUpdate()
    {
        UpdateVectors();
        DrawSensors();
        GetSensorData();
        printSensorData();
    }

    private void printSensorData()
    {
        string str = "";
        foreach (float data in sensorData)
        {
            str += data + ", ";
        }

        Debug.Log(str);
    }

    private void GetSensorData()
    {
        for (int i = 0; i < sensorVectors.Length; i++)
        {
            if (Physics.Raycast(this.transform.position, sensorVectors[i], out raycastHit, maxSensorDist))
            {
                sensorData[i] = raycastHit.distance;
            } else
            {
                sensorData[i] = maxSensorDist;
            }
        }
    }

    private void DrawSensors()
    {
        foreach (Vector3 vector in sensorVectors)
        {
            Debug.DrawRay(this.transform.position, this.transform.forward, Color.red);
        }
    }

    private void UpdateVectors()
    {
        sensorVectors[0] = transform.forward - (Vector3.forward * .4f);
        sensorVectors[1] = transform.forward - (Vector3.forward * .2f);
        sensorVectors[2] = transform.forward;
        sensorVectors[3] = transform.forward + (Vector3.forward * .2f);
        sensorVectors[4] = transform.forward + (Vector3.forward * .4f);
    }
}
