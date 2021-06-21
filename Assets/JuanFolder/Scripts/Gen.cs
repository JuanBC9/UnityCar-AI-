using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gen : MonoBehaviour
{
    public int weightsSize = 16;
    public double[] weights;
    private double fitness;
    public int CPs;
    public double time;

    private void Start()
    {
        CPs = 0;
        fitness = 0;
    }

    public double CalculateInput(double[] data)
    {
        double pred = 0;
        int dataIndex = 0;
        int currentDegree = 1;

        for (int i = 0; i < weightsSize - 1; i++)
        {
            pred += weights[i] * Math.Pow(data[dataIndex], currentDegree);

            dataIndex++;
            if (dataIndex == 15)
            {
                dataIndex = 0;
                currentDegree++;
            }
        }

        pred += weights[weights.Length-1];

        return pred;
    }

    internal double getFitness()
    {
        if (CPs > 0)
        {
            fitness = CPs / time;
        }
        return fitness;
    }

    public void setWeights(double[] newWeights)
    {
        weights = new double[newWeights.Length];

        weights = (double[]) newWeights.Clone();
    }

    public void addCP()
    {
        CPs++;
    }
}
