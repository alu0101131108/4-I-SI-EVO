using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class evoMaster : MonoBehaviour
{
    public visualizationMaster IU;

    public Button buttonFrombeginning;
    public Button buttonFromIteration;
    public InputField inputIterations;
    public InputField inputGenerations;

    private int iterations;
    private int generations; // number of generations
    public float mapSize = 50f;

    // Genetic algorithm parameters.
    public int wolves = 20;
    public int sheeps = 20;
    public int plants = 10;
    public int steps = 1;
    public int elitism = 3;
    public float mutationRate = 0.01f;

    // Start is called before the first frame update
    void Start()
    {
        Button btn = buttonFrombeginning.GetComponent<Button>();
        Button btn2 = buttonFromIteration.GetComponent<Button>();
        
		btn.onClick.AddListener(startCoroutineBeginning);
		btn2.onClick.AddListener(startCoroutineIteration);
        
        List<int> populationSizes = new List<int> {wolves, sheeps, plants};
        //Simulation = new GeneticAlgor(populationSizes, elitism, mutationRate, steps, mapSize);ithm(populationSizes, elitism, mutationRate, steps, mapSize);
        GeneticAlgorithm.initialize(populationSizes, elitism, mutationRate, steps, mapSize);
    }

    
    public void startCoroutineBeginning() {
        generations = int.Parse(inputGenerations.textComponent.text);

        StartCoroutine(simulateBeginning());

    }

    public void startCoroutineIteration() {

        iterations = int.Parse(inputIterations.textComponent.text);
        generations = int.Parse(inputGenerations.textComponent.text);
        StartCoroutine(simulateFromIteration());

    }

    IEnumerator simulateBeginning() {
        for (int i = 0; i < generations; i++) {

            bool valor = false;
            //GeneticAlgorithmexecute(1);
            Debug.Log("c");

            SimulationData data = GeneticAlgorithm.getDataFromNextGeneration(1);
            Debug.Log("a");
            /*
            while (!valor) {
                valor = GeneticAlgorithmend;
                yield return new WaitForSeconds(0.1f);
            }
            */
            //IU.populationData = GeneticAlgorithmgetGeneration();
            StartCoroutine(IU.executeGeneration(data, steps));
            Debug.Log("b");

            
            while (!valor) {
                Debug.Log("In while");
                Debug.Log(IU.canAdvanceGeneration);
                valor = IU.canAdvanceGeneration;
                yield return new WaitForSeconds(0.1f);
            }
            Debug.Log("out while");
            
        }
        yield return null;
    }

    IEnumerator simulateFromIteration() {

        bool valor = false;
        SimulationData data = GeneticAlgorithm.getDataFromNextGeneration(iterations);


        /*
        while (!valor) {
           valor = GeneticAlgorithmend;
            yield return new WaitForSeconds(0.1f);
        }
        */
        IU.executeGeneration(data, steps);

        while (!valor) {
            valor = IU.canAdvanceGeneration;
            //yield return new WaitForSeconds(0.1f);
        }

        for (int i = 0; i < generations; i++) {
            bool valor2 = false;
            SimulationData data2 = GeneticAlgorithm.getDataFromNextGeneration(1);
            /*
            while (!valor) {
                valor = GeneticAlgorithmend;
                yield return new WaitForSeconds(0.1f);
            }
            */
            IU.executeGeneration(data2, steps);


            while (!valor2) {
                valor2 = IU.canAdvanceGeneration;
                //yield return new WaitForSeconds(0.1f);
            }
        }

        yield return null;

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
