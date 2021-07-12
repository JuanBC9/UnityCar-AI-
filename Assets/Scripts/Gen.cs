using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gen
{
    public string id;
    public double[] weights;
    public double fitness;

    public Gen(double[] solucion, string nId) {

        id = nId;
        weights = (double[]) solucion.Clone();
        fitness = Mathf.NegativeInfinity;
    }

    public Gen(double[] solucion, string nId, double newFitness) {

        id = nId;
        weights = (double[]) solucion.Clone();
        fitness = newFitness;
    }

    public double CalculateInput(double[] data)
    {
        double pred = 0;
        int dataIndex = 0;
        int currentDegree = 1;

        for (int i = 0; i < (weights.Length/2) - 1; i++)
        {
            pred += weights[i] * Math.Pow(data[dataIndex], currentDegree);

            dataIndex++;
            if (dataIndex == 15)
            {
                dataIndex = 0;
                currentDegree++;
            }
        }

        pred += weights[(weights.Length/2)-1];

        

        return pred;
    }

    public double CalculateAcc(double[] data)
    {
        double pred = 0;
        int dataIndex = 0;
        int currentDegree = 1;

        for (int i = (weights.Length / 2); i < weights.Length - 1; i++)
        {
            pred += weights[i] * Math.Pow(data[dataIndex], currentDegree);

            dataIndex++;
            if (dataIndex == 15)
            {
                dataIndex = 0;
                currentDegree++;
            }
        }

        pred += weights[weights.Length - 1];

        return pred;
    }

    public double getFitness()
    {
        return fitness;
    }

    public void addFitness(int value)
    {
        if (fitness == Mathf.NegativeInfinity)
        {
            fitness = value;
        } else
        {
            fitness += value;
        }
    }
}
