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
        if (numberOfSensors % 2 == 0)
        {
            numberOfSensors += 1;
        }

        sensorData = new float[numberOfSensors];
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
        DrawSensors();
        GetSensorData();
    }

    private void LateUpdate()
    {
        printSensorData();
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

    public double CalculateInput()
    {

        double pred = 8.591410035454361 + -0.02267512936983882 * Math.Pow(sensorData[0], 1) + 14.849852214671426 * Math.Pow(sensorData[1], 1) + -3.3790514234218842 * Math.Pow(sensorData[2], 1) + -11.374102019354954 * Math.Pow(sensorData[3], 1) + -6.137730778382346 * Math.Pow(sensorData[4], 1) + -7.474582073877049 * Math.Pow(sensorData[5], 1) + 5.093103641874085 * Math.Pow(sensorData[6], 1) + -0.0023035410656850716 * Math.Pow(sensorData[7], 1) + 0.3208892105462676 * Math.Pow(sensorData[8], 1) + -0.004646158542801939 * Math.Pow(sensorData[9], 1) + -0.03597923265335872 * Math.Pow(sensorData[10], 1) + 0.04630702104189552 * Math.Pow(sensorData[11], 1) + -0.013503626881799754 * Math.Pow(sensorData[12], 1) + 0.10860346898399226 * Math.Pow(sensorData[13], 1) + -0.13912788339072452 * Math.Pow(sensorData[14], 1) + 0.000907740121699317 * Math.Pow(sensorData[0], 2) + -4.929360179075694 * Math.Pow(sensorData[1], 2) + 1.32628731273008 * Math.Pow(sensorData[2], 2) + 4.203598018547106 * Math.Pow(sensorData[3], 2) + 2.113606469352595 * Math.Pow(sensorData[4], 2) + 1.9139300393531355 * Math.Pow(sensorData[5], 2) + -1.521997085404587 * Math.Pow(sensorData[6], 2) + 0.0009241371343109906 * Math.Pow(sensorData[7], 2) + -0.03222196555486482 * Math.Pow(sensorData[8], 2) + 0.0004761864884418853 * Math.Pow(sensorData[9], 2) + 0.0014969268607249626 * Math.Pow(sensorData[10], 2) + -0.00227089683736615 * Math.Pow(sensorData[11], 2) + 0.0008551622785842308 * Math.Pow(sensorData[12], 2) + -0.00421585019280952 * Math.Pow(sensorData[13], 2) + 0.0058624176715815724 * Math.Pow(sensorData[14], 2) + -1.587787460402833e-05 * Math.Pow(sensorData[0], 3) + 0.8033157042005211 * Math.Pow(sensorData[1], 3) + -0.2609938937301677 * Math.Pow(sensorData[2], 3) + -0.7427138304807926 * Math.Pow(sensorData[3], 3) + -0.35414893769219413 * Math.Pow(sensorData[4], 3) + -0.2407170409503839 * Math.Pow(sensorData[5], 3) + 0.2236736904834381 * Math.Pow(sensorData[6], 3) + -2.5484003276343703e-05 * Math.Pow(sensorData[7], 3) + 0.0012457460042245017 * Math.Pow(sensorData[8], 3) + -8.542514756793018e-06 * Math.Pow(sensorData[9], 3) + -3.397109204261728e-05 * Math.Pow(sensorData[10], 3) + 5.113654298782855e-05 * Math.Pow(sensorData[11], 3) + -2.4002417119500796e-05 * Math.Pow(sensorData[12], 3) + 7.54074953257522e-05 * Math.Pow(sensorData[13], 3) + -0.00011232235918087752 * Math.Pow(sensorData[14], 3) + 1.254632413516532e-07 * Math.Pow(sensorData[0], 4) + -0.06382903789761699 * Math.Pow(sensorData[1], 4) + 0.024710709118222124 * Math.Pow(sensorData[2], 4) + 0.06309235266871188 * Math.Pow(sensorData[3], 4) + 0.028857755251450665 * Math.Pow(sensorData[4], 4) + 0.014618443411032411 * Math.Pow(sensorData[5], 4) + -0.01572174408883492 * Math.Pow(sensorData[6], 4) + 2.6236490535680446e-07 * Math.Pow(sensorData[7], 4) + -2.0624012764258737e-05 * Math.Pow(sensorData[8], 4) + 6.429676169261711e-08 * Math.Pow(sensorData[9], 4) + 3.3187928949462986e-07 * Math.Pow(sensorData[10], 4) + -4.899854832984829e-07 * Math.Pow(sensorData[11], 4) + 2.4752902838542923e-07 * Math.Pow(sensorData[12], 4) + -6.188795532990898e-07 * Math.Pow(sensorData[13], 4) + 9.710011434904686e-07 * Math.Pow(sensorData[14], 4) + -3.6778224909994606e-10 * Math.Pow(sensorData[0], 5) + 0.0019872974154331313 * Math.Pow(sensorData[1], 5) + -0.0009082718455593053 * Math.Pow(sensorData[2], 5) + -0.00208013443417987 * Math.Pow(sensorData[3], 5) + -0.000903308205024933 * Math.Pow(sensorData[4], 5) + -0.00034066522862485904 * Math.Pow(sensorData[5], 5) + 0.00042059275548522734 * Math.Pow(sensorData[6], 5) + -9.215295193598649e-10 * Math.Pow(sensorData[7], 5) + 1.224556585022185e-07 * Math.Pow(sensorData[8], 5) + -1.8249668443104383e-10 * Math.Pow(sensorData[9], 5) + -1.1155130152928905e-09 * Math.Pow(sensorData[10], 5) + 1.6453496343160623e-09 * Math.Pow(sensorData[11], 5) + -8.435552256713663e-10 * Math.Pow(sensorData[12], 5) + 1.888516454329192e-09 * Math.Pow(sensorData[13], 5) + -3.077816446150905e-09 * Math.Pow(sensorData[14], 5);

        return pred;
    }
}
