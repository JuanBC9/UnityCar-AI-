using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Sensors_Digital : MonoBehaviour
{
    public float maxSensorDist = 50;

    public float[] sensorData;

    public Vector3[] sensorVectors;

    private Vector3[] endPoints;

    RaycastHit raycastHit;

    [Header("Dataset Creation")]
    public bool createNewData;
    public string datasetName = "";
    string path;

    private void Start()
    {
        path = Application.dataPath + "/" + datasetName + ".txt";
        
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

        str += Input.GetAxisRaw("Horizontal").ToString();

        if (File.Exists(path))
        {
            str += "\n";
            File.AppendAllText(path, str);
        }

        //Debug.Log(str);
    }

    private void GetSensorData()
    {
        for (int i = 0; i < sensorVectors.Length; i++)
        {
            if (Physics.Raycast(this.transform.position, sensorVectors[i], out raycastHit, maxSensorDist))
            {
                sensorData[i] = raycastHit.distance;
                endPoints[i] = raycastHit.point;
            }
            else
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

    public double SteeringInputCalculator()
    {
        double steeringValueD0 = -0.8852850206419234;

        double steeringValueD1 = 1.896534903668094 * sensorData[0]
                                -1.2652277071815028 * sensorData[1]
                                -0.2010457856672807 * sensorData[2]
                                -0.30006409313620575 * sensorData[3]
                                + 0.48756994117342245 * sensorData[4];

        double steeringValueD2 = -0.14192381048026328 * Mathf.Pow(sensorData[0], 2) 
                                + 0.06793867086215136 * Mathf.Pow(sensorData[1], 2)
                                + 0.01797455949552655 * Mathf.Pow(sensorData[2], 2)
                                + 0.04461746954713519 * Mathf.Pow(sensorData[3], 2)
                                - 0.07351089716524958 * Mathf.Pow(sensorData[4], 2);

        double steeringValueD3 =  0.0053242933453843694 * Mathf.Pow(sensorData[0], 3)
                                - 0.0018026063532814646 * Mathf.Pow(sensorData[1], 3)
                                - 0.0008302299113159112 * Mathf.Pow(sensorData[2], 3)
                                - 0.0024327270159660323 * Mathf.Pow(sensorData[3], 3)
                                + 0.00473797887233356 * Mathf.Pow(sensorData[4], 3);

        double steeringValueD4 = -0.00010615952868219817 * Mathf.Pow(sensorData[0], 4)
                                + 0.00002330094647753101 * Mathf.Pow(sensorData[1], 4)
                                + 0.000021639391214862223 * Mathf.Pow(sensorData[2], 4)
                                + 0.00006362332309231551 * Mathf.Pow(sensorData[3], 4)
                                - 0.0001493966754550788 * Mathf.Pow(sensorData[4], 4);

        double steeringValueD5 =  0.000001072637715728868 * Mathf.Pow(sensorData[0], 5)
                                - 0.00000010779567847059445 * Mathf.Pow(sensorData[1], 5)
                                - 0.0000003009253472336802 * Mathf.Pow(sensorData[2], 5)
                                - 0.0000008096545534022186 * Mathf.Pow(sensorData[3], 5)
                                + 0.0000022755767607246166 * Mathf.Pow(sensorData[4], 5);

        double steeringValueD6 = -0.000000004317716224530317 * Mathf.Pow(sensorData[0], 6)
                                - 0.00000000016362881748004007 * Mathf.Pow(sensorData[1], 6)
                                + 0.000000001726454421608149 * Mathf.Pow(sensorData[2], 6)
                                + 0.000000004048355521366342 * Mathf.Pow(sensorData[3], 6)
                                - 0.000000013391490434040537 * Mathf.Pow(sensorData[4], 6);

        double steeringValueTotal = steeringValueD0 + steeringValueD1 + steeringValueD2 + steeringValueD3 + steeringValueD4 + steeringValueD5 + steeringValueD6;
        Debug.LogWarning(steeringValueTotal);
        return steeringValueTotal;
    }
}
