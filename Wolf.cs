using System.Collections;
using System.Collections.Generic;

enum Actions
{
  Explore,
  Rest,
  Hunt     // Chase a prey (predator only). This way the wolves can decide to keep chasing a prey or stop doing it (Switching to Explore or Rest)
}

public class Wolf: Individual {
  Wolf() {
    Random rand = new Random(Guid.NewGuid().GetHashCode());
    double[] speedRange = {0.4, 0.6};
    double[] perceptionRangeRange = {10, 20};
    double[] sizeRange = {5, 10};
    
    speed = rand.NextDouble(speedRange[0], speedRange[1]);
    perceptionRange = rand.NextDouble(perceptionRangeRange[0], perceptionRangeRange[1]);
    size = rand.NextDouble(sizeRange[0], sizeRange[1]);

    health = size * 10;
    energy = health;
    alive = true;

    energyLossRate = speed * perceptionRange * size / 50;
  }

  public void eat() {
    health += 20;

  }
}