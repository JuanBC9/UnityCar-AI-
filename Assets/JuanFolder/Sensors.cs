﻿using System;
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

        double pred = -8.206878623612283 * Math.Pow(sensorData[0], 1) + -4931.25954407049 * Math.Pow(sensorData[1], 1) + -33051.65978842229 * Math.Pow(sensorData[2], 1) + 2923.8292136488017 * Math.Pow(sensorData[3], 1) + 3073.838557615876 * Math.Pow(sensorData[4], 1) + 13.524117437267705 * Math.Pow(sensorData[5], 1) + 32259.404572190717 * Math.Pow(sensorData[6], 1) + -8.662476359331777 * Math.Pow(sensorData[7], 1) + -9716.900575175881 * Math.Pow(sensorData[8], 1) + 4.083832657287417 * Math.Pow(sensorData[9], 1) + 258.81851511169225 * Math.Pow(sensorData[10], 1) + 4.668919014955463 * Math.Pow(sensorData[11], 1) + 13.161224370938726 * Math.Pow(sensorData[12], 1) + -4.367868607354467 * Math.Pow(sensorData[13], 1) + -5.4736459566047415 * Math.Pow(sensorData[14], 1) + 1.7081611419303044 * Math.Pow(sensorData[0], 2) + 9580.966080708436 * Math.Pow(sensorData[1], 2) + 54818.86846143042 * Math.Pow(sensorData[2], 2) + -5378.945951129135 * Math.Pow(sensorData[3], 2) + -2886.0595681876875 * Math.Pow(sensorData[4], 2) + -4.915737365751738 * Math.Pow(sensorData[5], 2) + -46725.73641858064 * Math.Pow(sensorData[6], 2) + 2.206375971671605 * Math.Pow(sensorData[7], 2) + 11213.817650939804 * Math.Pow(sensorData[8], 2) + -1.442231122941223 * Math.Pow(sensorData[9], 2) + -190.19794861762784 * Math.Pow(sensorData[10], 2) + -1.5154888976825305 * Math.Pow(sensorData[11], 2) + -3.863618497880452 * Math.Pow(sensorData[12], 2) + 0.9103829296975618 * Math.Pow(sensorData[13], 2) + 1.0369030260299041 * Math.Pow(sensorData[14], 2) + -0.21854947870730257 * Math.Pow(sensorData[0], 3) + -11046.984443278669 * Math.Pow(sensorData[1], 3) + -55686.720570639314 * Math.Pow(sensorData[2], 3) + 5733.5141921637405 * Math.Pow(sensorData[3], 3) + 1817.6633369975607 * Math.Pow(sensorData[4], 3) + 0.9314182211769833 * Math.Pow(sensorData[5], 3) + 40618.63441270345 * Math.Pow(sensorData[6], 3) + -0.31226387923983623 * Math.Pow(sensorData[7], 3) + -7799.971437506203 * Math.Pow(sensorData[8], 3) + 0.27652647189133006 * Math.Pow(sensorData[9], 3) + 79.72058385167293 * Math.Pow(sensorData[10], 3) + 0.2636982740711318 * Math.Pow(sensorData[11], 3) + 0.6559918899458452 * Math.Pow(sensorData[12], 3) + -0.09865221474104402 * Math.Pow(sensorData[13], 3) + -0.11631561442288785 * Math.Pow(sensorData[14], 3) + 0.019070496034523376 * Math.Pow(sensorData[0], 4) + 8513.527668454273 * Math.Pow(sensorData[1], 4) + 38697.18188967261 * Math.Pow(sensorData[2], 4) + -4007.1615418850834 * Math.Pow(sensorData[3], 4) + -1066.3319691191741 * Math.Pow(sensorData[4], 4) + -0.10741000301872178 * Math.Pow(sensorData[5], 4) + -23792.282689229367 * Math.Pow(sensorData[6], 4) + 0.02771789416515652 * Math.Pow(sensorData[7], 4) + 3672.4847091026095 * Math.Pow(sensorData[8], 4) + -0.032693744590073764 * Math.Pow(sensorData[9], 4) + -21.016992637955695 * Math.Pow(sensorData[10], 4) + -0.028451238885639663 * Math.Pow(sensorData[11], 4) + -0.073195277154392 * Math.Pow(sensorData[12], 4) + 0.0060099981192320295 * Math.Pow(sensorData[13], 4) + 0.007754548207326906 * Math.Pow(sensorData[14], 4) + -0.00118928137670172 * Math.Pow(sensorData[0], 5) + -4675.467298519374 * Math.Pow(sensorData[1], 5) + -19482.311275871678 * Math.Pow(sensorData[2], 5) + 1964.2588647709722 * Math.Pow(sensorData[3], 5) + 628.8641428730834 * Math.Pow(sensorData[4], 5) + 0.008235776436968816 * Math.Pow(sensorData[5], 5) + 9989.29825813423 * Math.Pow(sensorData[6], 5) + -0.001645433209023689 * Math.Pow(sensorData[7], 5) + -1246.010118825633 * Math.Pow(sensorData[8], 5) + 0.0025598731236406254 * Math.Pow(sensorData[9], 5) + 3.6558434543158853 * Math.Pow(sensorData[10], 5) + 0.0020582478885835757 * Math.Pow(sensorData[11], 5) + 0.005766564933402857 * Math.Pow(sensorData[12], 5) + -1.8439715799306242E-4 * Math.Pow(sensorData[13], 5) + -2.634498953454911E-4 * Math.Pow(sensorData[14], 5) + 5.4608581321400295E-5 * Math.Pow(sensorData[0], 6) + 1901.520400143558 * Math.Pow(sensorData[1], 6) + 7350.188849311969 * Math.Pow(sensorData[2], 6) + -703.2972458267882 * Math.Pow(sensorData[3], 6) + -314.635086274247 * Math.Pow(sensorData[4], 6) + -4.433682276771131E-4 * Math.Pow(sensorData[5], 6) + -3119.890059222891 * Math.Pow(sensorData[6], 6) + 6.791717925055555E-5 * Math.Pow(sensorData[7], 6) + 316.4499947390244 * Math.Pow(sensorData[8], 6) + -1.3914141275143087E-4 * Math.Pow(sensorData[9], 6) + -0.4227918856830222 * Math.Pow(sensorData[10], 6) + -1.0473216069917997E-4 * Math.Pow(sensorData[11], 6) + -3.347919646961013E-4 * Math.Pow(sensorData[12], 6) + -4.60087684692412E-7 * Math.Pow(sensorData[13], 6) + -1.1464432810451633E-6 * Math.Pow(sensorData[14], 6) + -1.8880724325088816E-6 * Math.Pow(sensorData[0], 7) + -586.7177520109232 * Math.Pow(sensorData[1], 7) + -2121.8545971918256 * Math.Pow(sensorData[2], 7) + 188.6710211030656 * Math.Pow(sensorData[3], 7) + 119.51856556368543 * Math.Pow(sensorData[4], 7) + 1.7355820192056137E-5 * Math.Pow(sensorData[5], 7) + 741.8604051259305 * Math.Pow(sensorData[6], 7) + -1.994686357691409E-6 * Math.Pow(sensorData[7], 7) + -61.643093254962466 * Math.Pow(sensorData[8], 7) + 5.427102657395672E-6 * Math.Pow(sensorData[9], 7) + 0.030573019790862355 * Math.Pow(sensorData[10], 7) + 3.868182886108693E-6 * Math.Pow(sensorData[11], 7) + 1.469236189015257E-5 * Math.Pow(sensorData[12], 7) + 3.081591427633682E-7 * Math.Pow(sensorData[13], 7) + 5.896007056064906E-7 * Math.Pow(sensorData[14], 7) + 4.9963781201267E-8 * Math.Pow(sensorData[0], 8) + 139.404866995153 * Math.Pow(sensorData[1], 8) + 474.6250512121004 * Math.Pow(sensorData[2], 8) + -38.517884414725714 * Math.Pow(sensorData[3], 8) + -33.701728482987946 * Math.Pow(sensorData[4], 8) + -5.053347297787415E-7 * Math.Pow(sensorData[5], 8) + -136.20289183152912 * Math.Pow(sensorData[6], 8) + 4.2076685668191756E-8 * Math.Pow(sensorData[7], 8) + 9.353158508931298 * Math.Pow(sensorData[8], 8) + -1.554745270638717E-7 * Math.Pow(sensorData[9], 8) + -9.347176019865663E-4 * Math.Pow(sensorData[10], 8) + -1.0590357368373126E-7 * Math.Pow(sensorData[11], 8) + -4.942736549296428E-7 * Math.Pow(sensorData[12], 8) + -1.507971414772438E-8 * Math.Pow(sensorData[13], 8) + -3.1868713364293806E-8 * Math.Pow(sensorData[14], 8) + -1.0223107229510444E-9 * Math.Pow(sensorData[0], 9) + -25.71105666767929 * Math.Pow(sensorData[1], 9) + -82.7858918016125 * Math.Pow(sensorData[2], 9) + 6.033783129437476 * Math.Pow(sensorData[3], 9) + 7.096103253843992 * Math.Pow(sensorData[4], 9) + 1.1094500899618583E-8 * Math.Pow(sensorData[5], 9) + 19.446457883056386 * Math.Pow(sensorData[6], 9) + -6.331401739488514E-10 * Math.Pow(sensorData[7], 9) + -1.115081539242611 * Math.Pow(sensorData[8], 9) + 3.321942456701728E-9 * Math.Pow(sensorData[9], 9) + -6.352376145977827E-5 * Math.Pow(sensorData[10], 9) + 2.1780099498907472E-9 * Math.Pow(sensorData[11], 9) + 1.2821425596299405E-8 * Math.Pow(sensorData[12], 9) + 4.2883254442365717E-10 * Math.Pow(sensorData[13], 9) + 1.0007072881643999E-9 * Math.Pow(sensorData[14], 9) + 1.62341487256794E-11 * Math.Pow(sensorData[0], 10) + 3.6885169649116807 * Math.Pow(sensorData[1], 10) + 11.269770753328872 * Math.Pow(sensorData[2], 10) + -0.726667455300013 * Math.Pow(sensorData[3], 10) + -1.1247403137940364 * Math.Pow(sensorData[4], 10) + -1.848361567440788E-10 * Math.Pow(sensorData[5], 10) + -2.161719391849527 * Math.Pow(sensorData[6], 10) + 6.561043558626327E-12 * Math.Pow(sensorData[7], 10) + 0.1047482299003261 * Math.Pow(sensorData[8], 10) + -5.337911529734028E-11 * Math.Pow(sensorData[9], 10) + 1.0219707960380144E-5 * Math.Pow(sensorData[10], 10) + -3.3871642808860145E-11 * Math.Pow(sensorData[11], 10) + -2.562587220114222E-10 * Math.Pow(sensorData[12], 10) + -8.340461557048477E-12 * Math.Pow(sensorData[13], 10) + -2.1353026621297385E-11 * Math.Pow(sensorData[14], 10) + -1.9947522359226547E-13 * Math.Pow(sensorData[0], 11) + -0.41001722169550975 * Math.Pow(sensorData[1], 11) + -1.1917696191356195 * Math.Pow(sensorData[2], 11) + 0.06699354231295186 * Math.Pow(sensorData[3], 11) + 0.13456948140932515 * Math.Pow(sensorData[4], 11) + 2.335530542820239E-12 * Math.Pow(sensorData[5], 11) + 0.18620031186559335 * Math.Pow(sensorData[6], 11) + -4.170358480280377E-14 * Math.Pow(sensorData[7], 11) + -0.007728174033330754 * Math.Pow(sensorData[8], 11) + 6.459425482453352E-13 * Math.Pow(sensorData[9], 11) + -6.92635287165821E-7 * Math.Pow(sensorData[10], 11) + 3.9836420373073394E-13 * Math.Pow(sensorData[11], 11) + 3.920039214809387E-12 * Math.Pow(sensorData[12], 11) + 1.16576717878857E-13 * Math.Pow(sensorData[13], 11) + 3.250177500399164E-13 * Math.Pow(sensorData[14], 11) + 1.8765398639255087E-15 * Math.Pow(sensorData[0], 12) + 0.034941141005239866 * Math.Pow(sensorData[1], 12) + 0.0968062557375716 * Math.Pow(sensorData[2], 12) + -0.004673905368718067 * Math.Pow(sensorData[3], 12) + -0.012091504362754869 * Math.Pow(sensorData[4], 12) + -2.2203418010097876E-14 * Math.Pow(sensorData[5], 12) + -0.012282781105427134 * Math.Pow(sensorData[6], 12) + 7.87255305251017E-17 * Math.Pow(sensorData[7], 12) + 4.4322242364779155E-4 * Math.Pow(sensorData[8], 12) + -5.8495023042486325E-15 * Math.Pow(sensorData[9], 12) + 2.9736675239159987E-8 * Math.Pow(sensorData[10], 12) + -3.5174744651244317E-15 * Math.Pow(sensorData[11], 12) + -4.5298640023889515E-14 * Math.Pow(sensorData[12], 12) + -1.1879092438838637E-15 * Math.Pow(sensorData[13], 12) + -3.58048997006789E-15 * Math.Pow(sensorData[14], 12) + -1.3245571283516505E-17 * Math.Pow(sensorData[0], 13) + -0.002238142534935474 * Math.Pow(sensorData[1], 13) + -0.005919870962371466 * Math.Pow(sensorData[2], 13) + 2.416084583770774E-4 * Math.Pow(sensorData[3], 13) + 8.035030983352527E-4 * Math.Pow(sensorData[4], 13) + 1.5605109297074973E-16 * Math.Pow(sensorData[5], 13) + 6.07662968017741E-4 * Math.Pow(sensorData[6], 13) + 1.2425558559890785E-18 * Math.Pow(sensorData[7], 13) + -1.937372408535658E-5 * Math.Pow(sensorData[8], 13) + 3.900854916110785E-17 * Math.Pow(sensorData[9], 13) + -8.671019561423794E-10 * Math.Pow(sensorData[10], 13) + 2.2928939823625817E-17 * Math.Pow(sensorData[11], 13) + 3.8701843507438407E-16 * Math.Pow(sensorData[12], 13) + 8.774548608331083E-18 * Math.Pow(sensorData[13], 13) + 2.8400277113897117E-17 * Math.Pow(sensorData[14], 13) + 6.778619025708313E-20 * Math.Pow(sensorData[0], 14) + 1.0420131262500713E-4 * Math.Pow(sensorData[1], 14) + 2.634705933115925E-4 * Math.Pow(sensorData[2], 14) + -8.929430319320305E-6 * Math.Pow(sensorData[3], 14) + -3.831526073456397E-5 * Math.Pow(sensorData[4], 14) + -7.854975175843517E-19 * Math.Pow(sensorData[5], 14) + -2.1771387135721808E-5 * Math.Pow(sensorData[6], 14) + -1.3455533917564893E-20 * Math.Pow(sensorData[7], 14) + 6.238676327122942E-7 * Math.Pow(sensorData[8], 14) + -1.8580751488939775E-19 * Math.Pow(sensorData[9], 14) + 1.7179747110217505E-11 * Math.Pow(sensorData[10], 14) + -1.0696539597173657E-19 * Math.Pow(sensorData[11], 14) + -2.3615996386735967E-18 * Math.Pow(sensorData[12], 14) + -4.583187499703935E-20 * Math.Pow(sensorData[13], 14) + -1.5833747297798718E-19 * Math.Pow(sensorData[14], 14) + -2.371110314002034E-22 * Math.Pow(sensorData[0], 15) + -3.326577837347484E-6 * Math.Pow(sensorData[1], 15) + -8.050184585961576E-6 * Math.Pow(sensorData[2], 15) + 2.2186396845726298E-7 * Math.Pow(sensorData[3], 15) + 1.2402273512288986E-6 * Math.Pow(sensorData[4], 15) + 2.6757642111325237E-21 * Math.Pow(sensorData[5], 15) + 5.320055939021861E-7 * Math.Pow(sensorData[6], 15) + 6.517303349602566E-23 * Math.Pow(sensorData[7], 15) + -1.39512962397703E-8 * Math.Pow(sensorData[8], 15) + 5.97932639932877E-22 * Math.Pow(sensorData[9], 15) + -2.2169812878940547E-13 * Math.Pow(sensorData[10], 15) + 3.376312556008826E-22 * Math.Pow(sensorData[11], 15) + 9.706485793913398E-21 * Math.Pow(sensorData[12], 15) + 1.6065254087699172E-22 * Math.Pow(sensorData[13], 15) + 5.892861591349515E-22 * Math.Pow(sensorData[14], 15) + 5.063678710478323E-25 * Math.Pow(sensorData[0], 16) + 6.508858936527683E-8 * Math.Pow(sensorData[1], 16) + 1.5091976802594492E-7 * Math.Pow(sensorData[2], 16) + -3.295190175352008E-9 * Math.Pow(sensorData[3], 16) + -2.4408560608392494E-8 * Math.Pow(sensorData[4], 16) + -5.5210817442562435E-24 * Math.Pow(sensorData[5], 16) + -7.907240525697884E-9 * Math.Pow(sensorData[6], 16) + -1.6555289082368655E-25 * Math.Pow(sensorData[7], 16) + 1.9343414179453467E-10 * Math.Pow(sensorData[8], 16) + -1.1644388963002598E-24 * Math.Pow(sensorData[9], 16) + 1.6787089989345808E-15 * Math.Pow(sensorData[10], 16) + -6.4568545493043955E-25 * Math.Pow(sensorData[11], 16) + -2.402288798455583E-23 * Math.Pow(sensorData[12], 16) + -3.3920212663504243E-25 * Math.Pow(sensorData[13], 16) + -1.3150494932453379E-24 * Math.Pow(sensorData[14], 16) + -4.973422712436245E-28 * Math.Pow(sensorData[0], 17) + -5.882684051607388E-10 * Math.Pow(sensorData[1], 17) + -1.3083491401207564E-9 * Math.Pow(sensorData[2], 17) + 2.1859745873578408E-11 * Math.Pow(sensorData[3], 17) + 2.2050292494978232E-10 * Math.Pow(sensorData[4], 17) + 5.207034302322086E-27 * Math.Pow(sensorData[5], 17) + 5.371379954646832E-11 * Math.Pow(sensorData[6], 17) + 1.7863400016969212E-28 * Math.Pow(sensorData[7], 17) + -1.2515406094503483E-12 * Math.Pow(sensorData[8], 17) + 1.0361930572519775E-27 * Math.Pow(sensorData[9], 17) + -5.63937896629102E-18 * Math.Pow(sensorData[10], 17) + 5.647275190793749E-28 * Math.Pow(sensorData[11], 17) + 2.699779172341467E-26 * Math.Pow(sensorData[12], 17) + 3.261501140167673E-28 * Math.Pow(sensorData[13], 17) + 1.331268230247722E-27 * Math.Pow(sensorData[14], 17) + 1832.961373076425;

        return pred;
    }
}
