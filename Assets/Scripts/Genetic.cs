using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Genetic
{
    [Header("Genetic Settings")]
    private int populationSize = 10;
    private float mutationRate = .01f;
    private float parentSelectionRate = .5f;
    private int solutionSize = 16;

    public Genetic(int pSize, float mutRate, float bias, int solSize)
    {
        populationSize = pSize;
        mutationRate = mutRate;
        parentSelectionRate = bias;
        solutionSize = solSize;
    }

    public Genetic() { }

    public List<Gen> CreateRandomSolutions()
    {
        double[] random;
        List<Gen> solutions = new List<Gen>();

        Debug.Log("Creating Random Solutions...");
        //Create Random population of solutions
        for (int i = 0; i < populationSize; i++)
        {
            random = new double[solutionSize];
            //Create Random Solution
            for (int j = 0; j < solutionSize; j++)
            {
                random[j] = Random.Range(-0.02f, 0.02f);
            }

            //Asign Random Solution to Gen
            solutions.Add(new Gen(random, "S" + i));
        }

        return solutions;
    }

    public List<Gen> CreateChildSolutions(List<Gen> sols)
    {
        //Select Parents
        List<Gen> bestParents = getBestParents(sols);

        Debug.Log("Creating Child Solutions...");
        //Create child solutions
        List<Gen> childSols = new List<Gen>();
        double[] childSol;

        for (int i = 0; i < populationSize; i++)
        {

            //Create child solution
            childSol = new double[solutionSize];

            for (int j = 0; j < solutionSize; j++)
            {
                if (Random.Range(0f, 1f) < parentSelectionRate)
                {
                    //Mejor Padre
                    if (Random.Range(0f, 1f) < mutationRate)
                    {

                        childSol[j] = bestParents[0].weights[j] + Random.Range(-.01f, .01f);
                    }
                    else
                    {
                        childSol[j] = bestParents[0].weights[j];
                    }

                }
                else
                {

                    //Segundo Mejor Padre
                    if (Random.Range(0f, 1f) < mutationRate)
                    {

                        childSol[j] = bestParents[1].weights[j] + Random.Range(-.01f, .01f);
                    }
                    else
                    {
                        childSol[j] = bestParents[1].weights[j];
                    }
                }
            }

            //Asign Child solution to Gen
            childSols.Add(new Gen(childSol, "CS" + i));
        }

        return childSols;
    }

    public List<Gen> CombineSolutions(List<Gen> prevSols, List<Gen> childSols)
    {
        Debug.Log("Creating New Solutions...");
        //Swap some of the existing solutions with some of the better children
        List<Gen> newSolutionsList = new List<Gen>(populationSize);
        Gen bestSol;
        List<string> addedSolutions = new List<string>();

        for (int i = 0; i < populationSize; i++)
        {
            //Get Best Solution
            bestSol = new Gen(new double[solutionSize], "");

            for (int j = 0; j < populationSize; j++)
            {
                if (childSols[j].fitness >= bestSol.fitness && !addedSolutions.Contains(childSols[j].id))
                {
                    bestSol.weights = childSols[j].weights;
                    bestSol.fitness = childSols[j].fitness;
                    bestSol.id = childSols[j].id;
                }
            }

            for (int j = 0; j < populationSize; j++)
            {
                if (prevSols[j].fitness >= bestSol.fitness && !addedSolutions.Contains(prevSols[j].id))
                {
                    bestSol.weights = prevSols[j].weights;
                    bestSol.fitness = prevSols[j].fitness;
                    bestSol.id = prevSols[j].id;
                }
            }

            //Add Best Solution to list
            addedSolutions.Add(bestSol.id);
            newSolutionsList.Add(new Gen(bestSol.weights, "S" + i, bestSol.fitness));
        }

        return newSolutionsList;
    }

    private List<Gen> getBestParents(List<Gen> gens)
    {
        List<Gen> ret = new List<Gen>(2);
        ret.Add(null);
        ret.Add(null);

        double bestFitness = Mathf.NegativeInfinity;
        double bestFitness_2 = Mathf.NegativeInfinity;

        for (int i = 0; i < populationSize; i++)
        {
            if (gens[i].fitness >= bestFitness)
            {
                bestFitness = gens[i].fitness;
                ret[0] = gens[i];

            }
        }

        for (int i = 0; i < populationSize; i++)
        {
            if (gens[i].fitness >= bestFitness_2 && !ret.Contains(gens[i]))
            {
                bestFitness_2 = gens[i].fitness;
                ret[1] = gens[i];
            }
        }

        return ret;
    }
    
    public string SolutionToString(double[] sol)
    {
        string str = "";

        for (int i = 0; i < sol.Length; i++)
        {
            str += sol[i] + ", ";
        }

        return str;
    }
}
