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

        double pred = -0.31472669943468645 * Math.Pow(sensorData[0], 1) + -0.3849104675464332 * Math.Pow(sensorData[1], 1) + 0.22160835191607475 * Math.Pow(sensorData[2], 1) + -0.07624779362231493 * Math.Pow(sensorData[3], 1) + -0.2945975372567773 * Math.Pow(sensorData[4], 1) + 0.15013785939663649 * Math.Pow(sensorData[5], 1) + 0.04822506755590439 * Math.Pow(sensorData[6], 1) + -0.0701806154102087 * Math.Pow(sensorData[7], 1) + -0.004660400794818997 * Math.Pow(sensorData[8], 1) + 0.15918369035352953 * Math.Pow(sensorData[9], 1) + 0.1751081550028175 * Math.Pow(sensorData[10], 1) + -3.528923843987286E-4 * Math.Pow(sensorData[11], 1) + 0.025514028500765562 * Math.Pow(sensorData[12], 1) + 0.07857317815069109 * Math.Pow(sensorData[13], 1) + 0.2003734279423952 * Math.Pow(sensorData[14], 1) + 0.10617524954068358 * Math.Pow(sensorData[0], 2) + 0.08779136565635781 * Math.Pow(sensorData[1], 2) + -0.07605080993744195 * Math.Pow(sensorData[2], 2) + 0.08682017644969164 * Math.Pow(sensorData[3], 2) + 0.11118202366924379 * Math.Pow(sensorData[4], 2) + -0.018418382765958086 * Math.Pow(sensorData[5], 2) + -0.07427803577411396 * Math.Pow(sensorData[6], 2) + 0.0013891147827962413 * Math.Pow(sensorData[7], 2) + -0.017343722081932356 * Math.Pow(sensorData[8], 2) + -0.028505482864602527 * Math.Pow(sensorData[9], 2) + -0.03226271913626988 * Math.Pow(sensorData[10], 2) + -0.01553095397503057 * Math.Pow(sensorData[11], 2) + -0.030300620339403395 * Math.Pow(sensorData[12], 2) + -0.0028082167160476956 * Math.Pow(sensorData[13], 2) + -0.04746456720022252 * Math.Pow(sensorData[14], 2) + -0.017339316479109357 * Math.Pow(sensorData[0], 3) + -0.005480960384518951 * Math.Pow(sensorData[1], 3) + 0.0074072253718249215 * Math.Pow(sensorData[2], 3) + -0.02625693947179286 * Math.Pow(sensorData[3], 3) + -0.01538601205862733 * Math.Pow(sensorData[4], 3) + -0.003956498620077298 * Math.Pow(sensorData[5], 3) + 0.024415140212099118 * Math.Pow(sensorData[6], 3) + 0.0027300811618005127 * Math.Pow(sensorData[7], 3) + 0.007633755799162145 * Math.Pow(sensorData[8], 3) + 8.187878372609703E-4 * Math.Pow(sensorData[9], 3) + 0.0038132282647325155 * Math.Pow(sensorData[10], 3) + 0.0033186436162253585 * Math.Pow(sensorData[11], 3) + 0.008827254005069562 * Math.Pow(sensorData[12], 3) + -0.0017575661383801844 * Math.Pow(sensorData[13], 3) + 0.00635122907249297 * Math.Pow(sensorData[14], 3) + 0.0016821315564599926 * Math.Pow(sensorData[0], 4) + -5.057503419290121E-4 * Math.Pow(sensorData[1], 4) + -5.584242848521992E-5 * Math.Pow(sensorData[2], 4) + 0.003748606905327828 * Math.Pow(sensorData[3], 4) + 8.146678905658344E-4 * Math.Pow(sensorData[4], 4) + 0.0010788073464057524 * Math.Pow(sensorData[5], 4) + -0.003843473943017983 * Math.Pow(sensorData[6], 4) + -6.031708268118413E-4 * Math.Pow(sensorData[7], 4) + -0.001335941582179867 * Math.Pow(sensorData[8], 4) + 2.4277289752427933E-4 * Math.Pow(sensorData[9], 4) + -4.062197302545645E-4 * Math.Pow(sensorData[10], 4) + -3.333798670656085E-4 * Math.Pow(sensorData[11], 4) + -0.001234981566975435 * Math.Pow(sensorData[12], 4) + 3.201815338794711E-4 * Math.Pow(sensorData[13], 4) + -5.44319204568211E-4 * Math.Pow(sensorData[14], 4) + -1.0738345341379085E-4 * Math.Pow(sensorData[0], 5) + 1.0937646598083856E-4 * Math.Pow(sensorData[1], 5) + -4.142207680510757E-5 * Math.Pow(sensorData[2], 5) + -3.114800566827924E-4 * Math.Pow(sensorData[3], 5) + 1.002187533645114E-5 * Math.Pow(sensorData[4], 5) + -1.0914881501576345E-4 * Math.Pow(sensorData[5], 5) + 3.5382500007036504E-4 * Math.Pow(sensorData[6], 5) + 6.354981057750549E-5 * Math.Pow(sensorData[7], 5) + 1.2694527520223237E-4 * Math.Pow(sensorData[8], 5) + -3.225031587523379E-5 * Math.Pow(sensorData[9], 5) + 3.568347473889479E-5 * Math.Pow(sensorData[10], 5) + 1.980282527522565E-5 * Math.Pow(sensorData[11], 5) + 1.015019470934353E-4 * Math.Pow(sensorData[12], 5) + -2.7514338271729605E-5 * Math.Pow(sensorData[13], 5) + 3.146760311596025E-5 * Math.Pow(sensorData[14], 5) + 4.77895600328771E-6 * Math.Pow(sensorData[0], 6) + -8.689428122454156E-6 * Math.Pow(sensorData[1], 6) + 3.801790012804313E-6 * Math.Pow(sensorData[2], 6) + 1.670154061110483E-5 * Math.Pow(sensorData[3], 6) + -3.6766305119595E-6 * Math.Pow(sensorData[4], 6) + 6.352895872433279E-6 * Math.Pow(sensorData[5], 6) + -2.089032841307247E-5 * Math.Pow(sensorData[6], 6) + -4.0141108293594075E-6 * Math.Pow(sensorData[7], 6) + -7.45997234133524E-6 * Math.Pow(sensorData[8], 6) + 2.0042798753472644E-6 * Math.Pow(sensorData[9], 6) + -2.2088688484341E-6 * Math.Pow(sensorData[10], 6) + -7.561305180354638E-7 * Math.Pow(sensorData[11], 6) + -5.420932871724651E-6 * Math.Pow(sensorData[12], 6) + 1.4576909498101624E-6 * Math.Pow(sensorData[13], 6) + -1.2723755847637809E-6 * Math.Pow(sensorData[14], 6) + -1.5341324021518872E-7 * Math.Pow(sensorData[0], 7) + 4.1007722525802214E-7 * Math.Pow(sensorData[1], 7) + -1.8159091530015754E-7 * Math.Pow(sensorData[2], 7) + -6.129954194947351E-7 * Math.Pow(sensorData[3], 7) + 2.2678168775972077E-7 * Math.Pow(sensorData[4], 7) + -2.4128782505899945E-7 * Math.Pow(sensorData[5], 7) + 8.358012635130104E-7 * Math.Pow(sensorData[6], 7) + 1.6648296707844614E-7 * Math.Pow(sensorData[7], 7) + 2.9143952116722916E-7 * Math.Pow(sensorData[8], 7) + -7.718985117065432E-8 * Math.Pow(sensorData[9], 7) + 9.300134503162242E-8 * Math.Pow(sensorData[10], 7) + 1.9180270321286297E-8 * Math.Pow(sensorData[11], 7) + 1.9904329901132792E-7 * Math.Pow(sensorData[12], 7) + -5.2212117078741485E-8 * Math.Pow(sensorData[13], 7) + 3.708688343310043E-8 * Math.Pow(sensorData[14], 7) + 3.6239402862800783E-9 * Math.Pow(sensorData[0], 8) + -1.288905573095286E-8 * Math.Pow(sensorData[1], 8) + 5.608708307207091E-9 * Math.Pow(sensorData[2], 8) + 1.5955450495978148E-8 * Math.Pow(sensorData[3], 8) + -7.877736268908582E-9 * Math.Pow(sensorData[4], 8) + 6.341067674632864E-9 * Math.Pow(sensorData[5], 8) + -2.3475091450520955E-8 * Math.Pow(sensorData[6], 8) + -4.763297436057691E-9 * Math.Pow(sensorData[7], 8) + -7.90695656827604E-9 * Math.Pow(sensorData[8], 8) + 2.013466754371916E-9 * Math.Pow(sensorData[9], 8) + -2.7055106411600257E-9 * Math.Pow(sensorData[10], 8) + -3.2143365029582535E-10 * Math.Pow(sensorData[11], 8) + -5.192670840101355E-9 * Math.Pow(sensorData[12], 8) + 1.3231448138287343E-9 * Math.Pow(sensorData[13], 8) + -7.979332936234352E-10 * Math.Pow(sensorData[14], 8) + -6.36340065151079E-11 * Math.Pow(sensorData[0], 9) + 2.830152463278553E-10 * Math.Pow(sensorData[1], 9) + -1.202030496277734E-10 * Math.Pow(sensorData[2], 9) + -3.0057176150096095E-10 * Math.Pow(sensorData[3], 9) + 1.7999785364362644E-10 * Math.Pow(sensorData[4], 9) + -1.1889686199328684E-10 * Math.Pow(sensorData[5], 9) + 4.730488637870014E-10 * Math.Pow(sensorData[6], 9) + 9.670711333668373E-11 * Math.Pow(sensorData[7], 9) + 1.5289257503407684E-10 * Math.Pow(sensorData[8], 9) + -3.704588462139729E-11 * Math.Pow(sensorData[9], 9) + 5.553036417288251E-11 * Math.Pow(sensorData[10], 9) + 3.299761474866492E-12 * Math.Pow(sensorData[11], 9) + 9.804614355225198E-11 * Math.Pow(sensorData[12], 9) + -2.4301581215878492E-11 * Math.Pow(sensorData[13], 9) + 1.2876335799220405E-11 * Math.Pow(sensorData[14], 9) + 8.323018029387976E-13 * Math.Pow(sensorData[0], 10) + -4.435815602126986E-12 * Math.Pow(sensorData[1], 10) + 1.839267181335167E-12 * Math.Pow(sensorData[2], 10) + 4.1347879374194674E-12 * Math.Pow(sensorData[3], 10) + -2.8544377851467116E-12 * Math.Pow(sensorData[4], 10) + 1.613682929637343E-12 * Math.Pow(sensorData[5], 10) + -6.9127769174068986E-12 * Math.Pow(sensorData[6], 10) + -1.4129058464239125E-12 * Math.Pow(sensorData[7], 10) + -2.1345754204827188E-12 * Math.Pow(sensorData[8], 10) + 4.899296080106338E-13 * Math.Pow(sensorData[9], 10) + -8.151815903377818E-13 * Math.Pow(sensorData[10], 10) + -1.3392106612759023E-14 * Math.Pow(sensorData[11], 10) + -1.3504290604484321E-12 * Math.Pow(sensorData[12], 10) + 3.269275884640717E-13 * Math.Pow(sensorData[13], 10) + -1.5697610396124312E-13 * Math.Pow(sensorData[14], 10) + -8.059464876200075E-15 * Math.Pow(sensorData[0], 11) + 4.989772643612779E-14 * Math.Pow(sensorData[1], 11) + -2.0247589175270142E-14 * Math.Pow(sensorData[2], 11) + -4.1475993656240884E-14 * Math.Pow(sensorData[3], 11) + 3.201601152071377E-14 * Math.Pow(sensorData[4], 11) + -1.588342613764124E-14 * Math.Pow(sensorData[5], 11) + 7.327676876407889E-14 * Math.Pow(sensorData[6], 11) + 1.4887244662301215E-14 * Math.Pow(sensorData[7], 11) + 2.1551985433325185E-14 * Math.Pow(sensorData[8], 11) + -4.677872581751945E-15 * Math.Pow(sensorData[9], 11) + 8.587918943361761E-15 * Math.Pow(sensorData[10], 11) + -1.4293900539454966E-16 * Math.Pow(sensorData[11], 11) + 1.3542704109308626E-14 * Math.Pow(sensorData[12], 11) + -3.219651279068945E-15 * Math.Pow(sensorData[13], 11) + 1.4416480451842296E-15 * Math.Pow(sensorData[14], 11) + 5.685575988135891E-17 * Math.Pow(sensorData[0], 12) + -3.993710210938393E-16 * Math.Pow(sensorData[1], 12) + 1.5905299690146177E-16 * Math.Pow(sensorData[2], 12) + 2.9947661011416327E-16 * Math.Pow(sensorData[3], 12) + -2.5348075053941785E-16 * Math.Pow(sensorData[4], 12) + 1.1218949074464404E-16 * Math.Pow(sensorData[5], 12) + -5.570372494272311E-16 * Math.Pow(sensorData[6], 12) + -1.1197804785191297E-16 * Math.Pow(sensorData[7], 12) + -1.5572147559948326E-16 * Math.Pow(sensorData[8], 12) + 3.1959630195264914E-17 * Math.Pow(sensorData[9], 12) + -6.435688321722527E-17 * Math.Pow(sensorData[10], 12) + 2.7987918230846563E-18 * Math.Pow(sensorData[11], 12) + -9.762173502522204E-17 * Math.Pow(sensorData[12], 12) + 2.292554317812565E-17 * Math.Pow(sensorData[13], 12) + -9.824809283734203E-18 * Math.Pow(sensorData[14], 12) + -2.833741540945761E-19 * Math.Pow(sensorData[0], 13) + 2.2168262798256544E-18 * Math.Pow(sensorData[1], 13) + -8.688459691009543E-19 * Math.Pow(sensorData[2], 13) + -1.5127656954824718E-18 * Math.Pow(sensorData[3], 13) + 1.385840383362796E-18 * Math.Pow(sensorData[4], 13) + -5.536529274902079E-19 * Math.Pow(sensorData[5], 13) + 2.955056050956118E-18 * Math.Pow(sensorData[6], 13) + 5.856312355566141E-19 * Math.Pow(sensorData[7], 13) + 7.84097108698365E-19 * Math.Pow(sensorData[8], 13) + -1.5230290683599228E-19 * Math.Pow(sensorData[9], 13) + 3.3453046069348253E-19 * Math.Pow(sensorData[10], 13) + -2.2173925765928733E-20 * Math.Pow(sensorData[11], 13) + 4.917930634802298E-19 * Math.Pow(sensorData[12], 13) + -1.1474984525705517E-19 * Math.Pow(sensorData[13], 13) + 4.8136572503718687E-20 * Math.Pow(sensorData[14], 13) + 9.434928047412634E-22 * Math.Pow(sensorData[0], 14) + -8.097675854119225E-21 * Math.Pow(sensorData[1], 14) + 3.1306249757849004E-21 * Math.Pow(sensorData[2], 14) + 5.0625569578317185E-21 * Math.Pow(sensorData[3], 14) + -4.975342307271196E-21 * Math.Pow(sensorData[4], 14) + 1.8103717487117866E-21 * Math.Pow(sensorData[5], 14) + -1.036992616200211E-20 * Math.Pow(sensorData[6], 14) + -2.0201564911905765E-21 * Math.Pow(sensorData[7], 14) + -2.609779019691772E-21 * Math.Pow(sensorData[8], 14) + 4.805491333360519E-22 * Math.Pow(sensorData[9], 14) + -1.1452556080420488E-21 * Math.Pow(sensorData[10], 14) + 9.888336981450908E-23 * Math.Pow(sensorData[11], 14) + -1.6405889963573483E-21 * Math.Pow(sensorData[12], 14) + 3.824166881651876E-22 * Math.Pow(sensorData[13], 14) + -1.5993054412252326E-22 * Math.Pow(sensorData[14], 14) + -1.8786714537791956E-24 * Math.Pow(sensorData[0], 15) + 1.7470299728329343E-23 * Math.Pow(sensorData[1], 15) + -6.675483632897976E-24 * Math.Pow(sensorData[2], 15) + -1.0060422871994704E-23 * Math.Pow(sensorData[3], 15) + 1.0541426258721041E-23 * Math.Pow(sensorData[4], 15) + -3.519670793622473E-24 * Math.Pow(sensorData[5], 15) + 2.159018740567243E-23 * Math.Pow(sensorData[6], 15) + 4.1254087339049944E-24 * Math.Pow(sensorData[7], 15) + 5.153325940647399E-24 * Math.Pow(sensorData[8], 15) + -9.015278889235414E-25 * Math.Pow(sensorData[9], 15) + 2.3192548047871857E-24 * Math.Pow(sensorData[10], 15) + -2.4224417630532835E-25 * Math.Pow(sensorData[11], 15) + 3.2502602591092714E-24 * Math.Pow(sensorData[12], 15) + -7.605753701873257E-25 * Math.Pow(sensorData[13], 15) + 3.2154128897939203E-25 * Math.Pow(sensorData[14], 15) + 1.6876925644805825E-27 * Math.Pow(sensorData[0], 16) + -1.6823839528504566E-26 * Math.Pow(sensorData[1], 16) + 6.364115390102203E-27 * Math.Pow(sensorData[2], 16) + 8.965491719390359E-27 * Math.Pow(sensorData[3], 16) + -9.970388160053505E-27 * Math.Pow(sensorData[4], 16) + 3.0752895319641512E-27 * Math.Pow(sensorData[5], 16) + -2.0146041238961498E-26 * Math.Pow(sensorData[6], 16) + -3.7702426572529095E-27 * Math.Pow(sensorData[7], 16) + -4.563909674757819E-27 * Math.Pow(sensorData[8], 16) + 7.6060957537543445E-28 * Math.Pow(sensorData[9], 16) + -2.1013632673775303E-27 * Math.Pow(sensorData[10], 16) + 2.5436780165403146E-28 * Math.Pow(sensorData[11], 16) + -2.8895719987861176E-27 * Math.Pow(sensorData[12], 16) + 6.816090770216914E-28 * Math.Pow(sensorData[13], 16) + -2.9409902011400948E-28 * Math.Pow(sensorData[14], 16) + 0.20540420548059046;

        return pred;
    }
}
