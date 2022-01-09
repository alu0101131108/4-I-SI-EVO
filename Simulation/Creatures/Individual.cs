using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Individual
{
    public const float EATING_RANGE = 5f;                // Necessary distance to eat something.
    public const float PLANT_RESTORES = 20f;             // How much health does a plant restore when eaten.
    public const float SHEEP_RESTORES = 30f;             // How much health does a sheep restore when eaten.
    public const float ENERGY_RESTORES = 10f;            // How much energy is gained while resting.
    public const float ENERGY_LOSS_MULTIPLIER = 0.02f;    // Adjustment to the energy loss rate.
    public const float HEALTH_LOSS_RATE = 5f;
    
    public NeuralNetwork brain;   // NN that defines behaviour by deciding actions based on inputs.

    public float xPos;            // X Coordinate.
    public float yPos;            // Y Coordinate.
    public int action;            // Current state decided by the brain.
    public bool alive;            // Living status.
    public float energyLossRate;  // Rate at which energy will be lowered when moving.
    public float fitness;         // How fit is the individual to survive. Average health through all turns.
    public float maxEnergy;       // The maximum energy of an individual. Calculated as size x 10.

    // The fitness of the individual will be based on its health.
    public float health;          // Always decreases, increases when eats food. Healthiest individuals will succeed in natural selection.

    // Statistics - Brain will have these as inputs.
    public float energy;          // Movement decreases it and regenerates with time.
    public List<float> toWolf;        // Contains vector to nearest wolf.
    public List<float> toSheep;       // Contains vector to nearest sheep.
    public List<float> toPlant;       // Contains vector to nearest food.

    // Attributes - Performance of actions depend on these.
    public float speed;             // Moves faster but increase energy usage.
    public float perceptionRange;   // Sees further but increase energy usage.
    public float size;              // Increases initial health but increases energy usage.

    public Individual() {
      toWolf = new List<float> {0f, 0f};
      toSheep = new List<float> {0f, 0f};
      toPlant = new List<float> {0f, 0f};
  }
    
    // Updates the vectors to the nearest sheeps, wolves and plants at current position.
    public void updatePerception() {
      List<List<float>> perceivedObjects = GeneticAlgorithm.getPerceptionFrom(xPos, yPos, perceptionRange);
      toWolf = perceivedObjects[0];
      toSheep = perceivedObjects[1];
      toPlant = perceivedObjects[2];
    }
    
    // Walks to a random position inside its range.
    public void explore() {
      moveTowards(Random.Range(xPos - perceptionRange, xPos + perceptionRange),
                  Random.Range(yPos - perceptionRange, yPos + perceptionRange));
    }

    // Regenerates energy while not moving.
    public void rest() {
      energy += ENERGY_RESTORES;
      if (energy > maxEnergy) {
        energy = maxEnergy;
      }
    }

    // The animal dies.
    public void die() {
      alive = false;
      health = 0;
    }

    // The energy loss rate is updated with the current attributes.
    public void updateEnergyLossRate() {
      energyLossRate = speed * perceptionRange * size * ENERGY_LOSS_MULTIPLIER;
    }
    
    // Moves n units towards (x, y). N depends on speed.
    public void moveTowards(float x, float y) {
      if (energy >= energyLossRate) {
        // The two components of the vector starting in the individual position and pointing to the destination
        float diffX = x - xPos;
        float diffY = y - yPos;

        // The module of the previous vector
        float modVector = Mathf.Sqrt(Mathf.Pow(diffX, 2) + Mathf.Pow(diffY, 2));

        // The two components of the unit vector of the previous vector.
        float unitVectorX = diffX / modVector;
        float unitVectorY = diffY / modVector;

        // Moving in the specified direction according to the individual's speed
        // Using Math.Min just in case the movement surpasses the specified point
        xPos += Mathf.Min(speed * unitVectorX, diffX);
        yPos += Mathf.Min(speed * unitVectorY, diffY);
        energy -= energyLossRate;
      }
    }

    public void restart() {
      alive = true;
      health = maxEnergy;
      energy = maxEnergy;
      xPos = Random.Range(0f, GeneticAlgorithm.mapSize);
      yPos = Random.Range(0f, GeneticAlgorithm.mapSize);
    }

    // Virtual methods. Sheep and Wolf will each contain its own definition of these.
    public virtual void actuate() {}          // First think, then do action.
    public virtual void eat() {}              // Goes after food within its range if there's any.
    public virtual Individual Crossover(Individual otherParent, float mutationPercent) {
      Debug.Log("Esto no se deberia ejecutar aqui");
      return otherParent;
    } // Creates a child.

    // Methods related to the Genetic Algorithm.
    public float CalculateFitness(int steps) {
      fitness /= steps;
      return fitness;
    }
}
