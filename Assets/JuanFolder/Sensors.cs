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

        str += Input.GetAxisRaw("Horizontal");

        if (File.Exists(path))
        {
            str += "\n";
            File.AppendAllText(path, str);
        }
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

    public double CalculateInput()
    {
        double t0 = 0.6386575956463246;

        double c1 = + 0.013952981592857199 * sensorData[0]
                    - 0.1195329998878619 * sensorData[1]
                    + 0.020297015305557636 * sensorData[2]
                    + 0.035479849034045045 * sensorData[3]
                    + 0.008275173430363278 * sensorData[4];

        double c2 = - 0.0005060510619638572 * Math.Pow(sensorData[0],2)
                    + 0.002678589929073044 * Math.Pow(sensorData[1], 2)
                    - 0.0004286412399268085 * Math.Pow(sensorData[2], 2)
                    - 0.0007913978843288252 * Math.Pow(sensorData[3], 2)
                    - 0.00019591722190083708 * Math.Pow(sensorData[4], 2);

        double c3 = 0.000004354848783281807 * Math.Pow(sensorData[0], 3)
                    - 0.000018867866472499372 * Math.Pow(sensorData[1], 3)
                    + 0.0000030935655278745724 * Math.Pow(sensorData[2], 3)
                    + 0.000005342223069005512 * Math.Pow(sensorData[3], 3)
                    + 0.0000014809149881643422 * Math.Pow(sensorData[4], 3);

        double pred = t0 + c1 + c2 + c3;

        Debug.Log(pred);

        return pred;
    }
}
