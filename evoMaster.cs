using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class evoMaster : MonoBehaviour
{


    public visualizationMaster IU;
    //public GeneticAlgorithm Simulation;

    public Button buttonFromBeginning;
    public Button buttonFromIteration;
    public InputField inputIterations;
    public InputField inputGenerations;

    private int iterations;
    private int generations; // number of generations

    // things for genetic algorithm

    public int wolves = 20;
    public int sheeps = 20;
    public int plants = 10;
    public int steps = 50;
    public int elitism = 3;
    public float mutationRate = 0.01f;
    // Start is called before the first frame update
    void Start()
    {
        Button btn = buttonFromBeginning.GetComponent<Button>();
        Button btn2 = buttonFromIteration.GetComponent<Button>();
        
		btn.onClick.AddListener(startCoroutineBeginning);
		btn2.onClick.AddListener(startCoroutineIteration);
        
        List<int> populationSizes = new List<int> {wolves, sheeps, plants};
        GeneticAlgorithm EVO = new GeneticAlgorithm(populationSizes, elitism, mutationRate, steps);
    }

    
    public void startCoroutineBeginning() {
        generations = Int32.Parse(inputGenerations.textComponent.text);

        StartCoroutine();

    }

    public void startCoroutineIteration() {

        iterations = Int32.Parse(inputIterations.textComponent.text);
        generations = Int32.Parse(inputGenerations.textComponent.text);
        startCoroutineIteration();

    }

    IEnumerator simulateBeginning() {
        for (int i = 0; i < generations; i++) {

            bool valor = false;
            //Simulation.execute(1);
            SimulationData data = Simulation.getDataFromNextGeneration(1);
            /*
            while (!valor) {
                valor = Simulation.end;
                yield return new WaitForSeconds(0.1f);
            }
            */
            //IU.populationData = Simulation.getGeneration();
            IU.executeGeneration();

            while (!valor) {
                valor = IU.canAdvanceGeneration;
                //yield return new WaitForSeconds(0.1f);
            }
        }

    }

    IEnumerator simulateFromIteration() {

        bool valor = false;
        SimulationData data = Simulation.getDataFromNextGeneration(generations);


        /*
        while (!valor) {
           valor = Simulation.end;
            yield return new WaitForSeconds(0.1f);
        }
        */
        IU.executeGeneration(SimulationData, steps);

        while (!valor) {
            valor = IU.canAdvanceGeneration;
            //yield return new WaitForSeconds(0.1f);
        }

        for (int i = 0; i < generations; i++) {
            bool valor = false;
            SimulationData data = Simulation.getDataFromNextGeneration(1);
            /*
            while (!valor) {
                valor = Simulation.end;
                yield return new WaitForSeconds(0.1f);
            }
            */
            IU.executeGeneration(SimulationData, steps);


            while (!valor) {
                valor = IU.canAdvanceGeneration;
                //yield return new WaitForSeconds(0.1f);
            }
        }

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
