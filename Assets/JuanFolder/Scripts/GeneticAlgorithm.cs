using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticAlgorithm : MonoBehaviour
{
    public GameObject GenPrefab;
    public int populationSize = 10;
    public int generationTime = 20;
    public float targetFitness = 20;
    public float mutationRate = .5f;

    private static int solutionSize = 16;

    private double[] FirstPopulationSolution = {
    0.00011529200652074178,
    0.14399022224954028,
    -0.17760787035044975,
    -0.1261946864383615,
    0.10873645116524869,
    -0.10252068826841217,
    0.04422447736431941,
    0.01241326604397893,
    -0.012550330622690235,
    0.00950295599825631,
    -0.004470662265458513,
    0.007148653003135345,
    -0.004038385647423428,
    0.003776639132027863,
    -0.0016006402572690756,
    0.4832718374292429
    };
    private GameObject[] GenPrefabPopulation;

    private List<double[]> solutions;
    private List<double> fitneses;

    List<double[]> childSolutions;  
    List<double> childFitneses;

    private double[] bestSolution;
    private double bestFitness;

    public void Start()
    {
        GenPrefabPopulation = new GameObject[populationSize];

        solutions = new List<double[]>(populationSize);
        fitneses = new List<double>(populationSize);
        for (int i = 0; i < populationSize; i++)
        {
            solutions.Add(new double[solutionSize]);
            fitneses.Add(0);
        }

        childSolutions = new List<double[]>(populationSize);
        childFitneses = new List<double>(populationSize);
        for (int i = 0; i < populationSize; i++)
        {
            childSolutions.Add(new double[solutionSize]);
            childFitneses.Add(0);
        }
        
        
        bestSolution = new double[solutionSize];
        bestFitness = 0;

        Debug.Log("Start Genetic");
        StartCoroutine("StartGen");
    }

    IEnumerator StartGen()
    {
        double[] solution = new double[solutionSize];

        //Create Random population of solutions
        for (int i = 0; i < populationSize; i++)
        {
            for (int j = 0; j < solutionSize; j++)
            {
                //solution[j] = FirstPopulationSolution[j] + Random.Range(-0.01f, 0.01f);
                solution[j] = Random.Range(-0.01f, 0.01f);
            }

            solutions[i] = (double[])solution.Clone();

            GenPrefabPopulation[i] = Instantiate(GenPrefab, Vector3.up, Quaternion.identity);
            GenPrefabPopulation[i].SetActive(false);
            GenPrefabPopulation[i].GetComponent<BotController>().gen.setWeights(solution);
        }

        for (int i = 0; i < populationSize; i++)
        {
            GenPrefabPopulation[i].SetActive(true);
        }

        yield return new WaitForSeconds(generationTime);

        for (int i = 0; i < populationSize; i++)
        {
            //Get fitnesses
            fitneses[i] = GenPrefabPopulation[i].GetComponent<BotController>().gen.getFitness();
            Destroy(GenPrefabPopulation[i]);

            //Clean up Scene
            GameObject MotorSphere = FindObjectsOfType<Rigidbody>()[i].gameObject;
            if (MotorSphere.name == "MotorSphere")
            {
                Destroy(MotorSphere);
            }
        }

        StartCoroutine("GeneticAlg");
    }

    private int[] getBestParents()
    {
        int[] ret = new int[2];

        double bestFitness = -2;
        double bestFitness_2 = -1;

        int bestParentIndex = -1;
        int bestParentIndex_2 = -1;

        for (int i = 0; i < populationSize; i++)
        {
            if (fitneses[i] > bestFitness)
            {
                bestFitness = fitneses[i];
                bestParentIndex = i;
            }
        }

        for (int i = 0; i < populationSize; i++)
        {
            if (fitneses[i] > bestFitness_2 && i != bestParentIndex)
            {
                bestFitness_2 = fitneses[i];
                bestParentIndex_2 = i;
            }
        }

        ret[0] = bestParentIndex;
        ret[1] = bestParentIndex_2;

        return ret;
    }

    private void printSolution(double[] sol)
    {
        string str = "";

        for (int i = 0; i < sol.Length; i++)
        {
            str += sol[i] + ", ";
        }

        Debug.Log(str);
    }

    IEnumerator GeneticAlg()
    {
        do
        {
            Debug.Log("Generation_Start");

            //Select Parents
            int[] bestParentsIndex = getBestParents();

            double[] bestParent = solutions[bestParentsIndex[0]];
            double[] bestParent_2 = solutions[bestParentsIndex[1]];

            Debug.Log("Best Parent = " + bestParentsIndex[0]);
            Debug.Log("Best Parent_2 = " + bestParentsIndex[1]);

            //Create child solutions
            for (int i = 0; i < populationSize; i++)
            {
                for (int j = 0; j < solutionSize/2; j++)
                {
                    if (Random.Range(0, 1) < mutationRate)
                    {
                        childSolutions[i][j] = bestParent[j] + Random.Range(-.01f, .01f);
                    }
                    else
                    {
                        childSolutions[i][j] = bestParent[j];
                    }
                }

                for (int j = solutionSize/2; j < solutionSize; j++)
                {
                    if (Random.Range(0, 1) < mutationRate)
                    {
                        childSolutions[i][j] = bestParent_2[j] + Random.Range(-.01f, .01f);
                    }
                    else
                    {
                        childSolutions[i][j] = bestParent_2[j];
                    }
                }
            }

            //Evaluate Children
            Debug.Log("StartNewGeneration");
            yield return RunGeneration();

            //Swap some of the existing solutions with some of the better children
            List<double[]> newSolutions = new List<double[]>(populationSize);
            List<double> newFitneses = new List<double>(populationSize);
            for (int i = 0; i < populationSize; i++)
            {
                newSolutions.Add(new double[solutionSize]);
                newFitneses.Add(0);
            }
            
            for (int i = 0; i < populationSize; i++)
            {
                double[] bestSolution = null;
                double bestFit = -1;

                for (int j = 0; j < populationSize; j++)
                {
                    if (childFitneses[j] > bestFit && !newSolutions.Contains(childSolutions[i]))
                    {
                        bestSolution = childSolutions[i];
                        bestFit = childFitneses[j];
                    }

                    if (fitneses[j] > bestFit && !newSolutions.Contains(solutions[i]))
                    {
                        bestSolution = childSolutions[i];
                        bestFit = childFitneses[j];
                    }
                }

                newFitneses[i] = bestFit;
                newSolutions[i] = bestSolution;
            }

            solutions = newSolutions;
            fitneses = newFitneses;

            //////////
            printSolution(solutions[0]);

            double bestNewFitness = -1;
            double[] bestNewSolution = null;
            for (int i = 0; i < populationSize; i++)
            {
                if (fitneses[i] > bestNewFitness)
                {
                    bestNewFitness = fitneses[i];
                    bestNewSolution = solutions[i];
                }
            }

            bestFitness = bestNewFitness;
            bestSolution = bestNewSolution;

            Debug.Log("Best Fitness = " + bestFitness);
           
            //Repeat until the termintion criterion is met
        } while (targetFitness > bestFitness);
        
        printSolution(bestSolution);
    }

    IEnumerator RunGeneration()
    {
        for (int i = 0; i < populationSize; i++)
        {
            GenPrefabPopulation[i] = Instantiate(GenPrefab, Vector3.up, Quaternion.identity);
            GenPrefabPopulation[i].SetActive(false);
            GenPrefabPopulation[i].GetComponent<BotController>().gen.setWeights(childSolutions[i]);
        }

        for (int i = 0; i < populationSize; i++)
        {
            GenPrefabPopulation[i].SetActive(true);
        }

        yield return new WaitForSeconds(generationTime);

        for (int i = 0; i < populationSize; i++)
        {
            //Get fitnesses
            childFitneses[i] = GenPrefabPopulation[i].GetComponent<BotController>().gen.getFitness();
            Destroy(GenPrefabPopulation[i]);

            //Clean up Scene
            GameObject MotorSphere = FindObjectsOfType<Rigidbody>()[i].gameObject;
            if (MotorSphere.name == "MotorSphere")
            {
                Destroy(MotorSphere);
            }
        }
    }
}
