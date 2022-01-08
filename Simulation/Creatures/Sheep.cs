using System.Collections;
using System.Collections.Generic;

<<<<<<< HEAD

enum Actions
{
  Explore,
  Rest,
  Eat,      // Goes after a plant to eat it
  Flee      // Run away from wolf (sheep only)
}
=======
// enum Actions
// {
//   Explore,
//   Rest,
//   Eat,      // Goes after a plant to eat it
//   Flee      // Run away from wolf (sheep only)
// }
>>>>>>> 03b3f1c... pingo

public class Sheep: Individual {
  Sheep() {
    // Initialization of attributes
    Random rand = new Random(Guid.NewGuid().GetHashCode());
    float[] speedRange = {3f, 4f};
    float[] perceptionRangeRange = {10f, 20f};
    float[] sizeRange = {10f, 15f};
    
    speed = rand.NextDouble(speedRange[0], speedRange[1]);
    perceptionRange = rand.NextDouble(perceptionRangeRange[0], perceptionRangeRange[1]);
    size = rand.NextDouble(sizeRange[0], sizeRange[1]);

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
    float distanceToPlant = Math.Sqrt(Math.pow(toPlant[0]) + Math.pow(toPlant[1]));

    if (distanceToPlant > EATING_RANGE) {
      // If not in range, get closer.
      // Getting absolute coordinates to the nearest plant.
      float nearestPlantX = toPlant[0] + xPos;
      float nearestPlantY = toPlant[1] + yPos;

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
    speed += 0.5 * speed;
    updateEnergyLossRate();

    // Moving towards the opposite direction.
    moveTowards(-nearestWolfX, -nearestWolfY);

    // Resetting speed and energy usage.
    speed = originalSpeed;
    updateEnergyLossRate();
  }

  public void actuate() {
    if (alive) {
      float perceptionBound = Math.Sqrt(Math.Pow(perceptionRange, 2) + Math.Pow(perceptionRange, 2));
      float remappedEnergy = StaticMath.Remap(energy, 0, maxEnergy, 0, 1);
      float remappedDistanceToWolf = StaticMath.Remap(StaticMath.GetDistBetweenPoints(0, 0, toWolf[0], toWolf[1]), 0, perceptionBound, 0, 1);
      float remappedDistanceToSheep = StaticMath.Remap(StaticMath.GetDistBetweenPoints(0, 0, toSheep[0], toSheep[1]), 0, perceptionBound, 0, 1);
      float remappedDistanceToPlant = StaticMath.Remap(StaticMath.GetDistBetweenPoints(0, 0, toPlant[0], toPlant[1]), 0, perceptionBound, 0, 1);
      float[] brainInputs = {remappedEnergy, remappedDistanceToWolf, remappedDistanceToSheep, remappedDistanceToPlant};
      float[] brainOutputs = brain.FeedForward(brainInputs);
      int prediction = StaticMath.FindMaxValueIndex(brainOutputs);
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
}