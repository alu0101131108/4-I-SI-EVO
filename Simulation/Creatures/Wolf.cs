using System.Collections;
using System.Collections.Generic;

/*
enum Actions
{
  Explore,
  Rest,
  Hunt     // Chase a sheep (wolf only). This way the wolves can decide to keep chasing a sheep or stop doing it (Switching to Explore or Rest)
}
*/
public class Wolf: Individual {
  Wolf() {
    Random rand = new Random(Guid.NewGuid().GetHashCode());
    float[] speedRange = {40f, 60f};
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
    
  }
}