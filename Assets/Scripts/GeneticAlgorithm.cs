using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticAlgorithm : MonoBehaviour
{
    [Header("Genetic Settings")]
    public int populationSize = 10;
    public int generationTime = 20;
    public float targetFitness = 20;

    [Range(0f,1f)]
    public float mutationRate = .01f;
    [Range(0f, 1f)]
    public float parentSelectionRate = .5f;
    public GameObject prefab;
    public Vector3[] startPositions;
    public Vector3[] startRotations;

    private List<GameObject> prefabList;
    private static int solutionSize = 32;
    private double prevFitness = 0;
    private Gen bestGen;

    private Genetic genetic;

    //Inicializar Variables
    public void Start()
    {
        genetic = new Genetic(populationSize, mutationRate, parentSelectionRate, solutionSize);

        prefabList = new List<GameObject>(populationSize*2);
        for (int i = 0; i < populationSize*2; i++)
        {
            prefabList.Add(null);
        }

        bestGen = new Gen(new double[solutionSize], "");

        StartCoroutine(run());
    }

    IEnumerator run()
    {
        yield return StartCoroutine(RunGenetic());
        

        Debug.Log("\nBest Solution = " + genetic.SolutionToString(bestGen.weights));
        Debug.Log("\nBest Fitness = " + bestGen.fitness);

        GameObject test_0 = Instantiate(prefab, startPositions[0], Quaternion.Euler(startRotations[0]));
        test_0.GetComponent<BotController>().gen = bestGen;

        GameObject test_1 = Instantiate(prefab, startPositions[1], Quaternion.Euler(startRotations[1]));
        test_1.GetComponent<BotController>().gen = bestGen;

        GameObject test_2 = Instantiate(prefab, startPositions[2], Quaternion.Euler(startRotations[2]));
        test_2.GetComponent<BotController>().gen = bestGen;

    }

    IEnumerator RunGenetic()
    {
        List<Gen> solutionsList;
        List<Gen> childSolutionsList;
        prevFitness = 0;
        bestGen.fitness = 0;

        solutionsList = genetic.CreateRandomSolutions();

        //Evaluate Solutions
        yield return StartCoroutine(runGeneration(solutionsList));

        do
        {
            prevFitness = bestGen.fitness;

            childSolutionsList = genetic.CreateChildSolutions(solutionsList);

            //Evaluate Children Solutions
            yield return StartCoroutine(runGeneration(childSolutionsList));

            solutionsList = genetic.CombineSolutions(solutionsList, childSolutionsList);

            bestGen = new Gen(solutionsList[0].weights, "", solutionsList[0].fitness);

            Debug.Log("\nBest Fitness = " + bestGen.fitness);
            Debug.Log("Best Solution = " + genetic.SolutionToString(bestGen.weights));

            //Repeat until the termintion criterion is met
        } while (!(targetFitness < bestGen.fitness && bestGen.fitness == prevFitness));

        
    }

    IEnumerator runGeneration(List<Gen> generation)
    {

        for (int i = 0; i < populationSize; i+=2)
        {
            prefabList[i] = Instantiate(prefab, startPositions[0], Quaternion.Euler(startRotations[0]));
            prefabList[i+1] = Instantiate(prefab, startPositions[1], Quaternion.Euler(startRotations[1]));
            prefabList[i].GetComponent<BotController>().gen = generation[i];
            prefabList[i+1].GetComponent<BotController>().gen = generation[i];
        }

        yield return new WaitForSeconds(generationTime);

        for (int i = 0; i < populationSize; i+=2)
        {
            generation[i] = prefabList[i].GetComponent<BotController>().gen;
        }

        for (int i = 0; i < populationSize; i++)
        {
            Destroy(prefabList[i].GetComponent<BotController>().sphereRB.gameObject);
            Destroy(prefabList[i]);
        }
    }
}
