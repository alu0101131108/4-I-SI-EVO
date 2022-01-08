using System.Collections;
using System.Collections.Generic;


enum Actions
{
  Explore,
  Rest,
  Eat,      // Goes after a plant to eat it
  Flee      // Run away from wolf (sheep only)
}

public class Sheep: Individual {
  Sheep() {
    Random rand = new Random(Guid.NewGuid().GetHashCode());
    float[] speedRange = {30f, 40f};
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

    brain = NeuralNetwork();
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

  }
}