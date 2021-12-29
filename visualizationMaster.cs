using System.Collections;
using System.Collections.Generic;
using UnityEngine;

struct individualAndPosition {
    public int individualID;
    public Vector3 posToMove;
}
public class visualizationMaster : MonoBehaviour
{

    public float moveSpeed;
    public List<List<IndividualData>> populationData; // [Individuals [IndividualData]]
    public List<PlantData> plantData; 


    public List<GameObject> populationSpawned; // the population that has been spawned by spawnAnimals method
    public int totalTurns; // number of turns
    public int actualTurn; 
    public float turnDuration; // turn duration in seconds

    public List<int> individualsWhoMove; // the individuals that need to move x turn
    public GameObject wolfObject;
    public GameObject sheepObject;
    public GameObject plantObject;

    public bool waitUntilMove; // Does the program have to wait so it can advance the turn
    public bool canAdvanceTurn; 


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (individualsWhoMove.Count > 0) {
            moveIndividuals();
        } else if (waitUntilMove) {
            waitUntilMove = false;
            canAdvanceTurn = true;
        }

        if (canAdvanceTurn) {
            canAdvanceTurn = false;
            executeTurn();
        }
    }

    public void start(List<List<IndividualData>> population, List<PlantData> plants) {
        populationData = population;
        plantData = plants;
        canAdvanceTurn = true;
    }
    public void spawnIndividual() {
        for (int i = 0; i < populationData.Count(); i++) {
            gameObject animal;
            IndividualData data = populationData[i][0];
            Vector3 position = new Vector3(data.xPos, data.yPos, 0);
            if (data.type == 0) {
                animal = Instantiate(wolfObject, position, Quaternion.identity);
            } else 
                animal = Instantiate(sheepObject, position, Quaternion.identity);

            populationSpawned.Add(animal);
        }
        for (int i = 0; i < plantData.Count(); i++) {
            PlantData data = plantData[i][0];
            Vector3 position = new Vector3(data.xPos, data.yPos, 0);
            Instantiate(plantObject, position, Quaternion.identity);
        }
    }

    public void executeTurn() {
        for (int i = 0; i < populationData.Count(); i++) {
            IndividualData data = populationData[i][actualTurn];

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
        }
    }


    protected void rotateToPoint(transform obj, Vector3 position) { // rota el objeto hasta que mire a la posicion especificada
        targetRotation = Quaternion.LookRotation(position - obj.position);
        // Smoothly rotate towards the target point.
        obj.rotation = Quaternion.Slerp(obj.rotation, targetRotation, turnDuration * Time.deltaTime);
        
    }
    public void moveIndividuals(){
        for (int i = 0; i < individualsWhoMove.Count(); i++) {
            float step =  turnDuration * Time.deltaTime; // calculate distance to move
            Vector3 target = individualsWhoMove[i].posToMove;
            int id = individualsWhoMove[i].individualID;
            if (Vector3.Distance(populationSpawned[id].transform.position, target.position) >= 0.001f) {
                transform.position = Vector3.MoveTowards(populationSpawned[id].transform.position, target.position, step);
            } else {
                individualsWhoMove.RemoveAt(i);
            }
        }
    }
}
