using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public struct individualAndPosition {
    public int individualID;
    public Vector3 posToMove;
}
public class visualizationMaster : MonoBehaviour
{

    public float moveSpeed;
    public SimulationData currentDATA;
    public List<List<IndividualData>> populationData; // [Individuals [IndividualData]]
    public List<List<Plant>> plantData; 


    public List<GameObject> populationSpawned; // the population that has been spawned by spawnAnimals method
    public int totalTurns; // number of turns
    public int actualTurn; 
    public float turnDuration; // turn duration in seconds

    public List<individualAndPosition> individualsWhoMove; // the individuals that need to move x turn
    public GameObject wolfObject;
    public GameObject sheepObject;
    public GameObject plantObject;

    public bool waitUntilMove; // Does the program have to wait so it can advance the turn
    public bool canAdvanceTurn; 
    public bool canAdvanceGeneration;

    public Text generation;
    public Text bestWolf;
    public Text bestSheep;

    public Text bestWolfSpeedText;
    public Text bestWolfPerceptionText;
    public Text bestWolfSizeText;
    public Text bestSheepSpeedText;
    public Text bestSheepPerceptionText;
    public Text bestSheepSizeText;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        /*
        if (individualsWhoMove.Count > 0) {
            moveIndividuals();
        } else if (waitUntilMove) {
            waitUntilMove = false;
            canAdvanceTurn = true;
        }
        */
        /*
        if (canAdvanceTurn) {
            canAdvanceTurn = false;
            executeTurn();
        }
        */
    }

    public void start(List<List<IndividualData>> population, List<List<Plant>> plants) {
        populationData = population;
        plantData = plants;
    }


    public void spawnIndividual() {
        for (int i = 0; i < populationData.Count; i++) {
            GameObject animal;
            IndividualData data = populationData[i][0];
            Vector3 position = new Vector3(data.xPos, data.yPos, 0);
            if (data.type == 0) {
                animal = Instantiate(wolfObject, position, Quaternion.identity);
            } else 
                animal = Instantiate(sheepObject, position, Quaternion.identity);

            populationSpawned.Add(animal);
        }
        for (int i = 0; i < plantData.Count; i++) {
            Plant data = plantData[i][0];
            Vector3 position = new Vector3(data.xPos, data.yPos, 0);
            Instantiate(plantObject, position, Quaternion.identity);
        }
    }


    private void changeTexts(SimulationData DATA) {
        generation.text = DATA.generation.ToString();
        bestWolf.text = DATA.bestWolfFitness.ToString();
        bestSheep.text = DATA.bestSheepFitness.ToString();

        bestWolfSpeedText.text = DATA.bestWolfSpeed.ToString();
        bestWolfPerceptionText.text = DATA.bestWolfPerception.ToString();
        bestWolfSizeText.text = DATA.bestWolfSize.ToString();
        bestSheepSpeedText.text = DATA.bestSheepSpeed.ToString();
        bestSheepPerceptionText.text = DATA.bestSheepPerception.ToString();
        bestSheepSizeText.text = DATA.bestSheepSize.ToString();
    }
    public IEnumerator executeGeneration(SimulationData DATA, int maxTurns) {

        start(DATA.populationData, DATA.plantsData);
        waitUntilMove = false;
        canAdvanceTurn = false;
        for (int k = 0; k < maxTurns; k++) {

            
            for (int i = 0; i < populationData.Count; i++) {
                IndividualData data = populationData[i][actualTurn];
                if (data.alive) {

                
                    if (data.action == 0 || data.action == 3) {
                        IndividualData dataNextTurn = populationData[i][actualTurn + 1];

                        Vector3 position = new Vector3(dataNextTurn.xPos, dataNextTurn.yPos, 0);

                        individualAndPosition indv;

                        indv.individualID = i;
                        indv.posToMove = position;
                        individualsWhoMove.Add(indv);
                        rotateToPoint(populationSpawned[i].transform, position);
                        waitUntilMove = true;
                    }
                } else {
                    populationData.RemoveAt(i);
                    GameObject animal = populationSpawned[i];
                    populationSpawned.RemoveAt(i);
                    Destroy(animal);
                }

                
            }
            
            while (!canAdvanceTurn) {
                Debug.Log("trying to advance turn");
                Debug.Log(individualsWhoMove.Count);
                Debug.Log(waitUntilMove);
                if (individualsWhoMove.Count > 0) {
                    moveIndividuals();
                } else if (waitUntilMove) {
                    waitUntilMove = false;
                    canAdvanceTurn = true;
                }
            }
            actualTurn++;
            Debug.Log("Termino turno visual");
        }
        canAdvanceGeneration = true;
        Debug.Log("gen terminada");
        yield return null;

    }


    protected void rotateToPoint(Transform obj, Vector3 position) { // rota el objeto hasta que mire a la posicion especificada
        Quaternion targetRotation = Quaternion.LookRotation(position - obj.position);
        // Smoothly rotate towards the target point.
        obj.rotation = Quaternion.Slerp(obj.rotation, targetRotation, turnDuration * Time.deltaTime);
        
    }
    public void moveIndividuals(){
        for (int i = 0; i < individualsWhoMove.Count; i++) {
            float step =  turnDuration * Time.deltaTime; // calculate distance to move
            Vector3 target = individualsWhoMove[i].posToMove;
            int id = individualsWhoMove[i].individualID;
            if (Vector3.Distance(populationSpawned[id].transform.position, target) >= 0.001f) {
                transform.position = Vector3.MoveTowards(populationSpawned[id].transform.position, target, step);
            } else {
                individualsWhoMove.RemoveAt(i);
            }
        }
    }
}
