﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Sensors : MonoBehaviour
{
    public float maxSensorDist = 50;

    public float[] sensorData;

    public Vector3[] sensorVectors;

    public int numberOfSensors;

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
            File.WriteAllText(path, "x1 x2 x3 x4 x5 x6 x7 x8 x9 y\n");
        }

        sensorData = new float[numberOfSensors];
        sensorVectors = new Vector3[numberOfSensors];
        endPoints = new Vector3[numberOfSensors];
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
        sensorVectors[0] = Quaternion.AngleAxis(-90f, Vector3.up) * transform.forward;
        sensorVectors[1] = Quaternion.AngleAxis(-45f, Vector3.up) * transform.forward;
        sensorVectors[2] = Quaternion.AngleAxis(-20f, Vector3.up) * transform.forward;
        sensorVectors[3] = Quaternion.AngleAxis(-10f, Vector3.up) * transform.forward;
        sensorVectors[4] = transform.forward;
        sensorVectors[5] = Quaternion.AngleAxis(10f, Vector3.up) * transform.forward;
        sensorVectors[6] = Quaternion.AngleAxis(20f, Vector3.up) * transform.forward;
        sensorVectors[7] = Quaternion.AngleAxis(45f, Vector3.up) * transform.forward;
        sensorVectors[8] = Quaternion.AngleAxis(90f, Vector3.up) * transform.forward;

    }

    double prevP = -5;

    public double CalculateInput()
    {

        double pred = 3.241267982404679 * Math.Pow(sensorData[0], 1) + -0.344487079535611 * Math.Pow(sensorData[1], 1) + -1.5448593393957708 * Math.Pow(sensorData[2], 1) + 0.4407907670538407 * Math.Pow(sensorData[3], 1) + -0.10131277662003413 * Math.Pow(sensorData[4], 1) + 0.21618011462851427 * Math.Pow(sensorData[5], 1) + 0.4009584098821506 * Math.Pow(sensorData[6], 1) + 0.4362948705093004 * Math.Pow(sensorData[7], 1) + -2.7036006283015013 * Math.Pow(sensorData[8], 1) + -0.5805477726389654 * Math.Pow(sensorData[0], 2) + 0.055696068882753025 * Math.Pow(sensorData[1], 2) + 0.39612963479021346 * Math.Pow(sensorData[2], 2) + -0.051716752523134346 * Math.Pow(sensorData[3], 2) + 0.07126817164862587 * Math.Pow(sensorData[4], 2) + -0.07057514697953593 * Math.Pow(sensorData[5], 2) + -0.13311791120145244 * Math.Pow(sensorData[6], 2) + -0.11195986647499012 * Math.Pow(sensorData[7], 2) + 1.9196613187086768 * Math.Pow(sensorData[8], 2) + -0.7618434038222404 * Math.Pow(sensorData[0], 3) + -3.7396512267662274E-4 * Math.Pow(sensorData[1], 3) + -0.04804555621456075 * Math.Pow(sensorData[2], 3) + 0.0023944964534887703 * Math.Pow(sensorData[3], 3) + -0.011869536862731067 * Math.Pow(sensorData[4], 3) + 0.008395484742109716 * Math.Pow(sensorData[5], 3) + 0.01764000682881317 * Math.Pow(sensorData[6], 3) + 0.01231243815611549 * Math.Pow(sensorData[7], 3) + -0.6243048822934725 * Math.Pow(sensorData[8], 3) + 0.5302594797945801 * Math.Pow(sensorData[0], 4) + -4.7071079035203474E-4 * Math.Pow(sensorData[1], 4) + 0.003340697148557309 * Math.Pow(sensorData[2], 4) + -3.459183928238019E-5 * Math.Pow(sensorData[3], 4) + 9.476034612726685E-4 * Math.Pow(sensorData[4], 4) + -5.183272839124342E-4 * Math.Pow(sensorData[5], 4) + -0.0012443523606388285 * Math.Pow(sensorData[6], 4) + -7.435391054665952E-4 * Math.Pow(sensorData[7], 4) + 0.1117950891470656 * Math.Pow(sensorData[8], 4) + -0.1637529393191578 * Math.Pow(sensorData[0], 5) + 4.117492073270638E-5 * Math.Pow(sensorData[1], 5) + -1.4611959816770168E-4 * Math.Pow(sensorData[2], 5) + -8.206909281171851E-7 * Math.Pow(sensorData[3], 5) + -4.384932940011861E-5 * Math.Pow(sensorData[4], 5) + 1.910444610125038E-5 * Math.Pow(sensorData[5], 5) + 5.2794222568657186E-5 * Math.Pow(sensorData[6], 5) + 2.74795901065248E-5 * Math.Pow(sensorData[7], 5) + -0.011760439701107184 * Math.Pow(sensorData[8], 5) + 0.02973075396312859 * Math.Pow(sensorData[0], 6) + -1.6995953739236088E-6 * Math.Pow(sensorData[1], 6) + 4.210053438712662E-6 * Math.Pow(sensorData[2], 6) + 3.072873692762859E-8 * Math.Pow(sensorData[3], 6) + 1.2736622303241627E-6 * Math.Pow(sensorData[4], 6) + -4.492390326366141E-7 * Math.Pow(sensorData[5], 6) + -1.429359695016318E-6 * Math.Pow(sensorData[6], 6) + -6.560469862715953E-7 * Math.Pow(sensorData[7], 6) + 7.428606099096589E-4 * Math.Pow(sensorData[8], 6) + -0.0033816823340706657 * Math.Pow(sensorData[0], 7) + 4.0883166351474895E-8 * Math.Pow(sensorData[1], 7) + -8.125796010154018E-8 * Math.Pow(sensorData[2], 7) + -1.1573145164372412E-11 * Math.Pow(sensorData[3], 7) + -2.397860272851949E-8 * Math.Pow(sensorData[4], 7) + 6.931874269845505E-9 * Math.Pow(sensorData[5], 7) + 2.529677538956236E-8 * Math.Pow(sensorData[6], 7) + 1.034534619866753E-8 * Math.Pow(sensorData[7], 7) + -2.8237476739139154E-5 * Math.Pow(sensorData[8], 7) + 2.3970220605420918E-4 * Math.Pow(sensorData[0], 8) + -6.047557381025686E-10 * Math.Pow(sensorData[1], 8) + 1.0414841788984723E-9 * Math.Pow(sensorData[2], 8) + -1.2703593454655757E-11 * Math.Pow(sensorData[3], 8) + 2.925180916681498E-10 * Math.Pow(sensorData[4], 8) + -7.01309730280025E-11 * Math.Pow(sensorData[5], 8) + -2.9151518533956736E-10 * Math.Pow(sensorData[6], 8) + -1.0725941168556336E-10 * Math.Pow(sensorData[7], 8) + 6.45343943729621E-7 * Math.Pow(sensorData[8], 8) + -9.949459517527645E-6 * Math.Pow(sensorData[0], 9) + 5.436648335369641E-12 * Math.Pow(sensorData[1], 9) + -8.509692120015506E-12 * Math.Pow(sensorData[2], 9) + 2.304472663567883E-13 * Math.Pow(sensorData[3], 9) + -2.2322527256051887E-12 * Math.Pow(sensorData[4], 9) + 4.4958255923224576E-13 * Math.Pow(sensorData[5], 9) + 2.1078703908176435E-12 * Math.Pow(sensorData[6], 9) + 7.042227371929799E-13 * Math.Pow(sensorData[7], 9) + -8.634574749327544E-9 * Math.Pow(sensorData[8], 9) + 2.0549383933224695E-7 * Math.Pow(sensorData[0], 10) + -2.7316506578833504E-14 * Math.Pow(sensorData[1], 10) + 4.010722696816371E-14 * Math.Pow(sensorData[2], 10) + -1.7077497336612575E-15 * Math.Pow(sensorData[3], 10) + 9.684962529728812E-15 * Math.Pow(sensorData[4], 10) + -1.6627239118197629E-15 * Math.Pow(sensorData[5], 10) + -8.68367312005687E-15 * Math.Pow(sensorData[6], 10) + -2.657207565720593E-15 * Math.Pow(sensorData[7], 10) + 6.196129595046566E-11 * Math.Pow(sensorData[8], 10) + -1.2686923726020775E-9 * Math.Pow(sensorData[0], 11) + 5.890104012616947E-17 * Math.Pow(sensorData[1], 11) + -8.287668394174151E-17 * Math.Pow(sensorData[2], 11) + 4.7537859686657515E-18 * Math.Pow(sensorData[3], 11) + -1.8232645367365733E-17 * Math.Pow(sensorData[4], 11) + 2.7128230295709836E-18 * Math.Pow(sensorData[5], 11) + 1.5547543696285968E-17 * Math.Pow(sensorData[6], 11) + 4.392631734196656E-18 * Math.Pow(sensorData[7], 11) + -1.827076951702819E-13 * Math.Pow(sensorData[8], 11) + -1.2511676382273436;

        if (prevP == -5)
        {
            prevP = pred;
        } else
        {
            while (Math.Abs(prevP - pred) > 1)
            {
                pred = pred - 1 ;
            }
        }

        return pred;
    }
}
