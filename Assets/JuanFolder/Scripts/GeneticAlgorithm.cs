using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticAlgorithm : MonoBehaviour
{
    [Header("Genetic Settings")]
    public int populationSize = 10;
    public int generationTime = 20;
    public float targetFitness = 20;
    public float mutationRate = .01f;
    public float parentSelectionRate = .5f;
    public GameObject prefab;

    private List<GameObject> prefabList;
    private static int solutionSize = 16;

    private List<Gen> solutionsList;
    private List<Gen> childSolutionsList;
    private Gen bestSolution;

    //Inicializar Variables
    public void Start()
    {
        prefabList = new List<GameObject>(populationSize);
        for (int i = 0; i < populationSize; i++)
        {
            prefabList.Add(null);
        }
        
        solutionsList = new List<Gen>(populationSize);

        childSolutionsList = new List<Gen>(populationSize);
        for (int i = 0; i < populationSize; i++)
        {
            childSolutionsList.Add(new Gen(new double[solutionSize], "CS"+i));
        }

        bestSolution = new Gen(new double[solutionSize], "");

        StartCoroutine("GeneticAlg");
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

    private string SolutionToString(double[] sol)
    {
        string str = "";

        for (int i = 0; i < sol.Length; i++)
        {
            str += sol[i] + ", ";
        }

        return str;
    }

    IEnumerator GeneticAlg()
    {
        double[] random;

        Debug.Log("Creating Random Solutions...");
        //Create Random population of solutions
        for (int i = 0; i < populationSize; i++)
        {
            random = new double[solutionSize];
            //Create Random Solution
            for (int j = 0; j < solutionSize; j++)
            {
                random[j] = Random.Range(-0.01f, 0.01f);
            }

            //Asign Random Solution to Gen
            solutionsList.Add(new Gen(random, "S"+i));
        }

        Debug.Log("Testing Solutions...");
        //Test Solutions
        yield return StartCoroutine(runGeneration(solutionsList)); 

        do
        {
            //Select Parents
            List<Gen> bestParents = getBestParents(solutionsList);

            Debug.Log("Creating Child Solutions...");
            //Create child solutions
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

                    } else {
                        
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
                childSolutionsList[i] = new Gen(childSol, "CS"+i);
            }
            
            Debug.Log("Evaluating Child Solutions...");
            //Evaluate Children
            yield return StartCoroutine(runGeneration(childSolutionsList));


            Debug.Log("Solutions:");
            for (int i = 0; i < populationSize; i++)
            {
                Debug.Log(SolutionToString(solutionsList[i].weights) + "Fitness: " + solutionsList[i].fitness);
            }

            Debug.Log("Child Solutions:");
            for (int i = 0; i < populationSize; i++)
            {
                Debug.Log(SolutionToString(childSolutionsList[i].weights) + "Fitness: " + childSolutionsList[i].fitness);
            }

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
                    if (childSolutionsList[j].fitness >= bestSol.fitness && !addedSolutions.Contains(childSolutionsList[j].id))
                    {
                        bestSol.weights = childSolutionsList[j].weights;
                        bestSol.fitness = childSolutionsList[j].fitness;
                        bestSol.id = childSolutionsList[j].id;
                    }
                }

                for (int j = 0; j < populationSize; j++)
                {
                    if (solutionsList[j].fitness >= bestSol.fitness && !addedSolutions.Contains(solutionsList[j].id))
                    {
                        bestSol.weights = solutionsList[j].weights;
                        bestSol.fitness = solutionsList[j].fitness;
                        bestSol.id = solutionsList[j].id;
                    }
                }

                //Add Best Solution to list
                addedSolutions.Add(bestSol.id);
                newSolutionsList.Add(new Gen(bestSol.weights, "S"+i, bestSol.fitness));
                Debug.Log(SolutionToString(newSolutionsList[i].weights));
            }

            solutionsList = newSolutionsList;

            bestSolution = new Gen(solutionsList[0].weights, "", solutionsList[0].fitness);

            Debug.Log("\nBest Fitness = " + bestSolution.fitness);
            Debug.Log("Best Solution = " + SolutionToString(bestSolution.weights));
           
            //Repeat until the termintion criterion is met
        } while (targetFitness > bestSolution.fitness);
        
        Debug.Log("Done");
        Debug.Log(SolutionToString(bestSolution.weights));
    }

    IEnumerator runGeneration(List<Gen> generation) {

        for (int i = 0; i < populationSize; i++)
        {
            prefabList[i] = Instantiate(prefab, Vector3.up, Quaternion.identity);
            prefabList[i].GetComponent<BotController>().gen = generation[i];
        }

        yield return new WaitForSeconds(generationTime);

        for (int i = 0; i < populationSize; i++)
        {
            generation[i] = prefabList[i].GetComponent<BotController>().gen;
            Destroy(prefabList[i].GetComponent<BotController>().sphereRB.gameObject);
            Destroy(prefabList[i]);
        }
    }
}
