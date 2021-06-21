using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Sensors : MonoBehaviour
{
    [Header("Sensors")]
    public float maxSensorDist = 50;
    public int numberOfSensors;
    public LayerMask layerMask;
    public float maxVectorAngle;

    public double[] sensorData;
    public Vector3[] sensorVectors;
    private Vector3[] endPoints;

    RaycastHit raycastHit;

    [Header("Dataset Creation")]
    public bool createNewData;
    public String datasetName;
    string path;

    private void Start()
    {
        if (numberOfSensors % 2 == 0)
        {
            numberOfSensors += 1;
        }

        sensorData = new double[numberOfSensors];
        sensorVectors = new Vector3[numberOfSensors];
        endPoints = new Vector3[numberOfSensors];
        UpdateVectors();

        path = Application.dataPath + datasetName;

        if (!File.Exists(path) && createNewData)
        {
            string str = "";

            for (int i = 0; i < numberOfSensors; i++)
            {
                str += "x" + i + " ";
            }

            str += "y\n";
            File.WriteAllText(path, str);
        }
    }

    private void FixedUpdate()
    {
        UpdateVectors();
        GetSensorData();
    }

    private void LateUpdate()
    {
        printSensorData();
        DrawSensors();
    }

    private void printSensorData()
    {
        if (File.Exists(path) && createNewData)
        {
            string str = "";
            foreach (float data in sensorData)
            {
                str += data + " ";
            }

            str += Input.GetAxisRaw("Horizontal");

            str += "\n";
            File.AppendAllText(path, str);
        }
    }

    private void GetSensorData()
    {
        for (int i = 0; i < sensorVectors.Length; i++)
        {

            if (Physics.Raycast(this.transform.position, sensorVectors[i], out raycastHit, maxSensorDist, layerMask))
            {
                sensorData[i] = raycastHit.distance;
                endPoints[i] = raycastHit.point;
            } else
            {
                sensorData[i] = maxSensorDist;
                endPoints[i] = sensorVectors[i].normalized * maxSensorDist;
            }            
        }
    }

    private void DrawSensors()
    {
        for (int i = 0; i < endPoints.Length; i++)
        {
            Debug.DrawLine(this.transform.position, endPoints[i], Color.red);
        }
    }

    private void UpdateVectors()
    {
        if (numberOfSensors % 2 == 0)
        {
            numberOfSensors += 1;
        }

        sensorVectors[0] = transform.forward;

        float angle = maxVectorAngle;
        float angleOffset = maxVectorAngle / ((numberOfSensors-1)/2);

        for (int i = 1; i < numberOfSensors; i+=2)
        {
            sensorVectors[i] = Quaternion.AngleAxis(angle, Vector3.up) * transform.forward;
            sensorVectors[i + 1] = Quaternion.AngleAxis(-angle, Vector3.up) * transform.forward;
            angle -= angleOffset;
        }
    }

    public double[] getData()
    {
        return sensorData;
    }
}
