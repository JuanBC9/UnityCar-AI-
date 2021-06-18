using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Sensors : MonoBehaviour
{
    public float maxSensorDist = 50;

    public float[] sensorData;

    public Vector3[] sensorVectors;

    private Vector3[] endPoints;

    RaycastHit raycastHit;

    [Header("Dataset Creation")]
    public bool createNewData = false;
    public String datasetName = "";
    string path;

    private void Start()
    {
        path = Application.dataPath + "/Data.txt";

        if (!File.Exists(path) && createNewData)
        {
            File.WriteAllText(path, "x1 x2 x3 x4 x5 label\n");
        }

        sensorData = new float[5];
        sensorVectors = new Vector3[5];
        endPoints = new Vector3[5];
        UpdateVectors();
    }

    private void FixedUpdate()
    {
        UpdateVectors();
        DrawSensors();
        GetSensorData();
    }

    private void LateUpdate()
    {
        printSensorData();
    }

    private void printSensorData()
    {
        string str = "";
        foreach (float data in sensorData)
        {
            str += data + " ";
        }

        if (Input.GetKey(KeyCode.A) | Input.GetKey(KeyCode.LeftArrow))
        {
            str += "STEER_LEFT";
        }
        else if (Input.GetKey(KeyCode.D) | Input.GetKey(KeyCode.RightArrow))
        {
            str += "STEER_RIGHT";
        } else 
        {
            str += "NO_STEERING";
        }

        if (File.Exists(path))
        {
            str += "\n";
            File.AppendAllText(path, str);
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
        sensorVectors[0] = Quaternion.AngleAxis(-20f, Vector3.up) * transform.forward;
        sensorVectors[1] = Quaternion.AngleAxis(-10f, Vector3.up) * transform.forward;
        sensorVectors[2] = transform.forward;
        sensorVectors[3] = Quaternion.AngleAxis(10f, Vector3.up) * transform.forward;
        sensorVectors[4] = Quaternion.AngleAxis(20f, Vector3.up) * transform.forward;
    }
}
