using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gen
{
    public double[] weights;
    public double fitness;

    public Gen(double[] solucion) {

        weights = (double[]) solucion.Clone();
        fitness = Mathf.NegativeInfinity;
    }

    public Gen(double[] solucion, double newFitness) {

        weights = (double[]) solucion.Clone();
        fitness = newFitness;
    }

    public double CalculateInput(double[] data)
    {
        double pred = 0;
        int dataIndex = 0;
        int currentDegree = 1;

        for (int i = 0; i < weights.Length - 1; i++)
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

    public double getFitness()
    {
        return fitness;
    }

    public void addFitness(int value)
    {
        fitness+=value;
    }
}
