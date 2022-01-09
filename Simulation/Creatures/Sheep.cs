using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// enum Actions
// {
//   Explore,
//   Rest,
//   Eat,      // Goes after a plant to eat it
//   Flee      // Run away from wolf (sheep only)
// }
[System.Serializable]
public class Sheep: Individual {
  public Sheep(float x, float y) {
    // Position initialization
    xPos = x;
    yPos = y;

    // Initialization of attributes
    toWolf = new List<float> {0f, 0f};
    toSheep = new List<float> {0f, 0f};
    toPlant = new List<float> {0f, 0f};

    float[] speedRange = {3f, 4f};
    float[] perceptionRangeRange = {10f, 20f};
    float[] sizeRange = {10f, 15f};
    
    speed = Random.Range(speedRange[0], speedRange[1]);
    perceptionRange = Random.Range(perceptionRangeRange[0], perceptionRangeRange[1]);
    size = Random.Range(sizeRange[0], sizeRange[1]);

    maxEnergy = size * 10;
    energy = maxEnergy;
    health = maxEnergy;
    alive = true;

    updateEnergyLossRate();

    // Initialization of the brain
    brain = new NeuralNetwork(4, 2, 3, 4);  // A neural network with 4 inputs, 2 hidden layers with 3 nodes each and 4 outputs.
  }

  public void eat() {
    // Checking if the plant is in range to eat.
    float distanceToPlant = Mathf.Sqrt(Mathf.Pow(toPlant[0], 2) + Mathf.Pow(toPlant[1], 2));
    // Getting absolute coordinates to the nearest plant.
    float nearestPlantX = toPlant[0] + xPos;
    float nearestPlantY = toPlant[1] + yPos;

    if (distanceToPlant > EATING_RANGE) {
      // If not in range, get closer.
      // Moving towards it.
      moveTowards(nearestPlantX, nearestPlantY);
    } else {
      // If in range, eat it.
      GeneticAlgorithm.kill(2, nearestPlantX, nearestPlantY);
      health += PLANT_RESTORES;
    }
  }

  public void flee() {
    // Getting absolute coordinates to the nearest wolf.
    float nearestWolfX = toWolf[0] + xPos;
    float nearestWolfY = toWolf[1] + yPos;

    // In flee mode, the sheep will move 150% faster and therefore lose more energy
    float originalSpeed = speed;
    speed += 0.5f * speed;
    updateEnergyLossRate();

    // Moving towards the opposite direction.
    moveTowards(-nearestWolfX, -nearestWolfY);

    // Resetting speed and energy usage.
    speed = originalSpeed;
    updateEnergyLossRate();
  }

  public void actuate() {
    if (alive) {
      float perceptionBound = Mathf.Sqrt(Mathf.Pow(perceptionRange, 2) + Mathf.Pow(perceptionRange, 2));
      float remappedEnergy = StaticMath.Remap(energy, 0, maxEnergy, 0, 1);
      float remappedDistanceToWolf = StaticMath.Remap(StaticMath.GetDistBetweenPoints(0, 0, toWolf[0], toWolf[1]), 0, perceptionBound, 0, 1);
      float remappedDistanceToSheep = StaticMath.Remap(StaticMath.GetDistBetweenPoints(0, 0, toSheep[0], toSheep[1]), 0, perceptionBound, 0, 1);
      float remappedDistanceToPlant = StaticMath.Remap(StaticMath.GetDistBetweenPoints(0, 0, toPlant[0], toPlant[1]), 0, perceptionBound, 0, 1);
      float[] brainInputs = {remappedEnergy, remappedDistanceToWolf, remappedDistanceToSheep, remappedDistanceToPlant};
      float[] brainOutputs = brain.FeedForward(brainInputs);
      int prediction = StaticMath.FindMaxValueIndex(new List<float>(brainOutputs));
      switch (prediction) {
        case 0:
          explore();
          break;
        case 1:
          rest();
          break;
        case 2:
          eat();
          break;
        case 3:
          flee();
          break;
        default:
          break;
      }
      health -= 5;
      fitness += health;
      if (health <= 0) {
        die();
      }
    }
  }

  public Individual Crossover(Individual otherParent, float mutationPercent) {
    float[] parentAttributes = {speed, perceptionRange, size};
    float[] otherParentAttributes = {otherParent.speed, otherParent.perceptionRange, otherParent.size};
    float[] childAttributes = new float[3];

    int crossingPoint = Random.Range(1, 2);
    for (int i = 0; i < parentAttributes.Length; i++) {
      if (i < crossingPoint) {
        childAttributes[i] = parentAttributes[i];
      } else {
        childAttributes[i] = otherParentAttributes[i];
      }
      if (Random.Range(0f, 1f) <= mutationPercent) {
        childAttributes[i] *= Random.Range(0.7f, 1.3f);
      }
    }

    Sheep child = new Sheep(Random.Range(0f, GeneticAlgorithm.mapSize), Random.Range(0f, GeneticAlgorithm.mapSize));
    child.speed = childAttributes[0];
    child.perceptionRange = childAttributes[1];
    child.size = childAttributes[2];
    child.brain = NeuralNetwork.Crossover(brain, otherParent.brain, mutationPercent);

    return child;
  }
}