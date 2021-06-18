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
    public bool createNewData;
    public String datasetName;
    string path;

    private void Start()
    {
        path = Application.dataPath + datasetName;

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
        double pred = (0.029728605686488052 * Math.Pow(sensorData[0], 1)) + (0.31582283752976803 * Math.Pow(sensorData[1], 1)) + (0.3379645722216136 * Math.Pow(sensorData[2], 1)) + (-0.31012062810382446 * Math.Pow(sensorData[3], 1)) + (0.22041839338747593 * Math.Pow(sensorData[4], 1)) + (-0.007937983209747201 * Math.Pow(sensorData[0], 2)) + (-0.04193048673075994 * Math.Pow(sensorData[1], 2)) + (-0.08767333744967631 * Math.Pow(sensorData[2], 2)) + (0.0324459352086901 * Math.Pow(sensorData[3], 2)) + (-0.04907065098860164 * Math.Pow(sensorData[4], 2)) + (0.0022053252028608862 * Math.Pow(sensorData[0], 3)) + (0.0020820045775490556 * Math.Pow(sensorData[1], 3)) + (0.008809368106456675 * Math.Pow(sensorData[2], 3)) + (-7.333627995786685E-4 * Math.Pow(sensorData[3], 3)) + (0.004838499822825734 * Math.Pow(sensorData[4], 3)) + (-2.335313798718595E-4 * Math.Pow(sensorData[0], 4)) + (-2.727314408036936E-5 * Math.Pow(sensorData[1], 4)) + (-4.7482813823532016E-4 * Math.Pow(sensorData[2], 4)) + (-5.951795051925069E-5 * Math.Pow(sensorData[3], 4)) + (-2.6215147223800626E-4 * Math.Pow(sensorData[4], 4)) + (1.235756569280291E-5 * Math.Pow(sensorData[0], 5)) + (-1.4980758042929277E-6 * Math.Pow(sensorData[1], 5)) + (1.5410321148811432E-5 * Math.Pow(sensorData[2], 5)) + (4.481658469917961E-6 * Math.Pow(sensorData[3], 5)) + (8.626808573408398E-6 * Math.Pow(sensorData[4], 5)) + (-3.7340372631710633E-7 * Math.Pow(sensorData[0], 6)) + (7.717048933703871E-8 * Math.Pow(sensorData[1], 6)) + (-3.158283809231436E-7 * Math.Pow(sensorData[2], 6)) + (-1.3485217836675063E-7 * Math.Pow(sensorData[3], 6)) + (-1.8014064307554134E-7 * Math.Pow(sensorData[4], 6)) + (6.752496556851329E-9 * Math.Pow(sensorData[0], 7)) + (-1.616763871029692E-9 * Math.Pow(sensorData[1], 7)) + (4.121069857414492E-9 * Math.Pow(sensorData[2], 7)) + (2.2304097061902817E-9 * Math.Pow(sensorData[3], 7)) + (2.402263038076845E-9 * Math.Pow(sensorData[4], 7)) + (-7.22750430197977E-11 * Math.Pow(sensorData[0], 8)) + (1.8117541246453093E-11 * Math.Pow(sensorData[1], 8)) + (-3.321924334072646E-11 * Math.Pow(sensorData[2], 8)) + (-2.1201974994094306E-11 * Math.Pow(sensorData[3], 8)) + (-1.9827817905275814E-11 * Math.Pow(sensorData[4], 8)) + (4.217398062166416E-13 * Math.Pow(sensorData[0], 9)) + (-1.0640813116936322E-13 * Math.Pow(sensorData[1], 9)) + (1.508272570262654E-13 * Math.Pow(sensorData[2], 9)) + (1.0880100349498484E-13 * Math.Pow(sensorData[3], 9)) + (9.227577681464617E-14 * Math.Pow(sensorData[4], 9)) + (-1.0318641141411752E-15 * Math.Pow(sensorData[0], 10)) + (2.575266560010261E-16 * Math.Pow(sensorData[1], 10)) + (-2.950036158737933E-16 * Math.Pow(sensorData[2], 10)) + (-2.3395513344259535E-16 * Math.Pow(sensorData[3], 10)) + (-1.850576765443999E-16 * Math.Pow(sensorData[4], 10)) + -0.06175517161176458;

        return pred;
    }
}
