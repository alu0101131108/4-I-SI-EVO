using System.Collections;
using System.Collections.Generic;

// enum Actions
// {
//   Explore,
//   Rest,
//   Hunt     // Chase a sheep (wolf only). This way the wolves can decide to keep chasing a sheep or stop doing it (Switching to Explore or Rest)
// }

public class Wolf: Individual {
  Wolf() {
    // Initialization of attributes
    Random rand = new Random(Guid.NewGuid().GetHashCode());
    float[] speedRange = {4f, 6f};
    float[] perceptionRangeRange = {10f, 20f};
    float[] sizeRange = {5f, 10f};
    
    speed = rand.NextDouble(speedRange[0], speedRange[1]);
    perceptionRange = rand.NextDouble(perceptionRangeRange[0], perceptionRangeRange[1]);
    size = rand.NextDouble(sizeRange[0], sizeRange[1]);

    maxEnergy = size * 10;
    energy = maxEnergy;
    health = maxEnergy;
    alive = true;

    updateEnergyLossRate();

    // Initialization of the brain
    brain = new NeuralNetwork(4, 2, 3, 3);  // A neural network with 4 inputs, 2 hidden layers with 3 nodes each and 3 outputs.
  }

  public void eat() {
    // Checking if the sheep is in range to eat.
    float distanceToSheep = Math.Sqrt(Math.pow(toSheep[0]) + Math.pow(toSheep[1]));

    if (distanceToSheep > EATING_RANGE) {
      // If not in range, get closer.
      // Getting absolute coordinates to the nearest plant.
      float nearestSheepX = toSheep[0] + xPos;
      float nearestSheepY = toSheep[1] + yPos;

      // Moving towards it.
      moveTowards(nearestSheepX, nearestSheepY);
    } else {
      // If in range, eat it.
      GeneticAlgorithm.kill(1, nearestSheepX, nearestSheepY);
      health += SHEEP_RESTORES;
    }
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
        case 0:               // Explore
          explore();
          break;
        case 1:               // Rest
          rest();
          break;
        case 2:               // Hunt
          eat();
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
}