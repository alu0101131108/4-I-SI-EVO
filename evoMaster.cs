using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class evoMaster : MonoBehaviour
{


    public visualizationMaster IU;
    public GeneticAlgorithm Simulation;

    public button simulateFrombeginning;
    public button simulateFromIteration;
    public InputField inputIterations;
    public InputField inputGenerations;

    private int iterations;
    private int generations; // number of generations
    // Start is called before the first frame update
    void Start()
    {
        Button btn = simulateFrombeginning.GetComponent<Button>();
        Button btn2 = simulateFromIteration.GetComponent<Button>();
        
		btn.onClick.AddListener(startCoroutineBeginning);
		btn2.onClick.AddListener(startCoroutineIteration);
        
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
            Simulation.execute(1);
            /*
            while (!valor) {
                valor = Simulation.end;
                yield return new WaitForSeconds(0.1f);
            }
            */
            //IU.populationData = Simulation.getGeneration();
            IU.executeTurn();

        }

    }

    IEnumerator simulateFromIteration() {

        bool valor = false;
        SimulationData DATA = Simulation.execute(iteration);

        /*
        while (!valor) {
           valor = Simulation.end;
            yield return new WaitForSeconds(0.1f);
        }
        */
        IU.executeTurn();

        for (int i = 0; i < generations; i++) {
            bool valor = false;
            Simulation.execute(1);
            /*
            while (!valor) {
                valor = Simulation.end;
                yield return new WaitForSeconds(0.1f);
            }
            */
            IU.executeTurn();
        }

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
